using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using UserService.DomainModel.Commands;
using UserService.Infrastructure;

namespace UserService.Api.Controllers
{
    public class UserController : ApiController
    {
        // Normally you would use some type of command router or bus to dispatch
        // the commands to the relevant consumer.
        private readonly ICommandHandler<CreateBasicUser> _createUserCommandHandler;
        private readonly ICommandHandler<AddFriendToUser> _addFriendCommandHandler;
        private readonly ICommandHandler<DisableUser> _disableUserCommandHandler;
        private readonly ICommandHandler<AddWishListItem> _addWishListItemCommandHandler; 

        public UserController(ICommandHandler<CreateBasicUser> createUserCommandHandler, ICommandHandler<AddFriendToUser> addFriendCommandHandler, ICommandHandler<DisableUser> disableUserCommandHandler, ICommandHandler<AddWishListItem> addWishListItemCommandHandler)
        {
            _createUserCommandHandler = createUserCommandHandler;
            _addFriendCommandHandler = addFriendCommandHandler;
            _disableUserCommandHandler = disableUserCommandHandler;
            _addWishListItemCommandHandler = addWishListItemCommandHandler;
        }

        public HttpResponseMessage PostCreateBarebonesUser(string email, int? metroid = null)
        {
            try
            {
                var resolvedMetroId = metroid.HasValue ? metroid.Value : 0;
                var command = new CreateBasicUser { EmailAddress = email, GlobalPersonId = Guid.NewGuid(), MetroId = resolvedMetroId };
                _createUserCommandHandler.HandleCommand(command);
                return Request.CreateResponse(HttpStatusCode.Created);
            }
            catch (Exception ex)
            {
                var msg = string.Format("Error Occured on calling CreateBareboneUser(): {0}", ex);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, msg);
            }
        }

        public HttpResponseMessage PostDisableUser(string gpid)
        {
            try
            {
                var command = new DisableUser(Guid.Parse(gpid));
                _disableUserCommandHandler.HandleCommand(command);
                return Request.CreateResponse(HttpStatusCode.Created);
            }
            catch (Exception ex)
            {
                var msg = string.Format("Error Occured on calling PostDisableUser({0}): {1}", gpid, ex);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, msg);
            }
        }

        public HttpResponseMessage PostAddFriend(string gpid, string friendsGpid, string fName, string lName)
        {
            try
            {
                var command = new AddFriendToUser(Guid.Parse(gpid), Guid.Parse(friendsGpid), fName, lName, null, null, null);
                _addFriendCommandHandler.HandleCommand(command);
                return Request.CreateResponse(HttpStatusCode.Created);
            }
            catch (Exception ex)
            {
                var msg = string.Format("Error Occured on calling PostAddFriend({0}): {1}", gpid, ex);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, msg);
            }
        }

        public HttpResponseMessage PostAddWishListItem(string gpid, string wishListItemId, string restaurantId,
                                                       string notes)
        {
            try
            {
                var command = new AddWishListItem(Guid.Parse(gpid), Guid.Parse(wishListItemId), Guid.Parse(restaurantId),
                                                  notes);
                _addWishListItemCommandHandler.HandleCommand(command);
                return Request.CreateResponse(HttpStatusCode.Created);
            }
            catch (Exception ex)
            {
                var msg = string.Format("Error Occured on calling PostAddWishListItem({0}): {1}", gpid, ex);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, msg);
            }
        }
    }
}