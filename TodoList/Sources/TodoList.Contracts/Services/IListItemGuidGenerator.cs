using System;

namespace TodoList.Contracts.Services
{
    public interface IListItemGuidGenerator
    {
        Guid GenerateGuid();
    }
}