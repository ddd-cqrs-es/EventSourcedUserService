using System;
using UserService.DomainModel.Events;

namespace UserService.DomainModel
{
    public class WishListItem : Entity
    {
        public WishListItemId Id { get; private set; }
        public RestaurantId RestoId { get; private set; }

        public WishListItem(Action<object> applier) 
            : base(applier)
        {
            Register<UserEvents.UserHasNewWishListItem>(When);
        }

        protected void When(UserEvents.UserHasNewWishListItem newWishListItem)
        {
            Id = new WishListItemId(newWishListItem.WishListItemId);
            RestoId = new RestaurantId(newWishListItem.RestaurantId);
        }
    }

    public struct WishListItemId : IEquatable<WishListItemId>
    {
        readonly Guid _value;

        public WishListItemId(Guid value)
        {
            _value = value;
        }

        public bool Equals(WishListItemId other)
        {
            return _value.Equals(other._value);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is WishListItemId && Equals((WishListItemId)obj);
        }

        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }

        public static implicit operator Guid(WishListItemId id)
        {
            return id._value;
        }
    }
}
