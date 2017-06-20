using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TodoList.Contracts.Models;

namespace TodoList.Contracts.Interfaces
{
    public interface IListItemRepository
    {
        Task<IEnumerable<ListItem>> GetAsync();

        Task<ListItem> GetAsync(Guid id);

        Task<ListItem> DeleteAsync(Guid id);

        Task<ListItem> PutAsync(ListItem item);

        Task<ListItem> PostAsync(ListItem item);
    }
}