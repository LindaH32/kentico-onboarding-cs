using System;
using TodoList.Contracts.Services;

namespace TodoList.Services.Wrappers
{
    internal class GuidGenerator : IGuidGenerator
    {
        public Guid GenerateGuid()
            => Guid.NewGuid();
    }
}
