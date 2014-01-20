using System;
using AggregateSource;
using EventStore.ClientAPI;
using UserService.DomainModel;
using UserService.DomainModel.Commands;

namespace UserService.Infrastructure.CommandHandlers
{
    public class AddFriendToUserCommandHandler : CommandHandlerBase<AddFriendToUser>, ICommandHandler<AddFriendToUser>
    {
        public AddFriendToUserCommandHandler(IEventStoreConnection connection, IRepository<User> repository, UnitOfWork unitOfWork, Func<UserId, string> streamNameFactory) 
            : base(connection, repository, unitOfWork, streamNameFactory)
        {
        }

        public override void Handle(AddFriendToUser command)
        {
            var gpid = new UserId(command.Gpid);
            var user = Repository.Get(StreamNameFactory(gpid));

            user.AddFriend(gpid, new UserId(command.FriendsGpid), command.FName, command.LName);
            Repository.Add(StreamNameFactory(user.Id), user);
        }
    }
}
