using System;
using System.Runtime.CompilerServices;

namespace Calc.Domain.Functional
{
    public readonly struct Result<TValue>
    {
        private readonly Income _kind;

        private readonly TValue _income;
        private readonly Exception _exception;

        private Result(TValue value)
        {
            _kind = Income.Value;

            _exception = default!;
            _income = value;
        }

        private Result(Exception exception)
        {
            _kind = Income.Exception;

            _income = default!;
            _exception = exception;
        }

        public static Result<TValue> Error { get; } = new (Outcome.Unexpected.Error);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Result<TValue> (TValue value) => new (value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Result<TValue> (Exception exception) => new (exception);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Result<TValue> FromResult(TValue value) => value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Result<TValue> FromException(Exception exception) => exception;

        public Result<TValue, TAnother> Attach<TAnother>(TAnother another)
        {
            return _kind switch
            {
                Income.None => Result<TValue, TAnother>.Error,
                Income.Value => Result<TValue, TAnother>.FromResult(_income, another),
                Income.Exception => Result<TValue, TAnother>.FromException(_exception),
                _ => Result<TValue, TAnother>.Error
            };
        }

        public Result<TValue, TAnother> Combine<TAnother>(Result<TAnother> another)
        {
            return _kind switch
            {
                Income.None => Result<TValue, TAnother>.Error,
                Income.Value when another._kind is Income.Value => Result<TValue, TAnother>.FromResult(_income, another._income),
                Income.Exception => Result<TValue, TAnother>.FromException(_exception),
                _ => Result<TValue, TAnother>.Error
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Run(Action<TValue> run)
        {
            if (_kind == Income.Value)
            {
                run.Invoke(_income);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Result Run(Func<TValue, Result> run)
        {
            return _kind switch
            {
                Income.None => Result.Error,
                Income.Value => run.Invoke(_income),
                Income.Exception => Result.FromException(_exception),
                _ => Result.Error
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Result<TAnother> Run<TAnother>(Func<TValue, Result<TAnother>> run)
        {
            return _kind switch
            {
                Income.None => Result<TAnother>.Error,
                Income.Value => run.Invoke(_income),
                Income.Exception => Result<TAnother>.FromException(_exception),
                _ => Result<TAnother>.Error
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Match(Action<TValue> success, Action<Exception> error)
        {
            if (_kind is Income.Value)
            {
                success.Invoke(_income);
            }
            else if (_kind is Income.Exception)
            {
                error.Invoke(_exception);
            }
            else
            {
                error.Invoke(Outcome.Unexpected.Impossible);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TMatch Match<TMatch>(Func<TValue, TMatch> success, Func<Exception, TMatch> error)
        {
            return _kind switch
            {
                Income.Value => success.Invoke(_income),
                Income.Exception => error.Invoke(_exception),
                _ => error.Invoke(Outcome.Unexpected.Impossible)
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TValue Match(Func<TValue, TValue> success, Func<Exception, TValue> error)
        {
            return _kind switch
            {
                Income.Value => success.Invoke(_income),
                Income.Exception => error.Invoke(_exception),
                _ => error.Invoke(Outcome.Unexpected.Impossible)
            };
        }

        private enum Income
        {
            None = 0,
            Value = 1,
            Exception = -1
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

        public static Result<TFirst, TSecond> Error { get; } = new (Outcome.Unexpected.Error);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Result<TFirst, TSecond> ((TFirst First, TSecond Second) income) => new (income.First, income.Second);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Result<TFirst, TSecond> (Exception exception) => new (exception);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Result<TFirst, TSecond> FromResult(TFirst first, TSecond value) => (first, value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Result<TFirst, TSecond> FromException(Exception exception) => exception;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Run(Action<TFirst, TSecond> action)
        {
            if (_income.Provided)
            {
                action.Invoke(_income.First, _income.Second);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Result<TAnother> Run<TAnother>(Func<TFirst, TSecond, Result<TAnother>> action)
        {
            if (_income.Provided)
            {
                return action.Invoke(_income.First, _income.Second);
            }
            else
            {
                return Result<TAnother>.Error;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Match(Action<TFirst, TSecond> success, Action<Exception> error)
        {
            if (_income.Provided)
            {
                success.Invoke(_income.First, _income.Second);
            }
            else
            {
                error.Invoke(_exception.Value);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TMatch Match<TMatch>(Func<TFirst, TSecond, TMatch> success, Func<Exception, TMatch> error)
        {
            return _income.Provided
                ? success.Invoke(_income.First, _income.Second)
                : error.Invoke(_exception.Value);
        }
    }
}
