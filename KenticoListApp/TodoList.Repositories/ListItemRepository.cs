using System;
using System.Collections.Generic;
using TodoList.Contracts.Interfaces;
using TodoList.Contracts.Models;

namespace TodoList.Repositories
{
    internal class ListItemRepository : IListItemRepository
    {
        private static readonly ListItem[] SampleItems =
        {
            new ListItem { Id = new Guid("98DBDE18-639E-49A6-8E51-603CEB2AE92D"), Text = "text" },
            new ListItem { Id = new Guid("1C353E0A-5481-4C31-BD2E-47E1BAF84DBE"), Text = "giraffe" },
            new ListItem { Id = new Guid("D69E065C-99B1-4A73-B00C-AD05F071861F"), Text = "updated" }
        };

        public IEnumerable<ListItem> Get() 
            => SampleItems;

        public ListItem Get(Guid id) 
            => SampleItems[0];

        public ListItem Delete(Guid id) 
            => SampleItems[1];

        public ListItem Put(ListItem item) 
            => SampleItems[2];

        public ListItem Post(ListItem item) 
            => SampleItems[0];
    }
}