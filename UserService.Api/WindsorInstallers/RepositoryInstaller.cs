using AggregateSource;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using EventStore.ClientAPI;
using EventStore.ClientAPI.SystemData;
using UserService.DomainModel;
using UserService.Infrastructure;

namespace UserService.Api.WindsorInstallers
{
    public class RepositoryInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component
                                   .For<UnitOfWork>()
                                   .ImplementedBy<UnitOfWork>()
                                   .LifestyleTransient());

            container.Register(Component
                                   .For<IRepository<User>>()
                                   .UsingFactoryMethod(
                                       () =>
                                       RepositoryFactory.Create(container.Resolve<UnitOfWork>(),
                                                                container.Resolve<IEventStoreConnection>(),
                                                                new UserCredentials("admin", "changeit")))
                                   .LifestyleTransient());
        }
    }
}