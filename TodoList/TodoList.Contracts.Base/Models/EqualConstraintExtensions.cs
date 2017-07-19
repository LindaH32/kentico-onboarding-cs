using System;
using System.Collections.Generic;
using NUnit.Framework.Constraints;
using TodoList.Contracts.Models;

namespace TodoList.Contracts.Base.Models
{
    public static class EqualConstraintExtensions
    {
        private static readonly Lazy<IEqualityComparer<ListItem>> ItemComparer = new Lazy<IEqualityComparer<ListItem>>(()=> new ListItemComparer()) ;

        private static readonly Lazy<IEqualityComparer<AcquisitionResult>> ResultComparer = new Lazy<IEqualityComparer<AcquisitionResult>>(() => new AcquisitionResultComparer());
        
        public static EqualConstraint UsingListItemComparer(this EqualConstraint constraint) 
            => constraint.Using(ItemComparer.Value);

        public static EqualConstraint UsingAcquisitionResultComparer(this EqualConstraint constraint)
            => constraint.Using(ResultComparer.Value);

        private sealed class ListItemComparer : IEqualityComparer<ListItem>
        {
            public bool Equals(ListItem x, ListItem y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (ReferenceEquals(x, null)) return false;
                if (ReferenceEquals(y, null)) return false;
                if (x.GetType() != y.GetType()) return false;
                return x.Id.Equals(y.Id) && String.Equals(x.Text, y.Text) && x.CreationDateTime.Equals(y.CreationDateTime) && x.UpdateDateTime.Equals(y.UpdateDateTime);
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

        private sealed class AcquisitionResultComparer : IEqualityComparer<AcquisitionResult>
        {
            public bool Equals(AcquisitionResult x, AcquisitionResult y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (ReferenceEquals(x, null)) return false;
                if (ReferenceEquals(y, null)) return false;
                if (x.GetType() != y.GetType()) return false;
                return ItemComparer.Value.Equals(x.AcquiredItem, y.AcquiredItem) && Equals(x.WasSuccessful, y.WasSuccessful);
            }

            public int GetHashCode(AcquisitionResult obj)
            {
                return (obj.AcquiredItem != null ? obj.AcquiredItem.GetHashCode() : 0);
            }
        }
    }
}