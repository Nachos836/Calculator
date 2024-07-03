using System;
using Microsoft.Extensions.Logging;
using VContainer.Unity;
using ZLogger;

namespace Calc.Architecture.Presentation
{
    internal sealed class ApplicationEntryPoint : IInitializable
    {
        private readonly ILogger<ApplicationEntryPoint> _logger;

        public ApplicationEntryPoint(ILogger<ApplicationEntryPoint> logger)
        {
            _logger = logger;
        }

        void IInitializable.Initialize()
        {
            _logger.Info("Application is started!");
        }
    }

    // ReSharper disable once InconsistentNaming
    internal static partial class ApplicationEntryPoint_GeneratedLogFormats
    {
        [ZLoggerMessage(LogLevel.Trace, "{message}")]
        public static partial void Info(this ILogger<ApplicationEntryPoint> logger, string message);

        [ZLoggerMessage(LogLevel.Debug, "{message}")]
        public static partial void Debug(this ILogger<ApplicationEntryPoint> logger, string message);

        [ZLoggerMessage(LogLevel.Warning, "{message}")]
        public static partial void Warning(this ILogger<ApplicationEntryPoint> logger, string message);

        [ZLoggerMessage(LogLevel.Error, "{message}")]
        public static partial void Error(this ILogger<ApplicationEntryPoint> logger, string message);

        [ZLoggerMessage(LogLevel.Critical)]
        public static partial void Exception(this ILogger<ApplicationEntryPoint> logger, Exception ex);
    }
}
