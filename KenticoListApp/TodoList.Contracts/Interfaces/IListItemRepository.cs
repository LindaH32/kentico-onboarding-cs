using System;
using System.Collections.Generic;
using TodoList.Contracts.Models;

namespace TodoList.Contracts.Interfaces
{
    public interface IListItemRepository
    {
        IEnumerable<ListItem> Get();

        ListItem Get(Guid id);

        ListItem Delete(Guid id);

        ListItem Put(ListItem item);

        ListItem Post(ListItem item);
    }
}