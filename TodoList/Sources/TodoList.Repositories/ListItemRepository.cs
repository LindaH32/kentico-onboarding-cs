using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using TodoList.Contracts.Models;
using TodoList.Contracts.Repositories;

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

        private readonly IMongoCollection<ListItem> _listItems;

        public ListItemRepository(IConnectionDetails connectionDetails)
        {
            string connectionString = connectionDetails.ConnectionString;
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase("todolist");
            _listItems = database.GetCollection<ListItem>("ListItems");
        }

        public async Task<IEnumerable<ListItem>> GetAllAsync()
            => await _listItems.Find(_ => true).ToListAsync();

        public async Task<ListItem> GetAsync(Guid id)
            => await _listItems.Find(item => item.Id.Equals(id)).FirstOrDefaultAsync();

        public async Task<ListItem> CreateAsync(ListItem item)
        {
            await _listItems.InsertOneAsync(item);
            return item;
        }

        public async Task<ListItem> UpdateAsync(ListItem item) 
            => await Task.FromResult(SampleItems[2]);

        public async Task<ListItem> DeleteAsync(Guid id)
            => await Task.FromResult(SampleItems[1]);
    }
}