using System;
using System.Threading.Tasks;

namespace TodoList.Contracts.Services
{
    public interface IGuidGenerator
    {
        Task<Guid> GenerateGuid();
    }
}