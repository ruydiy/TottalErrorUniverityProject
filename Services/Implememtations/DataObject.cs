using Models;
using Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implememtations
{
    public class DataObject
    {
        public HashSet<Country> Countries { get; set; }

        public HashSet<ItemType> ItemTypes { get; set; }

        public List<DateTime> LastReadFiles { get; set; }

        public HashSet<Order> Orders { get; set; }

        public HashSet<Region> Regions { get; set; }

        public HashSet<Sale> Sales { get; set; }
    }
}
