using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ExtensionsLibrary.Extensions
{
    public static class TaskExtensions
    {
        public static async void FireAndForget<T>(this Task<T> task)
        {
            try
            {
                await task.ConfigureAwait(false);
            }
            catch (Exception ex)
            {
#if DEBUG
                Debug.WriteLine(ex.Message);
#endif
            }
        }

        public static async void FireAndForget(this Task task)
        {
            try
            {
                await task.ConfigureAwait(false);
            }
            catch (Exception ex)
            {
#if DEBUG
                Debug.WriteLine(ex.Message);
#endif
            }
        }
    }
}