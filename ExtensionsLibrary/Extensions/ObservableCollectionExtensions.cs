using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace ExtensionsLibrary.Extensions
{
    public static class ObservableCollectionExtensions
    {
        public static void AddRange<T>(this ObservableCollection<T> targetCollection, IEnumerable<T> sourceCollection)
        {
            foreach (T item in sourceCollection) targetCollection.Add(item);
        }

        public static void UpdateItemInAscendingOrder<T>(this ObservableCollection<T> collection, T item, params Func<T, IComparable>[] selectors)
        {
            if (item is not T) throw new ArgumentNullException();
            if (collection.Any())
            {
                List<T> tempCollection = new List<T>(collection);
                tempCollection.Remove(item);
                int index = tempCollection.FindIndex(0, tempCollection.Count, I => item.CompareTo(I, selectors) <= 0);
                if (index >= 0) collection.Move(collection.IndexOf(item), index);
            }
            else collection.Add(item);
        }

        public static void UpdateItemInDescendingOrder<T>(this ObservableCollection<T> collection, T item, params Func<T, IComparable>[] selectors)
        {
            if (item is not T) throw new ArgumentNullException();
            if (collection.Any())
            {
                List<T> tempCollection = new List<T>(collection);
                tempCollection.Remove(item);
                int index = tempCollection.FindIndex(0, tempCollection.Count, I => item.CompareTo(I, selectors) >= 0);
                if (index >= 0) collection.Move(collection.IndexOf(item), index);
            }
            else collection.Add(item);
        }

        public static void AddRangeInAscendingOrder<TSource>(this ObservableCollection<TSource> targetCollection, IEnumerable<TSource> sourceCollection, Func<TSource, IComparable> selector)
        {
            foreach (TSource item in sourceCollection) targetCollection.AddInAscendingOrder(item, selector);
        }

        public static void AddRangeInDescendingOrder<TSource>(this ObservableCollection<TSource> targetCollection, IEnumerable<TSource> sourceCollection, Func<TSource, IComparable> selector)
        {
            foreach (TSource item in sourceCollection) targetCollection.AddInDescendingOrder(item, selector);
        }

        public static void Sort<T>(this ObservableCollection<T> collection, Func<T, IComparable> comparer1, Func<T, IComparable> comparer2 = null)
        {
            List<T> orderedCollection = (comparer2 is null) ? collection.OrderBy(comparer1).ToList() : collection.OrderBy(comparer1).ThenBy(comparer2).ToList();
            for (int i = 0; i < orderedCollection.Count; ++i) collection.Move(collection.IndexOf(orderedCollection[i]), i);
        }

        public static void SortDescending<T>(this ObservableCollection<T> collection, Func<T, IComparable> comparer1, Func<T, IComparable> comparer2 = null)
        {
            List<T> orderedCollection = (comparer2 is null) ? collection.OrderByDescending(comparer1).ToList() : collection.OrderByDescending(comparer1).ThenByDescending(comparer2).ToList();
            for (int i = 0; i < orderedCollection.Count; ++i) collection.Move(collection.IndexOf(orderedCollection[i]), i);
        }
    }
}
