using Services.DtoModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IRegionsMainService
    {
        public List<RegionDto> GetRegions();

        public void AddRegion(string regionName);

        public void UpdateRegion(string currentRegionName, string newRegionName);

        public void DeleteRegion(string regionName);
    }
}
