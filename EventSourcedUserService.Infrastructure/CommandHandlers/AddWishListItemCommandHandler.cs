using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.DomainModel.Commands;

namespace UserService.Infrastructure.CommandHandlers
{
    public class AddWishListItemCommandHandler : ICommandHandler<AddWishListItem>
    {
        public void HandleCommand(AddWishListItem itemCommand)
        {
            throw new NotImplementedException();
        }
    }
}
