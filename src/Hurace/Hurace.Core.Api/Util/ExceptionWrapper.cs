using System;
using System.Threading.Tasks;

namespace Hurace.Core.Api.Util
{
    public static class ExceptionWrapper
    {
        public static async Task<Result<T, Exception>> Try<T>(Func<Task<T>> func) where T : class
        {
            try
            {
                return Result<T, Exception>.Ok(await func());
            }
            catch (Exception e)
            {
                return Result<T, Exception>.Err(e);
            }
        }
        
        public static async Task<Result<T?,Exception>> TryStruct<T>(Func<Task<T?>> func) where T : struct
        {
            try
            {
                return Result<T?, Exception>.Ok(await func());
            }
            catch (Exception e)
            {
                return Result<T?, Exception>.Err(e);
            }
        }

        public static async Task<Result<bool,Exception>> Try(Func<Task<bool>> func)
        {
            try
            {
                if(!await func()) return Result<bool,Exception>.Err(null);
                return Result<bool,Exception>.Ok(true);
            }
            catch (Exception e)
            {
                return Result<bool,Exception>.Err(e);
            }
        }

        public static async Task<Result<int, Exception>> Try(Func<Task<int>> func)
        {
            try
            {
                return Result<int,Exception>.Ok(await func());
            }
            catch (Exception e)
            {
                return Result<int, Exception>.Err(e);
            }
        }
    }
}