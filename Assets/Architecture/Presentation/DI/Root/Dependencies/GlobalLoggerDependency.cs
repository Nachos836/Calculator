using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;
using VContainer;
using ZLogger;
using ZLogger.Unity;

namespace Calc.Presentation.DI.Root.Dependencies
{
    internal static class GlobalLoggerDependency
    {
        [SuppressMessage("IDisposableAnalyzers.Correctness", "IDISP001:Dispose created")]
        public static IContainerBuilder AddGlobalLogger(this IContainerBuilder builder, out ILogger logger, out IDisposable handler)
        {
            var factory = LoggerFactory.Create(static logging =>
            {
                logging.SetMinimumLevel(LogLevel.Trace);
                logging.AddZLoggerUnityDebug();
            });

            builder.RegisterInstance(factory);

            logger = factory.CreateLogger(categoryName: nameof(RootScope));
            handler = factory;

            return builder;
        }
    }

    [SuppressMessage("ReSharper", "InconsistentNaming")]
    internal static partial class GlobalLoggerDependency_GeneratedLogFormats
    {
        [ZLoggerMessage(LogLevel.Critical)]
        public static partial void HandleException(this ILogger logger, Exception ex);
    }
}
