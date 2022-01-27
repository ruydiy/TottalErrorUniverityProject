using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Services.DtoModels;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace WebApiTotalError.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CountryController : ControllerBase
    {
        public CountryController(ICountriesMainService ordersMainService)
        {
            this.CountriesMainService = ordersMainService;
        }

        public ICountriesMainService CountriesMainService { get; }

        [HttpGet]
        [Route("all")]
        public IActionResult GetAllCountries()
        {
            List<CountryDto> countries = this.CountriesMainService.GetCountries();

            var countriesJson = JsonConvert.SerializeObject(countries);

            return Ok(countriesJson);
        }

        [HttpGet]
        [Route("get_country_by_name/{countryName}")]
        public IActionResult GetCountryByName([FromRoute] string countryName)
        {
            CountryDto country = this.CountriesMainService.GetCountryByName(countryName);

            if (country is null)
            {
                return BadRequest("Invalid country data!");
            }

            var countryJson = JsonConvert.SerializeObject(country);

            return Ok(countryJson);
        }

        [HttpPost]
        [Route("add")]
        public IActionResult AddCountry([FromBody] CountryDto country)
        {
            if (country.CountryName is null || country.Region is null)
            {
                return BadRequest("Invalid country name!");
            }

            this.CountriesMainService.AddCountry(country);

            return Ok(country);
        }

        [HttpPost]
        [Route("update/{countryName}")]
        public IActionResult UpdateCountry([FromRoute] string countryName, [FromBody] CountryDto updatedCountry)
        {
            if (updatedCountry.CountryName is null || updatedCountry.Region is null)
            {
                return BadRequest("Invalid country name!");
            }

            this.CountriesMainService.UpdateCountry(countryName, updatedCountry);

            return Ok(updatedCountry);
        }

        [HttpPost]
        [Route("delete/{countryName}")]
        public IActionResult DeleteCountry([FromRoute] string countryName)
        {
            if (countryName is null)
            {
                return BadRequest("Invalid country name!");
            }

            this.CountriesMainService.DeleteCountry(countryName);

            return Ok(string.Format("{0} is deleted successfully!", countryName));
        }


    }
}
