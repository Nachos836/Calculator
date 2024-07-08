using System;
using System.Runtime.CompilerServices;

namespace ThirdParty.Functional
{
    using Outcome;

    public readonly struct RichResult
    {
        private readonly (Exception Error, bool Provided) _unexpected;
        private readonly (Expected.Failure Failure, bool Provided) _expected;

        private RichResult(Expected.Failure expected)
        {
            _expected = (expected, Provided: true);
            _unexpected = default;
        }

        private RichResult(Exception unexpected)
        {
            _expected = default;
            _unexpected = (unexpected, Provided: true);
        }

        public static RichResult Success { get; } = new ();
        public static RichResult Failure { get; } = new (Expected.Failed);
        public static RichResult Error { get; } = new (Unexpected.Error);
        public static RichResult Impossible { get; } = new (Unexpected.Impossible);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator RichResult (Expected.Failure expected) => new (expected);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator RichResult (Exception unexpected) => new (unexpected);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RichResult FromFailure(Expected.Failure failure) => failure;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RichResult FromException(Exception exception) => exception;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public RichResult<TAnother> Attach<TAnother>(TAnother another)
        {
            if (_unexpected.Provided) return RichResult<TAnother>.FromException(_unexpected.Error);
            if (_expected.Provided) return RichResult<TAnother>.FromFailure(_expected.Failure);

            return RichResult<TAnother>.FromResult(another);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Match
        (
            Action success,
            Action<Expected.Failure> failure,
            Action<Exception> error
        ) {
            if (_unexpected.Provided)
            {
                error.Invoke(_unexpected.Error);
            }
            else if (_expected.Provided)
            {
                failure.Invoke(_expected.Failure);
            }
            else
            {
                success.Invoke();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TMatch Match<TMatch>
        (
            Func<TMatch> success,
            Func<Expected.Failure, TMatch> failure,
            Func<Exception, TMatch> error
        ) {
            if (_unexpected.Provided)
            {
                return error.Invoke(_unexpected.Error);
            }
            else if (_expected.Provided)
            {
                return failure.Invoke(_expected.Failure);
            }
            else
            {
                return success.Invoke();
            }
        }
    }
}
