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

        public UserController(ICommandHandler<CreateBasicUser> createUserCommandHandler, ICommandHandler<AddFriendToUser> addFriendCommandHandler)
        {
            _createUserCommandHandler = createUserCommandHandler;
            _addFriendCommandHandler = addFriendCommandHandler;
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
                var msg = string.Format("Error Occured on calling CreateBareboneUser()");
                return Request.CreateResponse(HttpStatusCode.InternalServerError, msg);
            }
        }
    }
}