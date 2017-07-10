using System.Threading.Tasks;
using TodoList.Contracts.Models;

namespace TodoList.Contracts.Services
{
    public interface IItemModificationService
    {
        Task<ListItem> UpdateExistingItemAsync(AcquisitionResult acquisitionResult, ListItem modifiedItem);
        
    }
}