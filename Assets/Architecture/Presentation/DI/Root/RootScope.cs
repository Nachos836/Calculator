using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using VContainer;
using VContainer.Unity;

namespace Calc.Presentation.DI.Root
{
    using Dependencies;
    using Common.Dependencies;

    [AddComponentMenu("Game/Architecture/Root Scope")]
    [DisallowMultipleComponent]
    internal sealed class RootScope : LifetimeScope
    {
        [Header("Main Scene to load")]
        [SerializeField] private AssetReference _mainScene = default!;

        [Header("Debug Build Stuff")]
        [SerializeField] private GameObject _debugPanel = default!;

        private IDisposable? _loggerHandler;

        protected override void Awake()
        {
        #if DEBUG && !UNITY_EDITOR
            _debugPanel.SetActive(true);
        # endif

            base.Awake();
        }

        protected override void Configure(IContainerBuilder builder)
        {
            base.Configure(builder);

            builder
                .AddGlobalLogger(out var logger, out _loggerHandler)
                .AddLogger<ApplicationEntryPoint>();

            builder.RegisterEntryPoint<ApplicationEntryPoint>()
                .WithParameter(_mainScene)
                .WithParameter((MonoBehaviour) this);
            builder.RegisterEntryPointExceptionHandler(logger.HandleException);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            _loggerHandler?.Dispose();
        }
    }
}
