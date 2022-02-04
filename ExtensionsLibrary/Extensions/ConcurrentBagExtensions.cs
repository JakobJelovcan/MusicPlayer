using System.Collections.Concurrent;

namespace ExtensionsLibrary.Extensions
{
    public static class ConcurrentBagExtensions
    {
        public static T AddAndReturn<T>(this ConcurrentBag<T> bag, T item)
        {
            bag.Add(item);
            return item;
        }
    }
}
