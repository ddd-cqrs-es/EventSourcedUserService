using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AggregateSource;
using EventStore.ClientAPI;
using EventStore.ClientAPI.SystemData;
using UserService.DomainModel.Commands;
using UserService.Infrastructure.CommandHandlers;

namespace UserService.Infrastructure
{
    public class Tester
    {
        public void HandleUserCommands()
        {
            IEventStoreConnection connection = null;
            var credentials = new UserCredentials("admin", "changeit");
            var unitOfWork = new UnitOfWork();
            var repository = RepositoryFactory.Create(unitOfWork, connection, credentials);
            ICommandHandler<CreateBasicUser> handler = new CreateBasicUserCommandHandler(connection, repository, unitOfWork);

            //handler.HandleCommand();
        }
    }
}
