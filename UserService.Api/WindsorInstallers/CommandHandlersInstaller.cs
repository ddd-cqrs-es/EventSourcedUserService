using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using UserService.DomainModel.Commands;
using UserService.Infrastructure;

namespace UserService.Api.WindsorInstallers
{
    public class CommandHandlersInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component
                                   .For<ICommandHandler<CreateBasicUser>>()
                                   .ImplementedBy<UserCommandHandler>());
        }
    }
}