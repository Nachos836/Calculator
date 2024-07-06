using System;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;

namespace Calc.Domain.Functional
{
    public readonly struct Result<TValue>
    {
        private readonly (bool Provided, TValue Value) _income;
        private readonly (bool Provided, Exception Value) _exception;

        private Result(TValue value)
        {
            _income = (Provided: true, value);
            _exception = default;
        }

        private Result(Exception exception)
        {
            _income = default;
            _exception = (Provided: true, exception);
        }

        [Pure] public static Result<TValue> Error { get; } = new (Outcome.Unexpected.Error);
        [Pure] public static Result<TValue> Impossible { get; } = new (Outcome.Unexpected.Impossible);

        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Result<TValue> (TValue value) => new (value);

        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Result<TValue> (Exception exception) => new (exception);

        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Result<TValue> FromResult(TValue value) => value;
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Result<TValue> FromException(Exception exception) => exception;

        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Result<TValue, TAnother> Attach<TAnother>(TAnother another)
        {
            return this switch
            {
                _ when _income.Provided => Result<TValue, TAnother>.FromResult(_income.Value, another),
                _ when _exception.Provided => Result<TValue, TAnother>.FromException(_exception.Value),
                _ => Result<TValue, TAnother>.Impossible
            };
        }

        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Result<TValue, TAnother> Attach<TAnother>(Func<TValue, Result<TAnother>> merge)
        {
            return this switch
            {
                _ when _income.Provided => Combine(merge.Invoke(_income.Value)),
                _ when _exception.Provided => Result<TValue, TAnother>.FromException(_exception.Value),
                _ => Result<TValue, TAnother>.Impossible
            };
        }

        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Result<TValue, TAnother> Combine<TAnother>(Result<TAnother> another)
        {
            return this switch
            {
                _ when _income.Provided && another._income.Provided => Result<TValue, TAnother>.FromResult(_income.Value, another._income.Value),
                _ when _exception.Provided && another._exception.Provided => Result<TValue, TAnother>.FromException(new AggregateException(_exception.Value, another._exception.Value)),
                _ when _exception.Provided => Result<TValue, TAnother>.FromException(_exception.Value),
                _ when another._exception.Provided => Result<TValue, TAnother>.FromException(another._exception.Value),
                _ => Result<TValue, TAnother>.Impossible
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Run(Action<TValue> run)
        {
            if (_income.Provided)
            {
                run.Invoke(_income.Value);
            }
        }

        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Result Run(Func<TValue, Result> run)
        {
            return this switch
            {
                _ when _income.Provided => run.Invoke(_income.Value),
                _ when _exception.Provided => Result.FromException(_exception.Value),
                _ => Result.Impossible
            };
        }

        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Result<TAnother> Run<TAnother>(Func<TValue, Result<TAnother>> run)
        {
            return this switch
            {
                _ when _income.Provided => run.Invoke(_income.Value),
                _ when _exception.Provided => Result<TAnother>.FromException(_exception.Value),
                _ => Result<TAnother>.Impossible
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Match(Action<TValue> success, Action<Exception> error)
        {
            if (_income.Provided)
            {
                success.Invoke(_income.Value);
            }
            else if (_exception.Provided)
            {
                error.Invoke(_exception.Value);
            }
            else
            {
                error.Invoke(Outcome.Unexpected.Impossible);
            }
        }

        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TMatch Match<TMatch>(Func<TValue, TMatch> success, Func<Exception, TMatch> error)
        {
            return this switch
            {
                _ when _income.Provided => success.Invoke(_income.Value),
                _ when _exception.Provided => error.Invoke(_exception.Value),
                _ => error.Invoke(Outcome.Unexpected.Impossible)
            };
        }

        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TValue Match(Func<TValue, TValue> success, Func<Exception, TValue> error)
        {
            return this switch
            {
                _ when _income.Provided => success.Invoke(_income.Value),
                _ when _exception.Provided => error.Invoke(_exception.Value),
                _ => error.Invoke(Outcome.Unexpected.Impossible)
            };
        }

        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string ToString()
        {
            return this switch
            {
                _ when _income.Provided => _income.Value!.ToString(),
                _ when _exception.Provided => _exception.Value.Message,
                _ => Outcome.Unexpected.Impossible.Message
            };
        }
    }

    public readonly struct Result<TFirst, TSecond>
    {
        private readonly (bool Provided, TFirst First, TSecond Second) _income;
        private readonly (bool Provided, Exception Value) _exception;

        private Result(TFirst first, TSecond second)
        {
            _income = (Provided: true, first, second);
            _exception = default;
        }

        private Result(Exception exception)
        {
            _income = default;
            _exception = (Provided: true, exception);
        }

        [Pure] public static Result<TFirst, TSecond> Error { get; } = new (Outcome.Unexpected.Error);
        [Pure] public static Result<TFirst, TSecond> Impossible { get; } = new (Outcome.Unexpected.Impossible);

        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Result<TFirst, TSecond> ((TFirst First, TSecond Second) income) => new (income.First, income.Second);

        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Result<TFirst, TSecond> (Exception exception) => new (exception);

        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Result<TFirst, TSecond> FromResult(TFirst first, TSecond value) => (first, value);

        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Result<TFirst, TSecond> FromException(Exception exception) => exception;

        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Result<TFirst> Reduce(Func<TFirst, TSecond, TFirst> reducer)
        {
            return this switch
            {
                _ when _income.Provided => reducer.Invoke(_income.First, _income.Second),
                _ when _exception.Provided => Result<TFirst>.FromException(_exception.Value),
                _ => Result<TFirst>.Impossible
            };
        }

        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Result<TSecond> Reduce(Func<TFirst, TSecond, TSecond> reducer)
        {
            return this switch
            {
                _ when _income.Provided => reducer.Invoke(_income.First, _income.Second),
                _ when _exception.Provided => Result<TSecond>.FromException(_exception.Value),
                _ => Result<TSecond>.Impossible
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Run(Action<TFirst, TSecond> action)
        {
            if (_income.Provided)
            {
                action.Invoke(_income.First, _income.Second);
            }
        }

        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Result<TAnother> Run<TAnother>(Func<TFirst, TSecond, Result<TAnother>> action)
        {
            return this switch
            {
                _ when _income.Provided => action.Invoke(_income.First, _income.Second),
                _ when _exception.Provided => Result<TAnother>.FromException(_exception.Value),
                _ => Result<TAnother>.Impossible
            };
        }

        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Result<TAnother, TAnotherOne> Run<TAnother, TAnotherOne>(Func<TFirst, TSecond, Result<TAnother, TAnotherOne>> action)
        {
            return this switch
            {
                _ when _income.Provided => action.Invoke(_income.First, _income.Second),
                _ when _exception.Provided => Result<TAnother, TAnotherOne>.FromException(_exception.Value),
                _ => Result<TAnother, TAnotherOne>.Impossible
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Match(Action<TFirst, TSecond> success, Action<Exception> error)
        {
            if (_income.Provided)
            {
                success.Invoke(_income.First, _income.Second);
            }
            else if (_exception.Provided)
            {
                error.Invoke(_exception.Value);
            }
            else
            {
                error.Invoke(Outcome.Unexpected.Impossible);
            }
        }

        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TMatch Match<TMatch>(Func<TFirst, TSecond, TMatch> success, Func<Exception, TMatch> error)
        {
            return this switch
            {
                _ when _income.Provided => success.Invoke(_income.First, _income.Second),
                _ when _exception.Provided => error.Invoke(_exception.Value),
                _ => error.Invoke(Outcome.Unexpected.Impossible)
            };
        }

        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string ToString()
        {
            return this switch
            {
                _ when _income.Provided => $"{ _income.First } { _income.Second }",
                _ when _exception.Provided => _exception.Value.Message,
                _ => Outcome.Unexpected.Impossible.Message
            };
        }
    }
}
