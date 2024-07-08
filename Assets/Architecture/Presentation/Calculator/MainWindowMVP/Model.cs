using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using MessagePipe;
using ThirdParty.Functional;
using ThirdParty.Functional.Outcome;

namespace Calc.Presentation.Calculator.MainWindowMVP
{
    using Application.EventSourcing;
    using ViewsManagement;

    internal sealed class Model
    {
        private readonly Application.Calculator _calculator;
        private readonly IPublisher<CalculationErrorOccured> _errorOccuredEvent;
        private readonly ICalculationEventsSourcing _calculationEvents;

        public Model
        (
            Application.Calculator calculator,
            IPublisher<CalculationErrorOccured> errorOccuredEvent,
            ICalculationEventsSourcing calculationEvents
        ) {
            _calculator = calculator;
            _errorOccuredEvent = errorOccuredEvent;
            _calculationEvents = calculationEvents;
        }

        public SessionHandler CreateOrLoadSessionAsync()
        {
            return new SessionHandler(_calculator, _errorOccuredEvent, _calculationEvents, Guid.Empty);
        }
    }

    internal sealed record SessionHandler : IUniTaskAsyncDisposable
    {
        private readonly Application.Calculator _calculator;
        private readonly IPublisher<CalculationErrorOccured> _errorOccuredEvent;
        private readonly ICalculationEventsSourcing _calculationEvents;
        private readonly Guid _session;

        public IEnumerable<string> CalculatedExpressions => _calculationEvents.MaterializeView(_session);
        public string PeekExpression => _calculationEvents.GetLatestFromMaterializedView(_session);

        internal SessionHandler
        (
            Application.Calculator calculator,
            IPublisher<CalculationErrorOccured> errorOccuredEvent,
            ICalculationEventsSourcing calculationEvents,
            Guid session
        ) {
            _calculator = calculator;
            _errorOccuredEvent = errorOccuredEvent;
            _calculationEvents = calculationEvents;
            _session = session;
        }

        public RichResult<decimal, string> Calculate(string input)
        {
            return _calculator.Evaluate(input);
        }

        public void PublishResult(decimal value, string resultedString)
        {
            _calculationEvents.Append(new ResultEvent(_session, DateTime.UtcNow, value, resultedString));
        }

        public void PublishFailure(Expected.Failure failure)
        {
            _calculationEvents.Append(new ErrorEvent(_session, DateTime.UtcNow, failure.Message));
        }

        public void PublishError(Exception exception)
        {
            _errorOccuredEvent.Publish(new CalculationErrorOccured(exception.Message));
        }

        public void CloseSession()
        {
            _calculationEvents.Append(new ClosedEvent(_session, DateTime.UtcNow));
        }

        public UniTask DisposeAsync()
        {
            return UniTask.CompletedTask;
        }
    }
}
