using AutoMapper;
using Models;
using Models.Interfaces;
using Services.DtoModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.MapperProfiles
{
    public class MapProfile : Profile
    {
        public MapProfile()
        {
            //map models and dtoModels
            CreateMap<Country, CountryDto>().ReverseMap();
            CreateMap<ItemType, ItemTypeDto>().ReverseMap();
            CreateMap<Order, OrderDto>().ReverseMap();
            CreateMap<Region, RegionDto>().ReverseMap();
            CreateMap<Sale, SaleDto>().ReverseMap();
        }
    }
}
