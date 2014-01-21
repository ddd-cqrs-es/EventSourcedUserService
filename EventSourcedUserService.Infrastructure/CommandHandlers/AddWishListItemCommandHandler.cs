using System;
using AggregateSource;
using EventStore.ClientAPI;
using UserService.DomainModel;
using UserService.DomainModel.Commands;

namespace UserService.Infrastructure.CommandHandlers
{
    public class AddWishListItemCommandHandler : CommandHandlerBase<AddWishListItem>, ICommandHandler<AddWishListItem>
    {
        public AddWishListItemCommandHandler(IEventStoreConnection connection, IRepository<User> repository, UnitOfWork unitOfWork, Func<UserId, string> streamNameDelegate) 
            : base(connection, repository, unitOfWork, streamNameDelegate)
        {
        }

        public override void Handle(AddWishListItem command)
        {
            var gpid = new UserId(command.Gpid);
            var user = Repository.Get(StreamNameFactory(gpid));

            user.AddWishListItem(gpid, new WishListItemId(command.WishListItemId), new RestaurantId(command.RestaurantId), command.Notes);
        }
    }
}
