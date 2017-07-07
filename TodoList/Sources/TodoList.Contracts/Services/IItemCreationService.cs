using System.Threading.Tasks;
using TodoList.Contracts.Models;

namespace TodoList.Contracts.Services
{
    public interface IItemCreationService
    {
        Task<ListItem> CreateNewItemAsync(ListItem item);
    }
}