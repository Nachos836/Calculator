using Microsoft.Extensions.Logging;
using VContainer;

namespace Calc.Presentation.DI.Common.Dependencies
{
    internal static class DedicatedLoggerDependency
    {
        public static IContainerBuilder AddLogger<TTarget>(this IContainerBuilder builder, Lifetime lifetime = Lifetime.Singleton)
        {
            builder.Register
            (
                implementationConfiguration: static resolver =>
                {
                    return resolver.Resolve<ILoggerFactory>()
                        .CreateLogger<TTarget>();
                },
                lifetime
            );

            return builder;
        }
    }
}
