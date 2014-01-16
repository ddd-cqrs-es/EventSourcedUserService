using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AggregateSource;
using UserService.DomainModel.Commands;
using UserService.DomainModel.Events;

namespace UserService.DomainModel
{
    public class User : AggregateRootEntity
    {
        //state
        private UserId _id;
        private bool _disabled;
        protected List<UserId> _friends; 

        //construction
        public static readonly Func<User> Factory = () => new User();

        public User()
        {
            //register the event handlers
            Register<UserEvents.BasicUserCreated>(When);
            Register<UserEvents.UserDisabled>(When);
            Register<UserEvents.UserHasNewFriend>(When);
        }

        public static User CreateBasicUser(UserId gpid, string emailAddress, int metroId)
        {
            // do any business logic to protect invariants
            // note: validation, like email address is well formed, is not something that should be done in here - 
            // that should be taken care of before this method is invoked, using validators
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

        public void AddFriend(AddFriendToUser command)
        {
            if(_disabled)
                throw new AggregateException("Can not add a friend to a disabled user.");

            if(!_friends.Contains(new UserId(command.FriendsGpid)))
                ApplyChange(UserEvents.NewFriend(_id, new UserId(command.FriendsGpid), command.FName, command.LName));
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
            _friends.Add(new UserId(@event.FriendsGpid));
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
    }
}
