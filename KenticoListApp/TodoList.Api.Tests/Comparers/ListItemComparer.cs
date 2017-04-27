using System.Collections.Generic;
using TodoList.Api.Models;

namespace TodoList.Api.Tests.Comparers
{
    public class ListItemComparer : IEqualityComparer<ListItem>
    {
        public bool Equals(ListItem x, ListItem y)
        {
            return x.Id.Equals(y.Id) && x.Text.Equals(y.Text);
        }

        public int GetHashCode(ListItem obj)
        {
            return (obj.Id.GetHashCode() * 397) ^ (obj.Text?.GetHashCode() ?? 0);
        }
    }
}