using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DtoModels
{
    public class CountryDto
    {
        public string CountryName { get; set; }

        public RegionDto Region { get; set; }
    }
}
