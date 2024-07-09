using System;
using System.Diagnostics.CodeAnalysis;
using Cysharp.Threading.Tasks;
using Microsoft.Extensions.Logging;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;
using VContainer.Unity;
using ZLogger;

namespace Calc.Presentation
{
    internal sealed class ApplicationEntryPoint : IInitializable
    {
        private readonly ILogger<ApplicationEntryPoint> _logger;
        private readonly AssetReference _mainScene;
        private readonly MonoBehaviour _loader;

        public ApplicationEntryPoint(ILogger<ApplicationEntryPoint> logger, AssetReference mainScene, MonoBehaviour loader)
        {
            _logger = logger;
            _mainScene = mainScene;
            _loader = loader;
        }

        void IInitializable.Initialize()
        {
            _logger.Info("Application is started!");

            _mainScene.LoadSceneAsync(LoadSceneMode.Additive)
                .ToUniTask(_loader)
                .Forget(_logger.Exception);
        }
    }

    [SuppressMessage("ReSharper", "InconsistentNaming")]
    internal static partial class ApplicationEntryPoint_GeneratedLogFormats
    {
        [ZLoggerMessage(LogLevel.Trace, "{message}")]
        public static partial void Info(this ILogger<ApplicationEntryPoint> logger, string message);

        [ZLoggerMessage(LogLevel.Debug, "{message}")]
        public static partial void Debug(this ILogger<ApplicationEntryPoint> logger, string message);

        [ZLoggerMessage(LogLevel.Warning, "{message}")]
        public static partial void Warning(this ILogger<ApplicationEntryPoint> logger, string message);

        [ZLoggerMessage(LogLevel.Error)]
        public static partial void Exception(this ILogger<ApplicationEntryPoint> logger, Exception ex);

        [ZLoggerMessage(LogLevel.Critical)]
        public static partial void Critical(this ILogger<ApplicationEntryPoint> logger, Exception ex);
    }
}
