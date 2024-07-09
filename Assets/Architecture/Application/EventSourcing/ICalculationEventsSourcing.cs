using System;
using System.Collections.Generic;

namespace Calc.Application.EventSourcing
{
    public interface ICalculationEventsSourcing
    {
        void Append(Event @event);
        IEnumerable<string> MaterializeView(Guid session);
        string? GetLatestFromMaterializedView(Guid session);
    }
}
