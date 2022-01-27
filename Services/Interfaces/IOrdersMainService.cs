using Models.Interfaces;
using Services.DtoModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IOrdersMainService
    {
        public List<OrderDto> GetOrders();

        public List<IGrouping<RegionDto, OrderDto>> GetOrdersGroupedByRegion(out Dictionary<RegionDto, decimal> countriesTotalCost,
            out Dictionary<RegionDto, decimal> countriesTotalProfit);

        public List<IGrouping<CountryDto, OrderDto>> GetOrdersGroupedByCountry(out Dictionary<CountryDto, decimal> countriesTotalCost,
            out Dictionary<CountryDto, decimal> countriesTotalProfit);

        public List<IGrouping<DateTime, OrderDto>> GetOrdersGroupedByOrderDate(out Dictionary<DateTime, decimal> countriesTotalCost,
            out Dictionary<DateTime, decimal> countriesTotalProfit);

        public void AddOrder(OrderDto orderDto);

        public void UpdateOrder(Order orderToBeUpdated, OrderDto orderDto);

        public void DeleteOrder(Order order);
    }
}
