using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contracts.Interfaces;
using Contracts.Models;

namespace ListItemRepository
{
    public class ListItemRepository : IListItemRepository
    {
        public IEnumerable<ListItem> Get()
        {
            throw new NotImplementedException();
        }

        public ListItem Get(Guid id)
        {
            throw new NotImplementedException();
        }

        public ListItem Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public ListItem Put()
        {
            throw new NotImplementedException();
        }

        public ListItem Post()
        {
            throw new NotImplementedException();
        }
    }
}
