using AggregateSource;
using EventStore.ClientAPI;
using UserService.DomainModel;
using UserService.DomainModel.Commands;

namespace UserService.Infrastructure.CommandHandlers
{
    public class CreateBasicUserCommandHandler : CommandHandlerBase<CreateBasicUser>, Handles<CreateBasicUser>
    {
        public CreateBasicUserCommandHandler(IEventStoreConnection connection, IRepository<User> repository, UnitOfWork unitOfWork)
            : base(connection, repository, unitOfWork)
        {
        }

        public override void Handle(CreateBasicUser createUserCommand)
        {
            var user = User.CreateBasicUser(new UserId(createUserCommand.GlobalPersonId), createUserCommand.EmailAddress,
                                            createUserCommand.MetroId);

            Repository.Add(user.Id.ToString(), user);
        }
    }
}
