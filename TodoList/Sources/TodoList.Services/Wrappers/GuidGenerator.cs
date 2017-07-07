using System;
using TodoList.Contracts.Services;

namespace TodoList.Services
{
    internal class GuidGenerator : IGuidGenerator
    {
        public Guid GenerateGuid()
            => Guid.NewGuid();
    }
}
