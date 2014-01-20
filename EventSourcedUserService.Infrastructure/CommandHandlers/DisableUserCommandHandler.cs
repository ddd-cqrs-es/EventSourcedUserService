using System;
using AggregateSource;
using EventStore.ClientAPI;
using UserService.DomainModel;
using UserService.DomainModel.Commands;

namespace UserService.Infrastructure.CommandHandlers
{
    public class DisableUserCommandHandler : CommandHandlerBase<DisableUser>, ICommandHandler<DisableUser>
    {
        public DisableUserCommandHandler(IEventStoreConnection connection, IRepository<User> repository, UnitOfWork unitOfWork, Func<UserId, string> streamNameFactory) 
            : base(connection, repository, unitOfWork, streamNameFactory)
        {
        }

        public override void Handle(DisableUser command)
        {
            var gpid = new UserId(command.Gpid);
            var user = Repository.Get(StreamNameFactory(gpid));

            user.Disable();
            Repository.Add(StreamNameFactory(gpid), user);
        }
    }
}
