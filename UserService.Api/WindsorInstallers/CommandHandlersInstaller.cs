using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using UserService.DomainModel.Commands;
using UserService.Infrastructure;
using UserService.Infrastructure.CommandHandlers;

namespace UserService.Api.WindsorInstallers
{
    public class CommandHandlersInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component
                                   .For<ICommandHandler<CreateBasicUser>>()
                                   .ImplementedBy<CreateBasicUserCommandHandler>()
                                   .LifestyleTransient());

            container.Register(Component
                                   .For<ICommandHandler<AddWishListItem>>()
                                   .ImplementedBy<AddWishListItemCommandHandler>()
                                   .LifestyleTransient());

            container.Register(Component
                                   .For<ICommandHandler<AddFriendToUser>>()
                                   .ImplementedBy<AddFriendToUserCommandHandler>()
                                   .LifestyleTransient());

            container.Register(Component
                                   .For<ICommandHandler<DisableUser>>()
                                   .ImplementedBy<DisableUserCommandHandler>()
                                   .LifestyleTransient());
        }
    }
}