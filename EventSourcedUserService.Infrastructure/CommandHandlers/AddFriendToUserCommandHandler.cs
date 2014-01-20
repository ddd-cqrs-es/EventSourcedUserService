using AggregateSource;
using EventStore.ClientAPI;
using UserService.DomainModel;
using UserService.DomainModel.Commands;

namespace UserService.Infrastructure.CommandHandlers
{
    public class AddFriendToUserCommandHandler : CommandHandlerBase<AddFriendToUser>, ICommandHandler<AddFriendToUser>
    {
        public AddFriendToUserCommandHandler(IEventStoreConnection connection, IRepository<User> repository, UnitOfWork unitOfWork) 
            : base(connection, repository, unitOfWork)
        {
        }

        public override void Handle(AddFriendToUser command)
        {
            var gpid = command.Gpid.ToString();
            var user = Repository.Get(gpid);

            user.AddFriend(new UserId(command.Gpid), new UserId(command.FriendsGpid), command.FName, command.LName);
            Repository.Add(gpid, user);
        }
    }
}
