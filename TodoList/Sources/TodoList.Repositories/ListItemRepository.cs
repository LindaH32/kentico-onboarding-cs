using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        
        public async Task<IEnumerable<ListItem>> GetAllAsync()
            => await Task.FromResult(SampleItems);

        public async Task<ListItem> GetAsync(Guid id) 
            => await Task.FromResult(SampleItems[0]);

        public async  Task<ListItem> DeleteAsync(Guid id) 
            => await Task.FromResult(SampleItems[1]);

        public async Task<ListItem> UpdateAsync(ListItem item) 
            => await Task.FromResult(SampleItems[2]);

        public async Task<ListItem> CreateAsync(ListItem item) 
            => await Task.FromResult(SampleItems[0]);
    }
}