using AutoMapper;
using Data;
using Models;
using Models.Interfaces;
using Services.DtoModels;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.MainServices
{
    public class OrdersMainService : IOrdersMainService
    {
        public OrdersMainService(TotalErrorDbContext dbContext, IMapper mapper)
        {
            this.DbContext = dbContext;
            this.Mapper = mapper;
        }

        public TotalErrorDbContext DbContext { get; set; }

        public IMapper Mapper { get; set; }

        public List<OrderDto> GetOrders()
        {
            this.Mapper.Map<List<CountryDto>>(this.DbContext.Countries.Where(c => c.IsDeleted == false).ToList());
            this.Mapper.Map<List<SaleDto>>(this.DbContext.Sales.Where(s => s.IsDeleted == false).ToList());
            this.Mapper.Map<List<ItemTypeDto>>(this.DbContext.ItemTypes.Where(it => it.IsDeleted == false).ToList());
            var orders = this.Mapper.Map<List<OrderDto>>(this.DbContext.Orders.Where(o => o.IsDeleted == false).ToList());

            return orders;
        }

        public List<IGrouping<RegionDto, OrderDto>> GetOrdersGroupedByRegion(out Dictionary<RegionDto, decimal> regionsTotalCost,
            out Dictionary<RegionDto, decimal> regionsTotalProfit)
        {
            this.Mapper.Map<List<CountryDto>>(this.DbContext.Countries.Where(c => c.IsDeleted == false).ToList());
            this.Mapper.Map<List<RegionDto>>(this.DbContext.Regions.Where(r => r.IsDeleted == false).ToList());
            this.Mapper.Map<List<SaleDto>>(this.DbContext.Sales.Where(s => s.IsDeleted == false).ToList());
            this.Mapper.Map<List<ItemTypeDto>>(this.DbContext.ItemTypes.Where(it => it.IsDeleted == false).ToList());
            var result = this.Mapper.Map<List<OrderDto>>(this.DbContext.Orders.Where(o => o.IsDeleted == false).ToList());
            var groupedByRegionResult = result.GroupBy(r => r.Country.Region).ToList();

            regionsTotalCost = new Dictionary<RegionDto, decimal>();
            regionsTotalProfit = new Dictionary<RegionDto, decimal>();
            foreach (var region in groupedByRegionResult)
            {
                regionsTotalCost[region.Key] = 0;
                regionsTotalProfit[region.Key] = 0;
            }

            foreach (var group in groupedByRegionResult)
            {
                var orders = group.ToList();

                foreach (OrderDto order in orders)
                {
                    foreach (SaleDto sale in order.Sales)
                    {
                        regionsTotalCost[group.Key] += sale.TotalCost;
                        regionsTotalProfit[group.Key] += sale.TotalProfit;
                    }

                }
            }

            return groupedByRegionResult;
        }

        public List<IGrouping<CountryDto, OrderDto>> GetOrdersGroupedByCountry(out Dictionary<CountryDto, decimal> countriesTotalCost,
            out Dictionary<CountryDto, decimal> countriesTotalProfit)
        {
            this.Mapper.Map<List<CountryDto>>(this.DbContext.Countries.Where(c => c.IsDeleted == false).ToList());
            this.Mapper.Map<List<SaleDto>>(this.DbContext.Sales.Where(s => s.IsDeleted == false).ToList());
            this.Mapper.Map<List<ItemTypeDto>>(this.DbContext.ItemTypes.Where(it => it.IsDeleted == false).ToList());
            var result = this.Mapper.Map<List<OrderDto>>(this.DbContext.Orders.Where(o => o.IsDeleted == false).ToList());
            var groupedByCountryResult = result.GroupBy(c => c.Country).ToList();

            countriesTotalCost = new Dictionary<CountryDto, decimal>();
            countriesTotalProfit = new Dictionary<CountryDto, decimal>();
            foreach (var country in groupedByCountryResult)
            {
                countriesTotalCost[country.Key] = 0;
                countriesTotalProfit[country.Key] = 0;
            }

            foreach (var group in groupedByCountryResult)
            {
                var orders = group.ToList();

                foreach (OrderDto order in orders)
                {
                    foreach (SaleDto sale in order.Sales)
                    {
                        countriesTotalCost[group.Key] += sale.TotalCost;
                        countriesTotalProfit[group.Key] += sale.TotalProfit;
                    }

                }
            }

            return groupedByCountryResult;
        }

        public List<IGrouping<DateTime, OrderDto>> GetOrdersGroupedByOrderDate(out Dictionary<DateTime, decimal> datesTotalCost,
            out Dictionary<DateTime, decimal> datesTotalProfit)
        {
            this.Mapper.Map<List<CountryDto>>(this.DbContext.Countries.Where(c => c.IsDeleted == false).ToList());
            this.Mapper.Map<List<SaleDto>>(this.DbContext.Sales.Where(s => s.IsDeleted == false).ToList());
            this.Mapper.Map<List<ItemTypeDto>>(this.DbContext.ItemTypes.Where(it => it.IsDeleted == false).ToList());
            var result = this.Mapper.Map<List<OrderDto>>(this.DbContext.Orders.Where(o => o.IsDeleted == false).ToList());
            var groupedByOrderDateResult = result.GroupBy(od => od.OrderDate).ToList();

            datesTotalCost = new Dictionary<DateTime, decimal>();
            datesTotalProfit = new Dictionary<DateTime, decimal>();
            foreach (var date in groupedByOrderDateResult)
            {
                datesTotalCost[date.Key] = 0;
                datesTotalProfit[date.Key] = 0;
            }

            foreach (var group in groupedByOrderDateResult)
            {
                var orders = group.ToList();

                foreach (OrderDto order in orders)
                {
                    foreach (SaleDto sale in order.Sales)
                    {
                        datesTotalCost[group.Key] += sale.TotalCost;
                        datesTotalProfit[group.Key] += sale.TotalProfit;
                    }

                }
            }

            return groupedByOrderDateResult;
        }

        public void AddOrder(OrderDto orderDto)
        {
            this.Mapper.Map<ICollection<SaleDto>, List<Sale>>(orderDto.Sales);
            this.Mapper.Map<CountryDto, Country>(orderDto.Country);
            Order order = this.Mapper.Map<OrderDto, Order>(orderDto);

            Country checkCountry = this.DbContext.Countries.Where(c => c.CountryName == orderDto.Country.CountryName).FirstOrDefault();
            Region checkRegion = this.DbContext.Regions.Where(r => r.RegionName == orderDto.Country.Region.RegionName).FirstOrDefault();

            List<ItemType> newItemTypes = new List<ItemType>();
            List<ItemType> presentItemTypes = new List<ItemType>();

            foreach (Sale sale in order.Sales)
            {
                ItemType itemType = this.DbContext.ItemTypes.Where(it => it.ItemTypeName == sale.ItemType.ItemTypeName).FirstOrDefault();

                if (itemType is null)
                {
                    newItemTypes.Add(sale.ItemType);
                }
                else
                {
                    sale.ItemType = itemType;
                    presentItemTypes.Add(itemType);
                }
            }

            if (newItemTypes.Count > 0)
            {
                this.DbContext.ItemTypes.AddRange(newItemTypes);
            }

            if (presentItemTypes.Count > 0)
            {
                this.DbContext.ItemTypes.AttachRange(presentItemTypes);
            }

            if (checkCountry is null)
            {
                if (checkRegion is null)
                {
                    this.DbContext.Regions.Add(order.Country.Region);
                    this.DbContext.Countries.Add(order.Country);
                }
                else
                {
                    order.Country.Region = checkRegion;
                    this.DbContext.Regions.Attach(checkRegion);
                    this.DbContext.Countries.Add(order.Country);
                }
            }
            else
            {
                if (checkRegion is not null)
                {
                    order.Country = checkCountry;
                    order.Country.Region = checkRegion;
                    this.DbContext.Countries.Attach(checkCountry);
                    this.DbContext.Regions.Attach(checkRegion);
                }
            }

            this.DbContext.Orders.Add(order);
            this.DbContext.SaveChanges();
        }
        public void UpdateOrder(Order orderToBeUpdated, OrderDto orderDto)
        {
            this.Mapper.Map<ICollection<SaleDto>, List<Sale>>(orderDto.Sales);
            this.Mapper.Map<CountryDto, Country>(orderDto.Country);
            Order order = this.Mapper.Map<OrderDto, Order>(orderDto);

            Country checkCountry = this.DbContext.Countries.Where(c => c.CountryName == orderDto.Country.CountryName && c.IsDeleted == false).FirstOrDefault();
            Region checkRegion = this.DbContext.Regions.Where(r => r.RegionName == orderDto.Country.Region.RegionName && r.IsDeleted == false).FirstOrDefault();

            List<ItemType> newItemTypes = new List<ItemType>();
            List<ItemType> presentItemTypes = new List<ItemType>();

            foreach (Sale sale in order.Sales)
            {
                ItemType itemType = this.DbContext.ItemTypes.Where(it => it.ItemTypeName == sale.ItemType.ItemTypeName).FirstOrDefault();

                if (itemType is null)
                {
                    newItemTypes.Add(sale.ItemType);
                }
                else
                {
                    sale.ItemType = itemType;
                    presentItemTypes.Add(itemType);
                }
            }

            if (newItemTypes.Count > 0)
            {
                this.DbContext.ItemTypes.AddRange(newItemTypes);
            }

            if (presentItemTypes.Count > 0)
            {
                this.DbContext.ItemTypes.AttachRange(presentItemTypes);
            }

            if (checkCountry is null)
            {
                if (checkRegion is null)
                {
                    this.DbContext.Regions.Add(order.Country.Region);
                    this.DbContext.Countries.Add(order.Country);
                }
                else
                {
                    order.Country.Region = checkRegion;
                    this.DbContext.Regions.Attach(checkRegion);
                    this.DbContext.Countries.Add(order.Country);
                }
            }
            else
            {
                if (checkRegion is not null)
                {
                    order.Country = checkCountry;
                    order.Country.Region = checkRegion;
                    this.DbContext.Countries.Attach(checkCountry);
                    this.DbContext.Regions.Attach(checkRegion);
                }
                else
                {
                    return;
                }
            }

            orderToBeUpdated.OrderPriority = order.OrderPriority;
            orderToBeUpdated.OrderDate = order.OrderDate;
            orderToBeUpdated.SalesChannel = order.SalesChannel;
            orderToBeUpdated.Country = order.Country;
            orderToBeUpdated.Country.Region = order.Country.Region;
            orderToBeUpdated.Sales = order.Sales;

            this.DbContext.Orders.Update(orderToBeUpdated);
            this.DbContext.SaveChanges();
        }
        public void DeleteOrder(Order order)
        {
            order.IsDeleted = true;
            order.DeletedAt = DateTime.Now;

            foreach (Sale sale in order.Sales)
            {
                sale.IsDeleted = true;
                sale.DeletedAt = DateTime.Now;
            }

            this.DbContext.Orders.Update(order);
            this.DbContext.SaveChanges();
        }
    }
}
