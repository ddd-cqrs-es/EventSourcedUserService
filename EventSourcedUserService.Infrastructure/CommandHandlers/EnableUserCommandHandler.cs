using System;
using AggregateSource;
using EventStore.ClientAPI;
using UserService.DomainModel;
using UserService.DomainModel.Commands;

namespace UserService.Infrastructure.CommandHandlers
{
    public class EnableUserCommandHandler : CommandHandlerBase<EnableUser>, ICommandHandler<EnableUser>
    {
        public EnableUserCommandHandler(IEventStoreConnection connection, IRepository<User> repository, UnitOfWork unitOfWork, Func<UserId, string> streamNameDelegate) 
            : base(connection, repository, unitOfWork, streamNameDelegate)
        {
        }

        public override void Handle(EnableUser command)
        {
            var gpid = new UserId(command.Gpid);
            var user = Repository.Get(StreamNameFactory(gpid));

            user.Enable();
        }
    }
}
