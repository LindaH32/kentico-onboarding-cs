using System.Threading.Tasks;
using TodoList.Contracts.Models;

namespace TodoList.Contracts.Services
{
    public interface ICreateItemService
    {
        Task<ListItem> CreateNewItemAsync(ListItem item);
    }
}