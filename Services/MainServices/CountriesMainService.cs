using AutoMapper;
using Data;
using Microsoft.EntityFrameworkCore;
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
    public class CountriesMainService : ICountriesMainService
    {
        public CountriesMainService(TotalErrorDbContext dbContext, IMapper mapper)
        {
            this.DbContext = dbContext;
            this.Mapper = mapper;
        }

        public TotalErrorDbContext DbContext { get; }

        public IMapper Mapper { get; }

        public List<CountryDto> GetCountries()
        {
            this.Mapper.Map<List<RegionDto>>(this.DbContext.Regions.Where(r => r.IsDeleted == false).ToList());
            var countries = this.Mapper.Map<List<CountryDto>>(this.DbContext.Countries.Where(c => c.IsDeleted == false).ToList());

            return countries;
        }
        public CountryDto GetCountryByName(string countryName)
        {
            Country country = this.DbContext.Countries.Where(c => c.CountryName.ToUpper() == countryName.ToUpper()).Include(c => c.Region).FirstOrDefault();
            Region region = country.Region;

            CountryDto countryDto = this.Mapper.Map<Country, CountryDto>(country);

            return countryDto;
        }
        public void AddCountry(CountryDto countryDto)
        {
            Country country = this.Mapper.Map<CountryDto, Country>(countryDto);

            Region region = this.DbContext.Regions.FirstOrDefault(r => r.RegionName == countryDto.Region.RegionName);

            country.Region = region;

            if (region is not null)
            {
                this.DbContext.Regions.Attach(country.Region);
            }

            this.DbContext.Countries.Add(country);
            this.DbContext.SaveChanges();
        }
        public void UpdateCountry(string countryName, CountryDto countryDto)
        {
            Country country = this.DbContext.Countries.Where(c => c.CountryName == countryName && c.IsDeleted == false).FirstOrDefault();
            Region region = this.DbContext.Regions.Where(r => r.RegionName == countryDto.Region.RegionName && r.IsDeleted == false).FirstOrDefault();

            country.CountryName = countryDto.CountryName;
            country.Region = region;

            this.DbContext.Regions.Attach(country.Region);
            this.DbContext.Countries.Update(country);
            this.DbContext.SaveChanges();
        }
        public void DeleteCountry(string countryName)
        {
            Country country = this.DbContext.Countries.Where(c => c.CountryName == countryName && c.IsDeleted == false).FirstOrDefault();

            country.IsDeleted = true;
            country.DeletedAt = DateTime.Now;

            this.DbContext.Countries.Update(country);
            this.DbContext.SaveChanges();
        }
    }
}
