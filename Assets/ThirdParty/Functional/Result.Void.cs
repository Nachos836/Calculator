using System;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;

namespace ThirdParty.Functional
{
    using Outcome;

    public readonly struct Result
    {
        private readonly (bool Provided, Exception Value) _error;

        private Result(Exception error) => _error = (Provided: true, error);

        [Pure] public static Result Success { get; } = new ();
        [Pure] public static Result Error { get; } = new (Unexpected.Error);
        [Pure] public static Result Impossible { get; } = new (Unexpected.Impossible);

        [Pure] public bool IsSuccessful => _error.Provided is false;
        [Pure] public bool IsFailure => _error.Provided;

        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Result (Exception error) => new (error);

        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Result FromException(Exception exception) => exception;

        [Pure]
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

        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string ToString()
        {
            return this switch
            {
                _ when _error.Provided => _error.Value.Message,
                _ => nameof(Success)
            };
        }
    }
}
