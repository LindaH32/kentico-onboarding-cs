using System.Collections.Generic;
using TodoList.Api.Models;

namespace TodoList.Api.Tests.Comparers
{
    public class ListItemComparer : IEqualityComparer<ListItem>
    {
        public bool Equals(ListItem x, ListItem y)
        {
            return (x.Id.CompareTo(y.Id) == 0) && (x.Text.CompareTo(y.Text) == 0);
        }

        public int GetHashCode(ListItem obj)
        {
            return ((obj.Id != null ? obj.Id.GetHashCode() : 0) * 397) ^ (obj.Text != null ? obj.Text.GetHashCode() : 0);
        }


    }
}