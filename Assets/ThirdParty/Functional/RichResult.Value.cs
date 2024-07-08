using System;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;

namespace ThirdParty.Functional
{
    using Outcome;

    public readonly struct RichResult<TValue>
    {
        private readonly (TValue Value, bool Provided) _result;
        private readonly (Exception Error, bool Provided) _unexpected;
        private readonly (Expected.Failure Failure, bool Provided) _expected;

        private RichResult(TValue value)
        {
            _result = (value, Provided: true);
            _expected = default;
            _unexpected = default;
        }

        private RichResult(Expected.Failure expected)
        {
            _result = default;
            _expected = (expected, Provided: true);
            _unexpected = default;
        }

        private RichResult(Exception unexpected)
        {
            _result = default;
            _expected = default;
            _unexpected = (unexpected, Provided: true);
        }

        public static RichResult<TValue> Failure { get; } = new (Expected.Failed);
        public static RichResult<TValue> Error { get; } = new (Unexpected.Error);
        public static RichResult<TValue> Impossible { get; } = new (Unexpected.Impossible);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator RichResult<TValue> (TValue value) => new (value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator RichResult<TValue> (Expected.Failure expected) => new (expected);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator RichResult<TValue> (Exception unexpected) => new (unexpected);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RichResult<TValue> FromResult(TValue value) => value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RichResult<TValue> FromFailure(Expected.Failure failure) => failure;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RichResult<TValue> FromException(Exception exception) => exception;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public RichResult<TValue, TAnother> Attach<TAnother>(TAnother another)
        {
            if (_result.Provided) return RichResult<TValue, TAnother>.FromResult(_result.Value, another);
            if (_expected.Provided) return RichResult<TValue, TAnother>.FromFailure(_expected.Failure);

            return RichResult<TValue, TAnother>.FromException(_unexpected.Error);
        }

        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public RichResult<TValue, TAnother> Combine<TAnother>(RichResult<TAnother> another)
        {
            return this switch
            {
                _ when _unexpected.Provided && another._unexpected.Provided => RichResult<TValue, TAnother>.FromException(new AggregateException(_unexpected.Error, another._unexpected.Error)),
                _ when _unexpected.Provided => RichResult<TValue, TAnother>.FromException(_unexpected.Error),
                _ when another._unexpected.Provided => RichResult<TValue, TAnother>.FromException(new AggregateException(another._unexpected.Error)),
                _ when _expected.Provided && another._expected.Provided => RichResult<TValue, TAnother>.FromFailure(new Expected.Failure(_expected.Failure.Message + " and " + another._expected.Failure.Message)),
                _ when _expected.Provided => RichResult<TValue, TAnother>.FromFailure(_expected.Failure),
                _ when another._expected.Provided => RichResult<TValue, TAnother>.FromFailure(another._expected.Failure),
                _ when _result.Provided => RichResult<TValue, TAnother>.FromResult(_result.Value, another._result.Value),
                _ => RichResult<TValue, TAnother>.FromException(Unexpected.Impossible)
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Match
        (
            Action<TValue> success,
            Action<Expected.Failure> failure,
            Action<Exception> error
        ) {
            if (_result.Provided)
            {
                success.Invoke(_result.Value);
            }
            else if (_expected.Provided)
            {
                failure.Invoke(_expected.Failure);
            }
            else if (_unexpected.Provided)
            {
                error.Invoke(_unexpected.Error);
            }
            else
            {
                error.Invoke(Unexpected.Impossible);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TMatch Match<TMatch>
        (
            Func<TValue, TMatch> success,
            Func<Expected.Failure, TMatch> failure,
            Func<Exception, TMatch> error
        ) {
            return _result.Provided
                ? success.Invoke(_result.Value)
                : _expected.Provided
                    ? failure.Invoke(_expected.Failure)
                    : error.Invoke(_unexpected.Error);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TValue Match
        (
            Func<TValue, TValue> success,
            Func<Expected.Failure, TValue> failure,
            Func<Exception, TValue> error
        ) {
            return _result.Provided
                ? success.Invoke(_result.Value)
                :_expected.Provided
                    ? failure.Invoke(_expected.Failure)
                    : error.Invoke(_unexpected.Error);
        }
    }

    public readonly struct RichResult<TFirst, TSecond>
    {
        private readonly (TFirst First, TSecond Second, bool Provided) _result;
        private readonly (Exception Error, bool Provided) _unexpected;
        private readonly (Expected.Failure Failure, bool Provided) _expected;

        private RichResult(TFirst first, TSecond second)
        {
            _result = (first, second, Provided: true);
            _expected = default;
            _unexpected = default;
        }

        private RichResult(Expected.Failure expected)
        {
            _result = default;
            _expected = (expected, Provided: true);
            _unexpected = default;
        }

        private RichResult(Exception unexpected)
        {
            _result = default;
            _expected = default;
            _unexpected = (unexpected, Provided: true);
        }

        public static RichResult<TFirst, TSecond> Failure { get; } = new (Expected.Failed);
        public static RichResult<TFirst, TSecond> Error { get; } = new (Unexpected.Error);
        public static RichResult<TFirst, TSecond> Impossible { get; } = new (Unexpected.Impossible);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator RichResult<TFirst, TSecond> ((TFirst First, TSecond Second) income) => new (income.First, income.Second);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator RichResult<TFirst, TSecond> (Expected.Failure expected) => new (expected);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator RichResult<TFirst, TSecond> (Exception unexpected) => new (unexpected);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RichResult<TFirst, TSecond> FromResult(TFirst first, TSecond second) => (first, second);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RichResult<TFirst, TSecond> FromFailure(Expected.Failure failure) => failure;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RichResult<TFirst, TSecond>FromException(Exception exception) => exception;

        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public RichResult<TAnother> Run<TAnother>(Func<TFirst, TSecond, RichResult<TAnother>> func)
        {
            if (_result.Provided)
            {
                return func.Invoke(_result.First, _result.Second);
            }
            else if (_expected.Provided)
            {
                return RichResult<TAnother>.FromFailure(_expected.Failure);
            }
            else if (_unexpected.Provided)
            {
                return RichResult<TAnother>.FromException(_unexpected.Error);
            }
            else
            {
                return RichResult<TAnother>.FromException(Unexpected.Impossible);
            }
        }

        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public RichResult<TAnother, TAnotherOne> Run<TAnother, TAnotherOne>(Func<TFirst, TSecond, RichResult<TAnother, TAnotherOne>> func)
        {
            if (_result.Provided)
            {
                return func.Invoke(_result.First, _result.Second);
            }
            else if (_expected.Provided)
            {
                return RichResult<TAnother, TAnotherOne>.FromFailure(_expected.Failure);
            }
            else if (_unexpected.Provided)
            {
                return RichResult<TAnother, TAnotherOne>.FromException(_unexpected.Error);
            }
            else
            {
                return RichResult<TAnother, TAnotherOne>.FromException(Unexpected.Impossible);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Match
        (
            Action<TFirst, TSecond> success,
            Action<Expected.Failure> failure,
            Action<Exception> error
        ) {
            if (_result.Provided)
            {
                success.Invoke(_result.First, _result.Second);
            }
            else if (_expected.Provided)
            {
                failure.Invoke(_expected.Failure);
            }
            else if (_unexpected.Provided)
            {
                error.Invoke(_unexpected.Error);
            }
            else
            {
                error.Invoke(Unexpected.Impossible);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TMatch Match<TMatch>
        (
            Func<TFirst, TSecond, TMatch> success,
            Func<Expected.Failure, TMatch> failure,
            Func<Exception, TMatch> error
        ) {
            return _result.Provided
                ? success.Invoke(_result.First, _result.Second)
                : _expected.Provided
                    ? failure.Invoke(_expected.Failure)
                    : error.Invoke(_unexpected.Error);
        }
    }
}
