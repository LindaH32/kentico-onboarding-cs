using System;
using System.Collections.Generic;
using NUnit.Framework.Constraints;
using TodoList.Contracts.Models;

namespace TodoList.Api.Tests.Helpers
{
    internal static class EqualConstraintExtensions
    {
        private static readonly Lazy<IEqualityComparer<ListItem>> Comparer = new Lazy<IEqualityComparer<ListItem>>(()=> new ListItemComparer()) ;

        public static EqualConstraint UsingListItemComparer(this EqualConstraint constraint) 
            => constraint.Using(Comparer.Value);

        private class ListItemComparer : IEqualityComparer<ListItem>
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
}