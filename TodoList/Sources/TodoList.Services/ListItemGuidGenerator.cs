using System;
using TodoList.Contracts.Models;
using TodoList.Contracts.Services;

namespace TodoList.Services
{
    public class ListItemGuidGenerator : IListItemGuidGenerator
    {
        public Guid GenerateGuid(ListItem item)
            => Guid.NewGuid();
    }
}
