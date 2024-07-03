using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;
using VContainer;
using ZLogger.Unity;

namespace Calc.Architecture.Presentation.DI.Root.Dependencies
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
}
