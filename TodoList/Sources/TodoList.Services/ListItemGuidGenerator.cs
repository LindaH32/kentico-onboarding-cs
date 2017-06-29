using System;
using TodoList.Contracts.Services;

namespace TodoList.Services
{
    public class ListItemGuidGenerator : IListItemGuidGenerator
    {
        public Guid GenerateGuid()
            => Guid.NewGuid();
    }
}
