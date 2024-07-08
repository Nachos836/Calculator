using MessagePipe;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Calc.Presentation.DI.Presentation
{
    using Common.Dependencies;
    using ViewsManagement;

    using MainWindow = Calculator.MainWindowMVP;
    using ErrorPopup = Calculator.ErrorPopupMVP;

    [AddComponentMenu("Game/Architecture/Presentation Scope")]
    [DisallowMultipleComponent]
    internal sealed class PresentationScope : LifetimeScope
    {
        [Header("Main Window MVP")]
        [SerializeField] private MainWindow.Presenter _mainWindowPresenter = default!;
        [SerializeField] private MainWindow.View _mainWindowView = default!;
        [Header("Error Popup MVP")]
        [SerializeField] private ErrorPopup.Presenter _errorPopupPresenter = default!;
        [SerializeField] private ErrorPopup.View _errorPopupView = default!;
        [Header("Views Router")]
        [SerializeField] private ViewRouter _viewRouter = default!;

        protected override void Configure(IContainerBuilder builder)
        {
            base.Configure(builder);

            builder.Register(static _ => Application.Calculator.Build("+"), Lifetime.Singleton);

            var options = builder.RegisterMessagePipe();
            builder.RegisterMessageBroker<CalculationErrorOccured>(options);
            builder.RegisterMessageBroker<CalculationErrorProcessed>(options);

            builder.AddLogger<MainWindow.Presenter>();
            builder.AddLogger<ErrorPopup.Presenter>();

            builder.Register<MainWindow.Model>(Lifetime.Singleton)
                .AsImplementedInterfaces()
                .AsSelf();
            builder.Register<ErrorPopup.Model>(Lifetime.Singleton)
                .AsImplementedInterfaces()
                .AsSelf();

            builder.RegisterBuildCallback(container =>
            {
                container.Inject(_mainWindowPresenter);
                container.Inject(_mainWindowView);

                container.Inject(_errorPopupPresenter);
                container.Inject(_errorPopupView);

                container.Inject(_viewRouter);
            });
        }
    }
}
