using System;

namespace TodoList.Contracts.Services
{
    public interface IGuidGenerator
    {
        Guid GenerateGuid();
    }
}