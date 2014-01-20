using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AggregateSource;
using EventStore.ClientAPI;
using Newtonsoft.Json;
using UserService.DomainModel;

namespace UserService.Infrastructure.CommandHandlers
{
    public abstract class CommandHandlerBase<TCommand> : ICommandHandler<TCommand>
    {
        private readonly IRepository<User> _repository;
        private readonly UnitOfWork _unitOfWork;
        private readonly IEventStoreConnection _connection;
        private readonly Func<UserId, string> _streamNameDelegate;

        public Func<UserId, string> StreamNameFactory
        {
            get { return _streamNameDelegate; }
        }

        protected IRepository<User> Repository
        {
            get { return _repository; }
        }

        protected UnitOfWork UnitOfWork
        {
            get { return _unitOfWork; }
        }

        protected IEventStoreConnection Connection
        {
            get { return _connection; }
        }

        protected CommandHandlerBase(IEventStoreConnection connection, IRepository<User> repository, UnitOfWork unitOfWork, Func<UserId, string> streamNameDelegate)
        {
            _connection = connection;
            _repository = repository;
            _unitOfWork = unitOfWork;
            _streamNameDelegate = streamNameDelegate;
        }

        public void HandleCommand(TCommand itemCommand)
        {
            Handle(itemCommand);
            WriteUnitOfWork(_unitOfWork);
        }

        public abstract void Handle(TCommand command);


        private void WriteUnitOfWork(UnitOfWork unitOfWork)
        {
            var affected = unitOfWork.GetChanges().Single();

            Write(new[] { affected });
        }

        private void Write(IEnumerable<Aggregate> affected)
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
                    JsonSerializer.Create().Serialize(writer, @event);
                    writer.Flush();
                }
                return stream.ToArray();
            }
        }
    }
}