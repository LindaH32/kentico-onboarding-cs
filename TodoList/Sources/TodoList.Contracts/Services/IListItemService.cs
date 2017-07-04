using System.Threading.Tasks;
using TodoList.Contracts.Models;

namespace TodoList.Contracts.Services
{
    public interface IListItemService
    {
        Task<ListItem> CreateNewItemAsync(ListItem item);

        Task<ListItem> UpdateExistingItemAsync(ListItem item);
    }
}
