using Services.DtoModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface ICountriesMainService
    {
        public List<CountryDto> GetCountries();

        public CountryDto GetCountryByName(string countryName);

        public void AddCountry(CountryDto countryDto);

        public void UpdateCountry(string countryName, CountryDto countryDto);

        public void DeleteCountry(string countryName);
    }
}
