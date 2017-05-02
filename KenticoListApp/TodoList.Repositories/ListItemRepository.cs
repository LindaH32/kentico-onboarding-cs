using System;
using System.Collections.Generic;
using TodoList.Contracts.Interfaces;
using TodoList.Contracts.Models;

namespace TodoList.Repositories
{
    public class ListItemRepository : IListItemRepository
    {
        private static readonly List<ListItem> SampleItems = new List<ListItem>
        {
            new ListItem(new Guid("98DBDE18-639E-49A6-8E51-603CEB2AE92D"), "text"),
            new ListItem(new Guid("1C353E0A-5481-4C31-BD2E-47E1BAF84DBE"), "giraffe"),
            new ListItem(new Guid("D69E065C-99B1-4A73-B00C-AD05F071861F"), "updated")
        };

        public IEnumerable<ListItem> Get()
        {
            return SampleItems;
        }

        public ListItem Get(Guid id)
        {
            return SampleItems[0];
        }

        public ListItem Delete(Guid id)
        {
            return SampleItems[1];
        }

        public ListItem Put(ListItem item)
        {
            return SampleItems[2];
        }

        public ListItem Post(ListItem item)
        {
            return SampleItems[0];
        }
    }
}