using System;
using TodoList.Contracts.Models;
using TodoList.Contracts.Services;

namespace TodoList.Services
{
    public class ListItemGuidGenerator : IListItemGuidGenerator
    {
        public ListItem ListItemWithGuid(ListItem item)
        {
            ListItem guidedItem = item;
            guidedItem.Id = Guid.NewGuid();;
            return guidedItem;
        }
    }
}
