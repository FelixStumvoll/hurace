using System;
using System.Threading.Tasks;

namespace Hurace.Core.Api
{
    public static class ExceptionWrapper
    {
        public static async Task<T?> Try<T>(Func<Task<T?>> func) where T : class
        {
            try
            {
                return await func();
            }
            catch (Exception)
            {
                return null;
            }
        }
        
        public static async Task<T?> TryStruct<T>(Func<Task<T?>> func) where T : struct
        {
            try
            {
                return await func();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static async Task<bool> Try(Func<Task<bool>> func)
        {
            try
            {
                return await func();
            }
            catch (Exception)
            {
                return false;
            }
        }
        
        public static bool Try(Func<bool> func)
        {
            try
            {
                return func();
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static async Task<int?> Try(Func<Task<int>> func)
        {
            try
            {
                return await func();
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}