using System;
using AggregateSource.Testing;
using NUnit.Framework;
using UserService.DomainModel.Commands;
using UserService.DomainModel.Events;

namespace UserService.DomainModel.Tests
{
    [TestFixture]
    public class UserScenarios
    {
        private readonly Guid _gpid = Guid.NewGuid();

        [Test]
        public void CreatingNewUser()
        {
            var id = new UserId(Guid.NewGuid());
            var email = "person@domain.com";
            var metroId = 11;
            var command = new CreateBasicUser {EmailAddress = email, GlobalPersonId = id.Gpid, MetroId = metroId};

            new ConstructorScenarioFor<User>(() => User.CreateBasicUser(id, email, metroId)).
                Then(UserEvents.Created(email, id, metroId, false)).
                Assert();
        }

        [Test]
        public void DisableUser()
        {
            var id = new UserId(Guid.NewGuid());
            var email = "person@domain.com";
            var metroId = 11;

            new CommandScenarioFor<User>(User.Factory)
                .Given(UserEvents.Created(email, id, metroId, false))
                .When(user => user.Disable())
                .Then(UserEvents.Disabled(id))
                .Assert();
        }

        [Test]
        public void AlreadyDisabledUserThrows()
        {
            // this test may be mute; disabling an already disabled user may be
            // the result of some eventual consistency, and ignoring a second command
            // to disable would make the operation idempotent
            var id = new UserId(Guid.NewGuid());
            var email = "person@domain.com";
            var metroId = 11;

            new CommandScenarioFor<User>(User.Factory)
            .Given(UserEvents.Created(email, id, metroId, false))
            .Given(UserEvents.Disabled(id))
            .When(user => user.Disable())
            .Throws(new AggregateException(string.Format("User {0} is already disabled", id)))
            .Assert();
        }

        [Test]
        public void Enable()
        {
            var id = new UserId(Guid.NewGuid());
            var email = "person@domain.com";
            var metroId = 11;

            new CommandScenarioFor<User>(User.Factory)
                .Given(UserEvents.Created(email, id, metroId, false))
                .Given(UserEvents.Disabled(id))
                .When(user => user.Enable())
                .Then(UserEvents.Enabled(id))
                .Assert();
        }

        [Test]
        public void AddFriendsToUser()
        {
            var id = new UserId(Guid.NewGuid());
            var friendId = new UserId(Guid.NewGuid());
            var email = "person@domain.com";
            var metroId = 11;
            var fname = "John";
            var lname = "Jack";
            var command = new AddFriendToUser(id, friendId, fname, lname, null, null, null);

            new CommandScenarioFor<User>(User.Factory)
            .Given(UserEvents.Created(email, id, metroId, false))
            .When(user => user.AddFriend(new UserId(command.Gpid), new UserId(command.FriendsGpid), command.FName, command.LName))
            .Then(UserEvents.NewFriend(id, friendId, fname, lname))
            .Assert();
        }

        [Test]
        public void AddWishListItemToUser()
        {
            var id = new UserId(Guid.NewGuid());
            var email = "person@domain.com";
            var metroId = 11;
            var wishlistItemId = new WishListItemId(Guid.NewGuid());
            var restoId = new RestaurantId(Guid.NewGuid());

            new CommandScenarioFor<User>(User.Factory)
            .Given(UserEvents.Created(email, id, metroId, false))
            .When(user => user.AddWishListItem(id, wishlistItemId, restoId, "some notes"))
            .Then(UserEvents.WishListItemAdded(id, wishlistItemId, restoId, "some notes"))
            .Assert();
        }
    }
}
