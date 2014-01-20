using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AggregateSource;
using EventStore.ClientAPI;
using EventStore.ClientAPI.SystemData;
using Newtonsoft.Json;

namespace UserService.Infrastructure
{
    public class EventStoreUnitOfWorkHandler
    {
        private readonly IEventStoreConnection _connection;

        public EventStoreUnitOfWorkHandler()
        {
            var credentials = new UserCredentials("admin", "changeit");
            _connection = EventStoreConnection.Create(
                ConnectionSettings.Create().
                    UseConsoleLogger().
                    SetDefaultUserCredentials(
                        credentials),
                new IPEndPoint(IPAddress.Loopback, 1113),
                "UserServiceConnection");
            _connection.Connect();

//            var repository = new Repository<ShoppingCart>(
//                ShoppingCart.Factory,
//                unitOfWork,
//                connection,
//                new EventReaderConfiguration(
//                    new SliceSize(512),
//                    new JsonDeserializer(),
//                    new PassThroughStreamNameResolver(),
//                    new FixedStreamUserCredentialsResolver(credentials)));
        }

        public void Handle(UnitOfWork unitOfWork)
        {
            var affected = unitOfWork.GetChanges().Single();
 
            Write(new[] {affected});
        }

        public void Write(IEnumerable<Aggregate> affected)
        {
            foreach (var aggregate in affected)
            {
                _connection.AppendToStream(
                    aggregate.Identifier,
                    aggregate.ExpectedVersion,
                    aggregate.Root.GetChanges().
                              Select(_ =>
                                     new EventData(
                                         Guid.NewGuid(),
                                         _.GetType().Name,
                                         true,
                                         ToJsonByteArray(_),
                                         new byte[0])));
            }
        }

        static byte[] ToJsonByteArray(object @event)
        {
            using (var stream = new MemoryStream())
            {
                using (var writer = new StreamWriter(stream))
                {
                    JsonSerializer.CreateDefault().Serialize(writer, @event);
                    writer.Flush();
                }
                return stream.ToArray();
            }
        }
    }
}
