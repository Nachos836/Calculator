using System;
using System.Diagnostics.CodeAnalysis;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using UnityEngine;
using VContainer;
using ZLogger;

namespace Calc.Presentation.Calculator.MainWindowMVP
{
    [SuppressMessage("ReSharper", "Unity.IncorrectMethodSignature")]
    [AddComponentMenu("Game/Main Window/Main Window Presenter")]
    [RequireComponent(typeof(View))]
    internal sealed class Presenter : MonoBehaviour
    {
        [SerializeField] [HideInInspector] private View _view = default!;

        private ILogger<Presenter> _logger = default!;
        private Model _model = default!;

        private SessionHandler? _session;

        [Inject]
        internal void Construct(ILogger<Presenter> logger, Model model)
        {
            _logger = logger;
            _model = model;
        }

        private void Reset()
        {
            _view = GetComponent<View>();
        }

        [UsedImplicitly]
        private async UniTaskVoid OnEnable()
        {
            if (_session is not null)
            {
                await _session.DisposeAsync();
            }

            _session = _model.CreateOrLoadSessionAsync();

            _view.UpdateEvaluatedList(_session.CalculatedExpressions);
            _view.CloseRequested += _session.CloseSession;
            _view.CalculationsRequested += OnViewOnCalculationsRequested;
        }

        private void OnViewOnCalculationsRequested(string input)
        {
            _session!.Calculate(input).Match
            (
                success: (value, resultedString) =>
                {
                    _session.PublishResult(value, resultedString);

                    _view.UpdateInputField(value);

                    if (_session.PeekExpression is { } expression)
                    {
                        _view.UpdateEvaluatedList(expression);
                    }

                    _logger.Info(resultedString);
                },
                failure: failure =>
                {
                    _session.PublishFailure(failure);

                    if (_session.PeekExpression is { } expression)
                    {
                        _view.UpdateEvaluatedList(expression);
                    }

                    _logger.Warning(failure.Message);
                },
                error: exception =>
                {
                    _session.PublishError(exception);

                    _logger.Exception(exception);
                }
            );
        }

        [UsedImplicitly]
        private async UniTaskVoid OnDisable()
        {
            _view.ClearEvaluatedList();
            _view.ClearInputField();

            if (_session is not null)
            {
                await _session.DisposeAsync();

                _view.CloseRequested -= _session.CloseSession;
            }

            _view.CalculationsRequested -= OnViewOnCalculationsRequested;
        }
    }

    [SuppressMessage("ReSharper", "InconsistentNaming")]
    internal static partial class MainWindowPresenter_GeneratedLogFormats
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
