using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Numerics;

namespace ExtensionsLibrary.Extensions
{
    public static class IEnumerableExtensions
    {
        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> collection)
        {
            return new ObservableCollection<T>(collection);
        }

        public static IEnumerable<T> Concat<T>(this IEnumerable<IEnumerable<T>> collection)
        {
            foreach (IEnumerable<T> innercollection in collection) foreach (T item in innercollection) yield return item;
        }

        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> collection, Func<TSource, TKey> selector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource item in collection) if (seenKeys.Add(selector(item))) yield return item;
        }

        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> collection, Action<T> action)
        {
            foreach (T item in collection) action(item);
            return collection;
        }

        public static Vector3 Average(this IEnumerable<Vector3> vectorList)
        {
            float x = default;
            float y = default;
            float z = default;
            int count = vectorList.Count();
            foreach (Vector3 vector in vectorList)
            {
                x += vector.X;
                y += vector.Y;
                z += vector.Z;
            }
            x /= count;
            y /= count;
            z /= count;
            return new Vector3(x, y, z);
        }

        public static T LastOr<T>(this IEnumerable<T> collection, T defaultValue)
        {
            return collection.LastOrDefault() ?? defaultValue;
        }

        public static T LastOr<T>(this IEnumerable<T> collection, Func<T, bool> selector, T defaultValue)
        {
            return collection.LastOrDefault(selector) ?? defaultValue;
        }

        public static T FirstOr<T>(this IEnumerable<T> collection, T defaultValue)
        {
            return collection.FirstOrDefault() ?? defaultValue;
        }

        public static T FirstOr<T>(this IEnumerable<T> collection, Func<T, bool> selector, T defaultValue)
        {
            return collection.FirstOrDefault(selector) ?? defaultValue;
        }
    }
}