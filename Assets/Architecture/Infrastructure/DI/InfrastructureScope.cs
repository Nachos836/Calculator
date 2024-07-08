using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Calc.Infrastructure.DI
{
    using EventSourcing;

    internal sealed class InfrastructureScope : LifetimeScope
    {
        [SerializeField] private string _databasePath = "events_storage";

        protected override void Configure(IContainerBuilder builder)
        {
            base.Configure(builder);

            builder.Register<CalculationEventSourcingLiteDb>(Lifetime.Singleton)
                .WithParameter(UnityEngine.Application.persistentDataPath + "/" + _databasePath)
                .AsImplementedInterfaces();
        }
    }
}
