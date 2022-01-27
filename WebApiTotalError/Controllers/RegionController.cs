using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
    public class RegionController : ControllerBase
    {
        public RegionController(IRegionsMainService regionsMainService)
        {
            this.RegionsMainService = regionsMainService;
        }

        public IRegionsMainService RegionsMainService { get; }

        [HttpGet]
        [Route("all")]
        public IActionResult GetAllRegions()
        {
            var regions = this.RegionsMainService.GetRegions();

            var regionsJson = JsonConvert.SerializeObject(regions);

            return Ok(regionsJson);
        }

        [HttpPost]
        [Route("add/{regionName}")]
        public IActionResult AddRegion([FromRoute] string regionName)
        {
            if (regionName is null)
            {
                return BadRequest("Invalid region name!");
            }

            this.RegionsMainService.AddRegion(regionName);

            return Ok(regionName);
        }

        [HttpPost]
        [Route("update/{oldRegion}-{newRegion}")]
        public IActionResult UpdateItemType([FromRoute] string oldRegion, [FromRoute] string newRegion)
        {
            if (oldRegion is null || newRegion is null)
            {
                return BadRequest("Invalid region name!");
            }

            this.RegionsMainService.UpdateRegion(oldRegion, newRegion);

            return Ok(newRegion);
        }

        [HttpPost]
        [Route("delete/{region}")]
        public IActionResult DeleteItemType([FromRoute] string region)
        {
            if (region is null)
            {
                return BadRequest("Invalid region name!");
            }

            this.RegionsMainService.DeleteRegion(region);

            return Ok(string.Format("{0} is successfully deleted!", region));
        }
    }
}
