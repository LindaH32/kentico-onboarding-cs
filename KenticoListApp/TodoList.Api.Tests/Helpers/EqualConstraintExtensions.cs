using System;
using System.Collections.Generic;
using NUnit.Framework.Constraints;
using TodoList.Contracts.Models;

namespace TodoList.Api.Tests.Helpers
{
    public static class EqualConstraintExtensions
    {
        private static readonly Lazy<IEqualityComparer<ListItem>> Comparer = new Lazy<IEqualityComparer<ListItem>>(()=> new ListItemComparer()) ;

        public static EqualConstraint UsingListItemComparer(this EqualConstraint constraint) 
            => constraint.Using(Comparer.Value);
    }
}