using System;

namespace Calc.Application.EventSourcing
{
    public abstract record Event(Guid StreamId, DateTime CreatedTimestamp)
    {
        public abstract string RawView { get; }
    }

    public sealed record ResultEvent(Guid StreamId, DateTime CreatedTimestamp, decimal Value, string Expression) : Event(StreamId, CreatedTimestamp)
    {
        public override string RawView => Expression;
    }

    public sealed record ErrorEvent(Guid StreamId, DateTime CreatedTimestamp, string Message) : Event(StreamId, CreatedTimestamp)
    {
        public override string RawView => Message;
    }

    public sealed record ClosedEvent(Guid StreamId, DateTime CreatedTimestamp) : Event(StreamId, CreatedTimestamp)
    {
        public override string RawView => "Error";
    }
}
