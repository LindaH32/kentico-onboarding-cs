using System;
using TodoList.Contracts.Models;

namespace TodoList.Contracts.Services
{
    public interface IListItemGuidGenerator
    {
        Guid GenerateGuid(ListItem item);
    }
}
