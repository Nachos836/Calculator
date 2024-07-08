using System;
using MessagePipe;
using VContainer.Unity;

namespace Calc.Presentation.Calculator.ErrorPopupMVP
{
    using ViewsManagement;

    internal sealed class Model : IStartable, IDisposable
    {
        private readonly IPublisher<CalculationErrorProcessed> _errorProcessedEvent;
        private readonly ISubscriber<CalculationErrorOccured> _errorOccuredEvent;

        private CalculationErrorOccured? _lastError;
        private IDisposable? _errorOccuredSubscription;

        public Model(IPublisher<CalculationErrorProcessed> errorProcessedEvent, ISubscriber<CalculationErrorOccured> errorOccuredEvent)
        {
            _errorProcessedEvent = errorProcessedEvent;
            _errorOccuredEvent = errorOccuredEvent;
        }

        void IStartable.Start()
        {
            _errorOccuredSubscription?.Dispose();
            _errorOccuredSubscription = _errorOccuredEvent.Subscribe(error => _lastError = error);
        }

        public bool TryHandleError(out string message)
        {
            _errorProcessedEvent.Publish(new CalculationErrorProcessed());

            if (_lastError is not null)
            {
                message = _lastError.Message;
                _lastError = null;

                return true;
            }
            else
            {
                message = string.Empty;

                return false;
            }
        }

        void IDisposable.Dispose()
        {
            _errorOccuredSubscription?.Dispose();
        }
    }
}
