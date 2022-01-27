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
    public class ItemTypesMainService : IItemTypesMainService
    {
        public ItemTypesMainService(TotalErrorDbContext dbContext, IMapper mapper)
        {
            this.DbContext = dbContext;
            this.Mapper = mapper;
        }

        public TotalErrorDbContext DbContext { get; }

        public IMapper Mapper { get; }

        public List<ItemTypeDto> GetItemTypes()
        {
            var itemTypes = this.Mapper.Map<List<ItemTypeDto>>(this.DbContext.ItemTypes.Where(it => it.IsDeleted == false).ToList());

            return itemTypes;
        }

        public void AddItemType(string itemTypeName)
        {
            ItemType itemType = new ItemType();
            itemType.ItemTypeName = itemTypeName;

            this.DbContext.ItemTypes.Add(itemType);
            this.DbContext.SaveChanges();
        }

        public void UpdateItemType(string currentItemType, string newItemType)
        {
            ItemType itemType = this.DbContext.ItemTypes.Where(it => it.ItemTypeName == currentItemType && it.IsDeleted == false).FirstOrDefault();

            itemType.ItemTypeName = newItemType;

            this.DbContext.ItemTypes.Update(itemType);
            this.DbContext.SaveChanges();
        }

        public void DeleteItemType(string itemTypeName)
        {
            ItemType itemType = this.DbContext.ItemTypes.Where(it => it.ItemTypeName == itemTypeName && it.IsDeleted == false).FirstOrDefault();

            itemType.IsDeleted = true;
            itemType.DeletedAt = DateTime.Now;

            this.DbContext.ItemTypes.Update(itemType);
            this.DbContext.SaveChanges();
        }
    }
}
