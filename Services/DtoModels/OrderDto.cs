using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DtoModels
{
    public class OrderDto
    {
        public string OrderPriority { get; set; }

        public DateTime OrderDate { get; set; }

        public string SalesChannel { get; set; }

        public ICollection<SaleDto> Sales { get; set; }

        public CountryDto Country { get; set; }

    }
}
