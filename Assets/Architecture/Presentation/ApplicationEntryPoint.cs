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
            _logger.ZLogDebug($"Application is started!");
        }
    }
}
