using System;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using ZLogger;

namespace Calc.Architecture.Presentation.DI.Root
{
    using Dependencies;
    using Common.Dependencies;

    [AddComponentMenu("Game/Architecture/Root Scope")]
    [DisallowMultipleComponent]
    internal sealed class RootScope : LifetimeScope
    {
        private IDisposable? _loggerHandler;

        protected override void Configure(IContainerBuilder builder)
        {
            base.Configure(builder);

            builder
                .AddGlobalLogger(out var logger, out _loggerHandler)
                .AddLogger<ApplicationEntryPoint>();

            builder.RegisterEntryPoint<ApplicationEntryPoint>();
            builder.RegisterEntryPointExceptionHandler(exception => logger.ZLogCritical(exception: exception, $"EntryPoint Exception Handler:"));
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            _loggerHandler?.Dispose();
        }
    }
}
