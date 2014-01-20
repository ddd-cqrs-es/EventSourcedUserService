using System;
using AggregateSource;
using EventStore.ClientAPI;
using UserService.DomainModel;
using UserService.DomainModel.Commands;

namespace UserService.Infrastructure.CommandHandlers
{
    public class CreateBasicUserCommandHandler : CommandHandlerBase<CreateBasicUser>, Handles<CreateBasicUser>
    {
        public CreateBasicUserCommandHandler(IEventStoreConnection connection, IRepository<User> repository, UnitOfWork unitOfWork, Func<UserId, string> streamNameFactory)
            : base(connection, repository, unitOfWork, streamNameFactory)
        {
        }

        public override void Handle(CreateBasicUser createUserCommand)
        {
            var user = User.CreateBasicUser(new UserId(createUserCommand.GlobalPersonId), createUserCommand.EmailAddress,
                                            createUserCommand.MetroId);

            Repository.Add(StreamNameFactory(user.Id), user);
        }
    }
}
