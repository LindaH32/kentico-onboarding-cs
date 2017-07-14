using System;
using System.Threading.Tasks;
using TodoList.Contracts.Models;

namespace TodoList.Contracts.Services
{
    public interface IItemAcquisitionService
    {
        Task<AcquisitionResult> GetItemAsync(Guid id);
    }
}