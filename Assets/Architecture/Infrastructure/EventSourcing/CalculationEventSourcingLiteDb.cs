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

                return;
            }

            _collection.Insert(@event);
        }

        IEnumerable<string> ICalculationEventsSourcing.MaterializeView(Guid session)
        {
            using var events = _collection.Find(@event => @event.StreamId == session)
                .OrderBy(static @event => @event.CreatedTimestamp)
                .GetEnumerator();

            if (events.MoveNext() is false) yield break;
            if (events.Current is ClosedEvent)
            {
                _collection.DeleteAll();

                yield break;
            }

            while (events.MoveNext())
            {
                yield return events.Current!.RawView;
            }
        }

        string? ICalculationEventsSourcing.GetLatestFromMaterializedView(Guid session)
        {
            var latest = _collection.Find(@event => @event.StreamId == session)
                .OrderByDescending(static @event => @event.CreatedTimestamp)
                .FirstOrDefault();
            return latest is not ClosedEvent
                ? latest?.RawView
                : null;
        }

        void IDisposable.Dispose()
        {
            _database.Dispose();
        }
    }
}
