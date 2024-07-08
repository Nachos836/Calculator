using Microsoft.Extensions.Logging;
using UnityEngine;
using VContainer;

namespace Calc.Presentation.Calculator.ErrorPopupMVP
{
    internal sealed class Presenter : MonoBehaviour
    {
        [SerializeField] [HideInInspector] private View _view = default!;

        private ILogger<Presenter> _logger = default!;
        private Model _model = default!;

        [Inject]
        internal void Construct(ILogger<Presenter> logger, Model model)
        {
            _logger = logger;
            _model = model;
        }

        private void Reset() => _view = GetComponent<View>();
        private void OnEnable() => _view.SubmitPressed += HandleErrorButtonClicked;
        private void OnDisable() => _view.SubmitPressed -= HandleErrorButtonClicked;

        private void HandleErrorButtonClicked()
        {
            if ( _model.TryHandleError(out var message))
            {
                _view.UpdateErrorDescription(message);
            }
        }
    }
}
