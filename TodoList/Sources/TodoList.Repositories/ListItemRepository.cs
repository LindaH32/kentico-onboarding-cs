using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using TodoList.Contracts.Models;
using TodoList.Contracts.Repositories;

namespace TodoList.Repositories
{
    internal class ListItemRepository : IListItemRepository
    {
        private const string CollectionName = "ListItems";

        private readonly IMongoCollection<ListItem> _listItems;

        public ListItemRepository(IConnectionDetails connectionDetails)
        {
            var mongoUrl = new MongoUrl(connectionDetails.ConnectionString);
            var client = new MongoClient(mongoUrl);
            var database = client.GetDatabase(mongoUrl.DatabaseName);
            _listItems = database.GetCollection<ListItem>(CollectionName);
        }

        public async Task<IEnumerable<ListItem>> GetAllAsync()
            => await _listItems.Find(_ => true).ToListAsync();

        public async Task<ListItem> GetAsync(Guid id)
            => await _listItems.Find(item => item.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(ListItem item) 
            => await _listItems.InsertOneAsync(item);

        public async Task<ListItem> UpdateAsync(ListItem item) 
            => await _listItems.FindOneAndReplaceAsync(databaseItem => databaseItem.Id == item.Id, item);

        public async Task<ListItem> DeleteAsync(Guid id) 
            => await _listItems.FindOneAndDeleteAsync(databaseItem => databaseItem.Id == id);
    }
}