using System;
using MessagePipe;
using UnityEngine;
using VContainer;

namespace Calc.Presentation.ViewsManagement
{
    internal sealed class ViewRouter : MonoBehaviour
    {
        [SerializeField] private GameObject _mainWindow = default!;
        [SerializeField] private GameObject _errorPopup = default!;

        private ISubscriber<CalculationErrorOccured> _errorOccuredEvent = default!;
        private ISubscriber<CalculationErrorProcessed> _errorProcessedEvent = default!;

        private IDisposable? _errorOccuredSubscription;
        private IDisposable? _errorProcessedSubscription;

        [Inject]
        public void Construct
        (
            ISubscriber<CalculationErrorOccured> errorOccuredEvent,
            ISubscriber<CalculationErrorProcessed> errorProcessedEvent
        ) {
            _errorOccuredEvent = errorOccuredEvent;
            _errorProcessedEvent = errorProcessedEvent;
        }

        private void OnEnable()
        {
            _errorOccuredSubscription?.Dispose();
            _errorOccuredSubscription = _errorOccuredEvent.Subscribe(occured =>
            {
                _mainWindow.SetActive(false);
                _errorPopup.SetActive(true);
            });

            _errorProcessedSubscription?.Dispose();
            _errorProcessedSubscription = _errorProcessedEvent.Subscribe(_ =>
            {
                _errorPopup.SetActive(false);
                _mainWindow.SetActive(true);
            });
        }

        private void OnDisable()
        {
            _errorOccuredSubscription?.Dispose();
            _errorProcessedSubscription?.Dispose();
        }
    }
}
