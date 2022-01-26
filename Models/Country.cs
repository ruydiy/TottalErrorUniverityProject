using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Country : BaseModel
    {
        public string CountryName { get; set; }

        public Region Region { get; set; }
    }
}
