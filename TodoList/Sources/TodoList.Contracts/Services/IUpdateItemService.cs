using System.Threading.Tasks;
using TodoList.Contracts.Models;

namespace TodoList.Contracts.Services
{
    public interface IUpdateItemService
    {
        Task<ListItem> UpdateExistingItemAsync(ListItem item);
    }
}