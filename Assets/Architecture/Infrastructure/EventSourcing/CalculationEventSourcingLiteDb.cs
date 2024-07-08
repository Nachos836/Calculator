using System;
using LiteDB;
using System.Linq;
using System.Collections.Generic;

namespace Calc.Infrastructure.EventSourcing
{
    using Application.EventSourcing;

    internal sealed class CalculationEventSourcingLiteDb : ICalculationEventsSourcing, IDisposable
    {
        private readonly LiteDatabase _database;
        private readonly ILiteCollection<Event> _collection;

        public CalculationEventSourcingLiteDb(string databasePath)
        {
            _database = new LiteDatabase(databasePath);
            _collection = _database.GetCollection<Event>(name: "events");
        }

        void ICalculationEventsSourcing.Append(Event @event)
        {
            if (@event is ClosedEvent)
            {
                _collection.DeleteAll();
            }

            _collection.Insert(@event);
        }

        IEnumerable<string> ICalculationEventsSourcing.MaterializeView(Guid session)
        {
            return _collection.Find(@event => @event.StreamId == session)
                .OrderBy(static @event => @event.CreatedTimestamp)
                .Select(static @event => @event.RawView);
        }

        string ICalculationEventsSourcing.GetLatestFromMaterializedView(Guid session)
        {
            var latest = _collection.Find(@event => @event.StreamId == session)
                .OrderByDescending(static @event => @event.CreatedTimestamp)
                .FirstOrDefault();
            return latest is not null and not ClosedEvent
                ? latest.RawView
                : string.Empty;
        }

        void IDisposable.Dispose()
        {
            _database.Dispose();
        }
    }
}
