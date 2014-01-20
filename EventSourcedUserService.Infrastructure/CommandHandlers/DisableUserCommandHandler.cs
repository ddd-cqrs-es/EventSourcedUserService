using AggregateSource;
using EventStore.ClientAPI;
using UserService.DomainModel;
using UserService.DomainModel.Commands;

namespace UserService.Infrastructure.CommandHandlers
{
    public class DisableUserCommandHandler : CommandHandlerBase<DisableUser>, ICommandHandler<DisableUser>
    {
        public DisableUserCommandHandler(IEventStoreConnection connection, IRepository<User> repository, UnitOfWork unitOfWork) : base(connection, repository, unitOfWork)
        {
        }

        public override void Handle(DisableUser command)
        {
            var gpid = command.Gpid.ToString();
            var user = Repository.Get(gpid);

            user.Disable();
            Repository.Add(gpid, user);
        }
    }
}
