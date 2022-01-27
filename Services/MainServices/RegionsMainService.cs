using AutoMapper;
using Data;
using Models;
using Services.DtoModels;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.MainServices
{
    public class RegionsMainService : IRegionsMainService
    {
        public RegionsMainService(TotalErrorDbContext dbContext, IMapper mapper)
        {
            this.DbContext = dbContext;
            this.Mapper = mapper;
        }

        public TotalErrorDbContext DbContext { get; }

        public IMapper Mapper { get; }

        public List<RegionDto> GetRegions()
        {
            var regions = this.Mapper.Map<List<RegionDto>>(this.DbContext.Regions.Where(r => r.IsDeleted == false).ToList());

            return regions;
        }

        public void AddRegion(string regionName)
        {
            Region region = new Region();
            region.RegionName = regionName;

            this.DbContext.Regions.Add(region);
            this.DbContext.SaveChanges();
        }

        public void UpdateRegion(string oldRegionName, string newRegionName)
        {
            Region region = this.DbContext.Regions.Where(r => r.RegionName == oldRegionName && r.IsDeleted == false).FirstOrDefault();

            region.RegionName = newRegionName;

            this.DbContext.Regions.Update(region);
            this.DbContext.SaveChanges();
        }

        public void DeleteRegion(string regionName)
        {
            Region region = this.DbContext.Regions.Where(r => r.RegionName == regionName && r.IsDeleted == false).FirstOrDefault();

            region.IsDeleted = true;
            region.DeletedAt = DateTime.Now;

            this.DbContext.Regions.Update(region);
            this.DbContext.SaveChanges();
        }
    }
}
