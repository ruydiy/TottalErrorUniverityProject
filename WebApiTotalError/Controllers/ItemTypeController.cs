using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models;
using Newtonsoft.Json;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiTotalError.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ItemTypeController : ControllerBase
    {
        public ItemTypeController(IItemTypesMainService itemTypesMainService)
        {
            this.ItemTypesMainService = itemTypesMainService;

        }

        public IItemTypesMainService ItemTypesMainService { get; set; }

        [HttpGet]
        [Route("all")]
        public IActionResult GetAllItemTypes()
        {
            var itemTypes = ItemTypesMainService.GetItemTypes();

            var itemTypesJson = JsonConvert.SerializeObject(itemTypes);

            return Ok(itemTypesJson);
        }

        [HttpPost]
        [Route("add/{itemType}")]
        public IActionResult AddItemType([FromRoute] string itemType)
        {
            if (itemType is null)
            {
                return BadRequest("Invalid item type name!");
            }

            this.ItemTypesMainService.AddItemType(itemType);

            return Ok(itemType);
        }

        [HttpPost]
        [Route("update/{oldItemType}-{newItemType}")]
        public IActionResult UpdateItemType([FromRoute] string oldItemType, [FromRoute] string newItemType)
        {
            if (oldItemType is null || newItemType is null)
            {
                return BadRequest("Invalid item type name!");
            }

            this.ItemTypesMainService.UpdateItemType(oldItemType, newItemType);

            return Ok(newItemType);
        }

        [HttpPost]
        [Route("delete/{itemType}")]
        public IActionResult DeleteItemType([FromRoute] string itemType)
        {
            if (itemType is null)
            {
                return BadRequest("Invalid item type name!");
            }

            this.ItemTypesMainService.DeleteItemType(itemType);

            return Ok(string.Format("{0} is successfully deleted!", itemType));
        }

    }
}
