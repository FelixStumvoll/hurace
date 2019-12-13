using System;
using System.Threading.Tasks;

namespace Hurace.Core.Api.Util
{
    public class Result<TVal, TErr>
    {
        public TVal Value { get; private set; }
        public bool Failure => Error != null;
        public TErr Error { get; protected set; }

        public new static Result<TVal, TErr> Err(TErr error) =>
            new Result<TVal, TErr>
            {
                Error = error
            };

        public static Result<TVal, TErr> Ok(TVal value) =>
            new Result<TVal, TErr>
            {
                Value = value
            };

        public Result<TVal, TErr> AndThen(Action<TVal> action)
        {
            if(!Failure) action.Invoke(Value);
            return this;
        }
        
        public Result<TVal, TErr> OrElse(Action<TErr> action)
        {
            if(Failure) action?.Invoke(Error);
            return this;
        }

        public Result<T, TErr> Map<T>(Func<TVal, T> map) => 
            Failure ? Result<T, TErr>.Err(Error) : Result<T,TErr>.Ok(map.Invoke(Value));
        
        public Result<T, TErr> And<T>(Result<T,TErr> input, Action<T> func = null )
        {
            if (Failure) return Result<T, TErr>.Err(input.Error);
            func?.Invoke(input.Value);
            return input;
        }
        
        
    }
}