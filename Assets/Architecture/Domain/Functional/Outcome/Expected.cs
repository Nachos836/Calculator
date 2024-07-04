using System;

namespace Calc.Domain.Functional.Outcome
{
    public static class Expected
    {
        private static readonly Lazy<Success> LazySuccess = new (static () => new Success());
        private static readonly Lazy<Unit> LazyUnit = new (static () => new Unit());

        private static readonly Lazy<Failure> LazyFailed= new (static () => new Failure(reason: "Execution Failed."));

        public static Success Success { get; } = LazySuccess.Value;
        public static Unit Unit { get; } = LazyUnit.Value;

        public static Failure Failed { get; } = LazyFailed.Value;

        public readonly struct Failure
        {
            private readonly Lazy<Exception> _exception;
            public readonly string Message;

            public Failure(string reason)
            {
                Message = reason;
                _exception = new Lazy<Exception>(() => new Exception(reason));
            }

            public Exception ToException() => _exception.Value;

            public override string ToString() => Message;
        }
    }
}
