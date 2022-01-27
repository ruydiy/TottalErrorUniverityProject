using Services.DtoModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IItemTypesMainService
    {
        public List<ItemTypeDto> GetItemTypes();

        public void AddItemType(string itemTypeName);

        public void UpdateItemType(string currentItemType, string newItemType);

        public void DeleteItemType(string itemTypeName);
    }
}
