using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AggregateSource;
using AggregateSource.EventStore;
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
                                   .LifestyleSingleton());

            container.Register(Component
                                   .For<Repository<User>>()
                                   .UsingFactoryMethod(
                                       () =>
                                       RepositoryFactory.Create(container.Resolve<UnitOfWork>(),
                                                                container.Resolve<IEventStoreConnection>(),
                                                                new UserCredentials("admin", "changeit")))
                                   .LifestyleTransient());
        }
    }
}