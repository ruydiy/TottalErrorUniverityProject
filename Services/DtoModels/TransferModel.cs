using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DtoModels
{
    public class TransferModel
    {
        [Name("Region")]
        public string Region { get; set; }

        [Name("Country")]
        public string Country { get; set; }

        [Name("Item Type")]
        public string ItemType { get; set; }

        [Name("Sales Channel")]
        public string SalesChannel { get; set; }

        [Name("Order Priority")]
        public string OrderPriority { get; set; }

        [Name("Order Date")]
        public string OrderDate { get; set; }

        [Name("Order ID")]
        public string OrderId { get; set; }

        [Name("Ship Date")]
        public string ShipDate { get; set; }

        [Name("Units Sold")]
        public string UnitsSold { get; set; }

        [Name("Unit Price")]
        public string UnitPrice { get; set; }

        [Name("Unit Cost")]
        public string UnitCost { get; set; }

        [Name("Total Revenue")]
        public string TotalRevenue { get; set; }

        [Name("Total Cost")]
        public string TotalCost { get; set; }

        [Name("Total Profit")]
        public string TotalProfit { get; set; }
    }
}
