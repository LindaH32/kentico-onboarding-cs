using System.Threading.Tasks;
using TodoList.Contracts.Models;

namespace TodoList.Contracts.Services
{
    public interface IListItemServices
    {
        Task<ListItem> PostAsync(ListItem item);
    }
}
