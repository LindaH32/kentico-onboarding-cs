using System;
using System.Threading.Tasks;
using TodoList.Contracts.Services;

namespace TodoList.Services.Wrappers
{
    internal class GuidGenerator : IGuidGenerator
    {
        public async Task<Guid> GenerateGuid()
            => await Task.FromResult(Guid.NewGuid());
    }
}
