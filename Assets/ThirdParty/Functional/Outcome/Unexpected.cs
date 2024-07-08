using System;

namespace ThirdParty.Functional.Outcome
{
    internal static class Unexpected
    {
        private static readonly Lazy<Exception> LazyError = new (static () => new Exception("Execution is Aborted by Exception"));
        private static readonly Lazy<Exception> LazyUnreachable = new (static () => new Exception("Execution shouldn't have happened"));

        public static Exception Error { get; } = LazyError.Value;
        public static Exception Impossible { get; } = LazyUnreachable.Value;
    }
}
