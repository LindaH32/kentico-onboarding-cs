using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TodoList.Contracts.Models;

namespace TodoList.Contracts.Repositories
{
    public interface IListItemRepository
    {
        Task<IEnumerable<ListItem>> GetAllAsync();

        Task<ListItem> GetAsync(Guid id);

        Task CreateAsync(ListItem item);

        Task<ListItem> UpdateAsync(ListItem item);

        Task<ListItem> DeleteAsync(Guid id);
    }
}