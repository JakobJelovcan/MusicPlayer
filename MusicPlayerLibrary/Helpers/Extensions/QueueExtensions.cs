using System.Collections.Generic;

namespace MusicPlayerLibrary.Helpers.Extensions
{
    public static class QueueExtensions
    {
        public static void EnqueueRange<T>(this Queue<T> targetCollection, IEnumerable<T> sourceCollection)
        {
            sourceCollection.ForEach(T => targetCollection.Enqueue(T));
        }
    }
}
