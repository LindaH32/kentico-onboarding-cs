using System;
using System.Collections.Generic;
using NUnit.Framework.Constraints;
using TodoList.Contracts.Models;

namespace TodoList.Contracts.Base.Models
{
    internal static class EqualConstraintExtensions
    {
        private static readonly Lazy<IEqualityComparer<ListItem>> Comparer = new Lazy<IEqualityComparer<ListItem>>(()=> new ListItemComparer()) ;

        public static EqualConstraint UsingListItemComparer(this EqualConstraint constraint) 
            => constraint.Using(Comparer.Value);

        private sealed class ListItemComparer : IEqualityComparer<ListItem>
        {
            public bool Equals(ListItem x, ListItem y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (ReferenceEquals(x, null)) return false;
                if (ReferenceEquals(y, null)) return false;
                if (x.GetType() != y.GetType()) return false;
                return x.Id.Equals(y.Id) && string.Equals(x.Text, y.Text) && x.CreationDateTime.Equals(y.CreationDateTime) && x.UpdateDateTime.Equals(y.UpdateDateTime);
            }

            public int GetHashCode(ListItem obj)
            {
                unchecked
                {
                    var hashCode = obj.Id.GetHashCode();
                    hashCode = (hashCode * 397) ^ (obj.Text?.GetHashCode() ?? 0);
                    hashCode = (hashCode * 397) ^ obj.CreationDateTime.GetHashCode();
                    hashCode = (hashCode * 397) ^ obj.UpdateDateTime.GetHashCode();
                    return hashCode;
                }
            }
        }
    }
}