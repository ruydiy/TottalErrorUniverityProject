using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Interfaces
{
    public class Order : BaseModel, IFileDate
    {
        public string OrderPriority { get; set; }

        public DateTime OrderDate { get; set; }

        public string SalesChannel { get; set; }

        public ICollection<Sale> Sales { get; set; }

        public Country Country { get; set; }

        //could be null.
        public string? FileDate { get; set; }
    }
}
