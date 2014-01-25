using System.Net;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using EventStore.ClientAPI;
using EventStore.ClientAPI.SystemData;

namespace UserService.Api.WindsorInstallers
{
    public class EventStoreInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            const string ipAddress = "192.168.1.80";

            container.Register(Component
                                   .For<IEventStoreConnection>()
                                   .UsingFactoryMethod(() =>
                                       {
                                           var credentials = new UserCredentials("admin", "changeit");
                                           var connection = EventStoreConnection.Create(
                                               ConnectionSettings.Create().
                                                                  UseConsoleLogger().
                                                                  SetDefaultUserCredentials(
                                                                      credentials),
                                               new IPEndPoint(IPAddress.Parse(ipAddress), 1113),
                                               "UserServiceConnection");
                                           connection.Connect();
                                           return connection;
                                       }));
        }
    }
}