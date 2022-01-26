using Services.DtoModels;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using CsvHelper;
using System.Globalization;
using Models;
using System.Text;
using System.Threading.Tasks;
using Data;
using System.IO;
using Services.Implememtations;
using Models.Interfaces;

namespace Services.Implementations
{
    public class FileReader : IFileReader
    {
        public FileReader(TotalErrorDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public TotalErrorDbContext DbContext { get; }
        public Dictionary<string, List<TransferModel>> ReadFileFromDir(string dir)
        {
            string[] files = Directory.GetFiles(dir);
            List<TransferModel> models = new List<TransferModel>();
            Dictionary<string, List<TransferModel>> trModelDates = new Dictionary<string, List<TransferModel>>();
            var lastReadDate = this.DbContext.LastReadFiles.OrderByDescending(x => x.LastReadFileDateTime);

            foreach (var currFile in files)
            {
                string fileName = Path.GetFileNameWithoutExtension(currFile);
                using (var reader = new StreamReader(currFile))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    bool isRead = lastReadDate.Count() > 0 && lastReadDate.First().LastReadFileDateTime < DateTime.Parse(fileName);
                    bool anyFilesRead = lastReadDate.Count() > 0;
                    if (!isRead || !anyFilesRead)
                    {
                        models = csv.GetRecords<TransferModel>().ToList();
                        trModelDates[fileName] = models;
                    }
                }
            }
            return trModelDates;
        }
        public DataObject Convert(Dictionary<string, List<TransferModel>> transferModelsByFile)
        {
            Region region;
            Country country;
            ItemType itemType;
            Order order;
            Sale sale;

            HashSet<Region> dbRegions = this.DbContext.Regions.ToHashSet();
            HashSet<Country> dbCountries = this.DbContext.Countries.ToHashSet();
            HashSet<ItemType> dbItemTypes = this.DbContext.ItemTypes.ToHashSet();
            HashSet<Order> dbOrders = this.DbContext.Orders.ToHashSet();
            HashSet<Sale> dbSales = this.DbContext.Sales.ToHashSet();

            HashSet<Region> tmpRegions = new HashSet<Region>();
            HashSet<Country> tmpCountries = new HashSet<Country>();
            HashSet<ItemType> tmpItemTypes = new HashSet<ItemType>();
            HashSet<Order> tmpOrders = new HashSet<Order>();
            HashSet<Sale> tmpSales = new HashSet<Sale>();

            Queue<string> dates = new Queue<string>();
            foreach (string date in transferModelsByFile.Keys)
            {
                dates.Enqueue(date);
            }
            foreach (List<TransferModel> trModelsList in transferModelsByFile.Values)
            {
                string fileDate = dates.Dequeue();
                foreach (TransferModel transferModel in trModelsList)
                {
                    List<Sale> sales = new List<Sale>();
                    order = dbOrders.FirstOrDefault(x => x.Id == transferModel.OrderId);
                    if (order is null)
                    {
                        order = tmpOrders.FirstOrDefault(x => x.Id == transferModel.OrderId);
                        if (order is null)
                        {
                            order = new Order();
                            itemType = dbItemTypes.FirstOrDefault(t => t.ItemTypeName == transferModel.ItemType);
                            if (itemType is null)
                            {
                                itemType = tmpItemTypes.FirstOrDefault(t => t.ItemTypeName == transferModel.ItemType);
                                if (itemType is null)
                                {
                                    ItemType newItem = new ItemType()
                                    {
                                        ItemTypeName = transferModel.ItemType
                                    };
                                    tmpItemTypes.Add(newItem);
                                    itemType = newItem;
                                }
                            }
                            List<TransferModel> salesInOrder = trModelsList.Where(t => t.OrderId == transferModel.OrderId).ToList();
                            foreach (TransferModel saleInOrder in salesInOrder)
                            {
                                sale = dbSales.FirstOrDefault(s => DateTime.Equals(s.ShipDate,
                                    DateTime.ParseExact(transferModel.ShipDate, "M/d/yyyy", CultureInfo.InvariantCulture))
                                && s.TotalProfit == Decimal.Parse(transferModel.TotalProfit));
                                if (sale is null)
                                {
                                    sale = tmpSales.FirstOrDefault(s => DateTime.Equals(s.ShipDate,
                                    DateTime.ParseExact(transferModel.ShipDate, "M/d/yyyy", CultureInfo.InvariantCulture))
                                    && s.TotalProfit == Decimal.Parse(transferModel.TotalProfit));
                                    if (sale is null)
                                    {
                                        Sale newSale = new Sale()
                                        {
                                            ShipDate = DateTime.ParseExact(transferModel.ShipDate, "M/d/yyyy", CultureInfo.InvariantCulture),
                                            UnitsSold = int.Parse(transferModel.UnitsSold),
                                            UnitPrice = Decimal.Parse(transferModel.UnitPrice),
                                            UnitCost = Decimal.Parse(transferModel.UnitCost),
                                            TotalRevenue = Decimal.Parse(transferModel.TotalRevenue),
                                            TotalCost = Decimal.Parse(transferModel.TotalCost),
                                            TotalProfit = Decimal.Parse(transferModel.TotalProfit),
                                            Order = order,
                                            ItemType = itemType,
                                            FileDate = fileDate
                                        };
                                        sale = newSale;
                                        sales.Add(sale);
                                        tmpSales.Add(newSale);
                                    }
                                }
                            }

                            country = dbCountries.FirstOrDefault(c => c.CountryName == transferModel.Country);
                            if (country is null)
                            {
                                country = tmpCountries.FirstOrDefault(c => c.CountryName == transferModel.Country);
                                if (country is null)
                                {
                                    region = dbRegions.FirstOrDefault(r => r.RegionName == transferModel.Region);

                                    if (region is null)
                                    {
                                        region = tmpRegions.FirstOrDefault(r => r.RegionName == transferModel.Region);
                                        if (region is null)
                                        {
                                            Region newRegion = new Region()
                                            {
                                                RegionName = transferModel.Region
                                            };

                                            tmpRegions.Add(newRegion);

                                            region = newRegion;
                                        }
                                    }
                                    Country newCountry = new Country()
                                    {
                                        CountryName = transferModel.Country,
                                        Region = region,
                                    };

                                    tmpCountries.Add(newCountry);

                                    country = newCountry;

                                }
                            }
                            order.Id = transferModel.OrderId;
                            order.OrderPriority = transferModel.OrderPriority;
                            order.OrderDate = DateTime.ParseExact(transferModel.OrderDate, "M/d/yyyy", CultureInfo.InvariantCulture);
                            order.SalesChannel = transferModel.SalesChannel;
                            order.Sales = sales;
                            order.Country = country;
                            order.FileDate = fileDate;

                            tmpOrders.Add(order);
                        }
                    }
                }
            }
            DataObject data = new DataObject();
            data.Countries = tmpCountries;
            data.ItemTypes = tmpItemTypes;
            data.Orders = tmpOrders;
            data.Regions = tmpRegions;
            data.Sales = tmpSales;

            return data;
        }



        public void SaveDataToDatabase(DataObject data)
        {
            this.DbContext.ItemTypes.AddRange(data.ItemTypes);
            this.DbContext.Countries.AddRange(data.Countries);
            this.DbContext.Regions.AddRange(data.Regions);
            this.DbContext.Sales.AddRange(data.Sales);
            this.DbContext.Orders.AddRange(data.Orders);

            List<LastReadFile> dates = new List<LastReadFile>();

            foreach (DateTime fileName in data.LastReadFiles)
            {
                LastReadFile lastReadFile = new LastReadFile()
                {
                    LastReadFileDateTime = fileName
                };

                dates.Add(lastReadFile);
            }

            this.DbContext.LastReadFiles.AddRange(dates);

            DbContext.SaveChanges();
        }

        DataObject IFileReader.Convert(Dictionary<string, List<TransferModel>> transferModelsByFile)
        {
            throw new NotImplementedException();
        }

    }
}