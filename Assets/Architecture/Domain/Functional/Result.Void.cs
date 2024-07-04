using System;
using System.Runtime.CompilerServices;

namespace Calc.Domain.Functional
{
    using Outcome;

    public readonly struct Result
    {
        private readonly (bool Provided, Exception Value) _error;

        private Result(Exception error) => _error = (Provided: true, error);

        public static Result Success { get; } = new ();
        public static Result Error { get; } = new (Unexpected.Error);

        public bool IsSuccessful => _error.Provided is false;
        public bool IsFailure => _error.Provided;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Result (Exception error) => new (error);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Result FromException(Exception exception) => exception;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TMatch Match<TMatch>(Func<TMatch> success, Func<Exception, TMatch> error)
        {
            return IsSuccessful
                ? success.Invoke()
                : error.Invoke(_error.Value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Match(Action success, Action<Exception> error)
        {
            if (IsSuccessful)
            {
                success.Invoke();
            }
            else
            {
                error.Invoke(_error.Value);
            }
        }
    }
}
