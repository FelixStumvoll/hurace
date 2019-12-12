namespace Hurace.Core.Api.Util
{
    public class Result<TVal, TError>
    {
        public bool Success => Value != null;
        public TVal Value { get; private set; }
        public TError Error { get; private set; }

        public static Result<TVal,TError> Err(TError error) =>
            new Result<TVal, TError>
            {
                Error = error
            };

        public static Result<TVal, TError> Ok(TVal value) =>
            new Result<TVal, TError>
            {
                Value = value
            };
    }
}