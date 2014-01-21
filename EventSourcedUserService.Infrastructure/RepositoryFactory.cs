using System;
using System.Collections.Generic;
using System.IO;
using AggregateSource;
using AggregateSource.EventStore;
using AggregateSource.EventStore.Resolvers;
using EventStore.ClientAPI;
using EventStore.ClientAPI.SystemData;
using Newtonsoft.Json;
using UserService.DomainModel;

namespace UserService.Infrastructure
{
    public static class RepositoryFactory
    {
        public static Repository<User> Create(UnitOfWork unitOfWork, IEventStoreConnection connection, UserCredentials credentials) 
        {
            return new Repository<User>(() => User.Factory(), unitOfWork, connection,
                                        new EventReaderConfiguration(
                                            new SliceSize(512),
                                            new JsonDeserializer(),
                                            new PassThroughStreamNameResolver(),
                                            new FixedStreamUserCredentialsResolver(credentials)));
        }

        class JsonDeserializer : IEventDeserializer
        {
            public IEnumerable<object> Deserialize(ResolvedEvent resolvedEvent)
            {
                var type = Type.GetType(resolvedEvent.Event.EventType + ", UserService.DomainModel", true);
                using (var stream = new MemoryStream(resolvedEvent.Event.Data))
                {
                    using (var reader = new StreamReader(stream))
                    {
                        yield return JsonSerializer.CreateDefault().Deserialize(reader, type);
                    }
                }
            }
        }
    }
}
