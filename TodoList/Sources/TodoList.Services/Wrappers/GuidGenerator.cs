using System;
using TodoList.Contracts.Services;

namespace TodoList.Services
{
    public class GuidGenerator : IGuidGenerator
    {
        public Guid GenerateGuid()
            => Guid.NewGuid();
    }
}
