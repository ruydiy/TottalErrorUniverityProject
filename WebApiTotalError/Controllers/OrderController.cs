using Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.Interfaces;
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
    public class OrderController : ControllerBase
    {
        public OrderController(IOrdersMainService ordersMainService, TotalErrorDbContext dbContext)
        {
            this.OrdersMainService = ordersMainService;
            this.DbContext = dbContext;
        }

        public IOrdersMainService OrdersMainService { get; }

        public TotalErrorDbContext DbContext { get; }

        [HttpGet]
        [Route("all")]
        public IActionResult GetAllOrders()
        {
            var orders = OrdersMainService.GetOrders();

            var ordersJson = JsonConvert.SerializeObject(orders);

            return Ok(ordersJson);
        }

        [HttpGet]
        [Route("orders_grouped_by_region")]
        public IActionResult GetOrdersGroupedByRegion()
        {
            var orders = OrdersMainService.GetOrdersGroupedByRegion(out Dictionary<RegionDto, decimal> regionsTotalCost,
                out Dictionary<RegionDto, decimal> regionsTotalProfit);

            GroupedOrdersDtoModel<RegionDto> groupedModel = new GroupedOrdersDtoModel<RegionDto>(orders,
                regionsTotalCost, regionsTotalProfit);
            var jsonObject = JsonConvert.SerializeObject(groupedModel);

            return Ok(jsonObject);
        }

        [HttpGet]
        [Route("orders_grouped_by_country")]
        public IActionResult GetOrdersGroupedByCountry()
        {
            var orders = OrdersMainService.GetOrdersGroupedByCountry(out Dictionary<CountryDto, decimal> countriesTotalCost,
                out Dictionary<CountryDto, decimal> countriesTotalProfit);

            GroupedOrdersDtoModel<CountryDto> groupedModel = new GroupedOrdersDtoModel<CountryDto>(orders,
                countriesTotalCost, countriesTotalProfit);
            var jsonObject = JsonConvert.SerializeObject(groupedModel);

            return Ok(jsonObject);
        }

        [HttpGet]
        [Route("orders_grouped_by_order_date")]
        public IActionResult GetOrdersGroupedByOrderDate()
        {
            var orders = OrdersMainService.GetOrdersGroupedByOrderDate(out Dictionary<DateTime, decimal> datesTotalCost,
                out Dictionary<DateTime, decimal> datesTotalProfit);

            GroupedOrdersDtoModel<DateTime> groupedModel = new GroupedOrdersDtoModel<DateTime>(orders,
                datesTotalCost, datesTotalProfit);
            var jsonObject = JsonConvert.SerializeObject(groupedModel);

            return Ok(jsonObject);
        }

        [HttpPost]
        [Route("add")]
        public IActionResult AddOrder([FromBody] OrderDto order)
        {
            bool isValid = order.SalesChannel is not null && order.Sales is not null && order.OrderPriority is not null
                && order.Country is not null && order.Country.Region is not null;

            if (!isValid)
            {
                return BadRequest("Invalid order data!");
            }

            bool isAlreadyInTheDatabase = false;

            foreach (SaleDto sale in order.Sales)
            {
                isAlreadyInTheDatabase = this.DbContext.Sales.Any(s => s.ShipDate == sale.ShipDate && s.UnitsSold == sale.UnitsSold
                && s.UnitPrice == sale.UnitPrice && s.UnitCost == sale.UnitCost && s.TotalRevenue == sale.TotalRevenue
                && s.TotalCost == sale.TotalCost && s.TotalProfit == sale.TotalProfit && s.ItemType.ItemTypeName == sale.ItemType.ItemTypeName
                && s.IsDeleted == false);

                if (isAlreadyInTheDatabase)
                {
                    break;
                }
            }

            if (isAlreadyInTheDatabase)
            {
                return BadRequest("A sale is already in the database!");
            }

            this.OrdersMainService.AddOrder(order);

            return Ok(order);
        }

        [HttpPost]
        [Route("update/{orderId}")]
        public IActionResult UpdateOrder(string orderId, [FromBody] OrderDto order)
        {
            Order initialOrder = this.DbContext.Orders.Where(o => o.Id == orderId && o.IsDeleted == false)
                                                        .Include(o => o.Sales.Where(s => s.IsDeleted == false)).FirstOrDefault();
            if (initialOrder is null)
            {
                return BadRequest("Order with such id doesn't exist!");
            }

            this.OrdersMainService.UpdateOrder(initialOrder, order);

            return Ok(order);
        }

        [HttpPost]
        [Route("delete/{orderId}")]
        public IActionResult DeleteOrder(string orderId)
        {
            Order order = this.DbContext.Orders.Where(o => o.Id == orderId && o.IsDeleted == false)
                                                        .Include(o => o.Sales.Where(s => s.IsDeleted == false)).FirstOrDefault();

            if (order is null)
            {
                return BadRequest("Order with such id doesn't exist!");
            }

            this.OrdersMainService.DeleteOrder(order);

            return Ok("Order is successfully deleted!");
        }
    }
}
