using System;
using System.Collections.Generic;
using System.Linq;
using AggregateSource;
using UserService.DomainModel.Events;

namespace UserService.DomainModel
{
    public class User : AggregateRootEntity
    {
        //state
        private UserId _id;
        private bool _disabled;
        protected List<UserId> _friends;
        protected List<WishListItem> _wishList;

        public UserId Id { get { return _id; } }

        //construction
        public static readonly Func<User> Factory = () => new User();

        public User()
        {
            //register the event handlers
            Register<UserEvents.BasicUserCreated>(When);
            Register<UserEvents.UserDisabled>(When);
            Register<UserEvents.UserHasNewFriend>(When);Register<UserEvents.UserHasNewWishListItem>(When);
        }

        public static User CreateBasicUser(UserId gpid, string emailAddress, int metroId)
        {
            // do any business logic to protect invariants
            // note: validation, like email address is well formed, is not something that should be done in here - 
            // that should be taken care of before this method is invoked
            var user = Factory();
            user.ApplyChange(UserEvents.Created(emailAddress, gpid, metroId, false));
            return user;
        }

        public void Disable()
        {
            if(_disabled)
                throw new AggregateException(string.Format("User {0} is already disabled", _id));

            ApplyChange(UserEvents.Disabled(_id));
        }

        public void AddFriend(UserId userId, UserId friendsId, string firstname, string lastName)
        {
            if(_disabled)
                throw new AggregateException("Can not add a friend to a disabled user.");

            if(IDontHaveAnyFriends() || ThatIsANewFriend(userId))
                ApplyChange(UserEvents.NewFriend(_id, friendsId, firstname, lastName));
        }

        private bool IDontHaveAnyFriends()
        {
            return _friends == null;
        }

        private bool ThatIsANewFriend(UserId friendId)
        {
            return !_friends.Contains(friendId);
        }

        public void AddWishListItem(UserId id, WishListItemId wishlistItemId, RestaurantId restoId, string someNotes)
        {
            if (_disabled)
                throw new AggregateException("Can not add a to the wish list of a disabled user.");

            if (MyWishListIsEmpty() || ThatResturantIsNotInMyWishList(restoId))
                ApplyChange(UserEvents.WishListItemAdded(id, wishlistItemId, restoId, someNotes));
        }

        private bool MyWishListIsEmpty()
        {
            return _wishList == null;
        }

        private bool ThatResturantIsNotInMyWishList(RestaurantId restoId)
        {
            return !_wishList.Any(w => w.RestoId.Equals(restoId));
        }

        //event handlers
        protected void When(UserEvents.BasicUserCreated @event)
        {
            // apply the state contained in the event
            _id = new UserId(@event.Gpid);
            _disabled = @event.Disabled;
        }

        protected void When(UserEvents.UserDisabled @event)
        {
            _disabled = true;
        }

        protected void When(UserEvents.UserHasNewFriend @event)
        {
            if(IDontHaveAnyFriends())
                _friends = new List<UserId>();

            _friends.Add(new UserId(@event.FriendsGpid));
        }

        protected void When(UserEvents.UserHasNewWishListItem @event)
        {
            if(MyWishListIsEmpty())
                _wishList = new List<WishListItem>();

            var item = new WishListItem(ApplyChange);
            item.Route(@event);
            _wishList.Add(item);
        }
    }

    public class UserId : IEquatable<UserId>
    {
        private readonly Guid _gpid;

        public Guid Gpid
        {
            get { return _gpid; }
        }

        public UserId(Guid gpid)
        {
            _gpid = gpid;
        }

        public bool Equals(UserId other)
        {
            return _gpid.Equals(other._gpid);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is UserId && Equals((UserId)obj);
        }

        public override int GetHashCode()
        {
            return _gpid.GetHashCode();
        }

        public static implicit operator Guid(UserId id)
        {
            return id._gpid;
        }

        public override string ToString()
        {
            return _gpid.ToString();
        }
    }
}
