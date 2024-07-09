using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;
using UnityEngine;
using VContainer;
using ZLogger;

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

        private void OnEnable()
        {
            _model.ErrorOccured += HandleError;
            _view.SubmitPressed += OnSubmitButtonClicked;
        }

        private void OnDisable()
        {
            _view.SubmitPressed -= OnSubmitButtonClicked;
            _model.ErrorOccured -= HandleError;
        }

        private void HandleError(string message)
        {
            _view.UpdateErrorDescription(message);
        }

        private void OnSubmitButtonClicked()
        {
            if ( _model.TryPeekOnLastError(out var message))
            {
                _logger.Critical(new Exception($"Error: \"{message}\" was not handled!"));
            }
            else
            {
                _model.NotifyErrorHandled();
            }
        }
    }

    [SuppressMessage("ReSharper", "InconsistentNaming")]
    internal static partial class ErrorPopupPresenter_GeneratedLogFormats
    {
        [ZLoggerMessage(LogLevel.Trace, "{message}")]
        public static partial void Info(this ILogger<Presenter> logger, string message);

        [ZLoggerMessage(LogLevel.Debug, "{message}")]
        public static partial void Debug(this ILogger<Presenter> logger, string message);

        [ZLoggerMessage(LogLevel.Warning, "{message}")]
        public static partial void Warning(this ILogger<Presenter> logger, string message);

        [ZLoggerMessage(LogLevel.Error)]
        public static partial void Exception(this ILogger<Presenter> logger, Exception ex);

        [ZLoggerMessage(LogLevel.Critical)]
        public static partial void Critical(this ILogger<Presenter> logger, Exception ex);
    }
}
