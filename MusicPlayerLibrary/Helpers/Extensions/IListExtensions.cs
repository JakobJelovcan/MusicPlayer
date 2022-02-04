using System;
using System.Collections.Generic;
using System.Linq;

namespace MusicPlayerLibrary.Helpers.Extensions
{
    public static class IListExtensions
    {
        public static void AddInAscendingOrder<T>(this IList<T> target, T item, params Func<T, IComparable>[] selectors)
        {
            if (item is not T) throw new ArgumentNullException();
            if (target.Any())
            {
                int index = target.BinarySearch(item, selectors);
                if (index != -1)
                {
                    target.Insert(index, item);
                    return;
                }
            }
            target.Add(item);
        }

        public static void UpdateItemInAscendingOrder<T>(this IList<T> collection, T item, params Func<T, IComparable>[] selectors)
        {
            if (item is not T) throw new ArgumentNullException();
            if (collection.Any())
            {
                List<T> tempCollection = new List<T>(collection);
                tempCollection.Remove(item);
                int index = tempCollection.FindIndex(0, tempCollection.Count, I => item.CompareTo(I, selectors) <= 0);
                if (index >= 0)
                {
                    if (item.Equals(collection[index])) return;
                    collection.Remove(item);
                    collection.Insert(index, item);
                    return;
                }
            }
            collection.Add(item);
        }

        public static void AddRangeInAscendingOrder<T>(this IList<T> target, IEnumerable<T> source, params Func<T, IComparable>[] selectors)
        {
            foreach (T item in source) target.AddInAscendingOrder(item, selectors);
        }

        public static void AddInDescendingOrder<T>(this IList<T> target, T item, params Func<T, IComparable>[] selectors)
        {
            if (item is not T) throw new ArgumentNullException();
            if (target.Any())
            {
                int index = target.ReverseBinarySearch(item, selectors);
                if (index != -1)
                {
                    target.Insert(index, item);
                    return;
                }
            }
            target.Add(item);
        }

        public static void UpdateItemInDescendingOrder<T>(this IList<T> collection, T item, params Func<T, IComparable>[] selectors)
        {
            if (item is not T) throw new ArgumentNullException();
            if (collection.Any())
            {
                List<T> tempCollection = new List<T>(collection);
                tempCollection.Remove(item);
                int index = tempCollection.FindIndex(0, tempCollection.Count, I => item.CompareTo(I, selectors) >= 0);
                if (index >= 0)
                {
                    if (item.Equals(collection[index])) return;
                    collection.Remove(item);
                    collection.Insert(index, item);
                    return;
                }
            }
            collection.Add(item);
        }

        public static void AddRangeInDescendingOrder<T>(this IList<T> target, IEnumerable<T> source, params Func<T, IComparable>[] selectors)
        {
            foreach (T item in source) target.AddInDescendingOrder(item, selectors);
        }

        public static void AddIfDoesntContain<T>(this IList<T> collection, T item)
        {
            if (item is T && !collection.Contains(item)) collection.Add(item);
        }

        public static void AddIfDoesntContain<T, TParam>(this IList<T> collection, T item, Func<T, TParam> selector)
        {
            if (item is T && selector(item) != null && !collection.Any(I => selector(I).Equals(selector(item)))) collection.Add(item);
        }

        public static IList<T> Update<T>(this IList<T> targetCollection, IList<T> sourceCollection)
        {
            targetCollection.Where(T => !sourceCollection.Any(T1 => T1?.Equals(T) ?? false)).ToArray().ForEach(T => targetCollection.Remove(T));
            sourceCollection.Where(T => !targetCollection.Any(T1 => T1?.Equals(T) ?? false)).ToArray().ForEach(T => targetCollection.Add(T));
            return targetCollection;
        }

        public static IList<T> UpdateInAscendingOrder<T>(this IList<T> targetCollection, IList<T> sourceCollection, params Func<T, IComparable>[] selectors)
        {
            targetCollection.Where(T => !sourceCollection.Any(T1 => T1.Equals(T))).ToArray().ForEach(T => targetCollection.Remove(T));
            sourceCollection.Where(T => !targetCollection.Any(T1 => T1.Equals(T))).ToArray().ForEach(T => targetCollection.AddInAscendingOrder(T, selectors));
            return targetCollection;
        }

        public static IList<T> UpdateInDescendingOrder<T>(this IList<T> targetCollection, IList<T> sourceCollection, params Func<T, IComparable>[] selectors)
        {
            targetCollection.Where(T => !sourceCollection.Any(T1 => T1.Equals(T))).ToArray().ForEach(T => targetCollection.Remove(T));
            sourceCollection.Where(T => !targetCollection.Any(T1 => T1.Equals(T))).ToArray().ForEach(T => targetCollection.AddInDescendingOrder(T, selectors));
            return targetCollection;
        }

        public static void Remove<T>(this IList<T> collection, int startIndex)
        {
            if (startIndex < collection.Count) for (int i = startIndex; i < collection.Count; i++) collection.RemoveAt(i);
            else throw new IndexOutOfRangeException();
        }

        public static void Remove<T>(this IList<T> collection, int startIndex, int numOfItems)
        {
            if ((startIndex + numOfItems) < collection.Count) for (int i = startIndex; i < startIndex + numOfItems; i++) collection.RemoveAt(i);
            else throw new IndexOutOfRangeException();
        }

        public static void RemoveRange<T>(this IList<T> target, IEnumerable<T> source)
        {
            foreach (T item in source) target.Remove(item);
        }

        public static T GetPrevious<T>(this IList<T> collection, T item)
        {
            int index = collection.IndexOf(item);
            return index > 1 ? collection[index - 1] : default;
        }

        public static T GetNext<T>(this IList<T> collection, T item)
        {
            int index = collection.IndexOf(item);
            return index < collection.Count - 1 ? collection[index + 1] : default;
        }

        public static List<T> Randomize<T>(this IList<T> collection)
        {
            List<T> sourceCollection = collection.ToList();
            List<T> targetCollection = new List<T>(collection.Count);
            Random r = new Random();
            while (sourceCollection.Count != 0)
            {
                int index = r.Next(0, sourceCollection.Count);
                targetCollection.Add(sourceCollection[index]);
                sourceCollection.RemoveAt(index);
            }
            return targetCollection;
        }

        public static int BinarySearch<T>(this IList<T> collection, T item, params Func<T, IComparable>[] selectors)
        {
            if (collection.Any() && selectors.Any())
            {
                int start = 0;
                int end = collection.Count - 1;
                while (start <= end)
                {
                    int middle = start + (end - start) / 2;
                    int result = item.CompareTo(collection[middle], selectors);
                    if (result == 0) return middle;
                    else if (result < 0) end = middle - 1;
                    else start = middle + 1;
                }
                return start;
            }
            throw new ArgumentNullException();
        }

        public static int ReverseBinarySearch<T>(this IList<T> collection, T item, params Func<T, IComparable>[] selectors)
        {
            if (collection.Any() && selectors.Any())
            {
                int start = 0;
                int end = collection.Count - 1;
                while (start <= end)
                {
                    int middle = start + (end - start) / 2;
                    int result = item.CompareTo(collection[middle], selectors);
                    if (result == 0) return middle;
                    else if (result > 0) end = middle - 1;
                    else start = middle + 1;
                }
                return start;
            }
            throw new ArgumentNullException();
        }

        public static void AddRange<T>(this IList<T> targetCollection, IEnumerable<T> sourceCollection)
        {
            sourceCollection.ForEach(T => targetCollection.Add(T));
        }
    }
}