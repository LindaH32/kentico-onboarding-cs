using System;
using System.Collections.Generic;
using Contracts.Models;

namespace Contracts.Interfaces
{
    public interface IListItemRepository
    {
        IEnumerable<ListItem> Get();

        ListItem Get(Guid id);

        ListItem Delete(Guid id);

        ListItem Put();

        ListItem Post();
    }
}
