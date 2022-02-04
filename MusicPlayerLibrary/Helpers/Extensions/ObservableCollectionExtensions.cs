using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MusicPlayerLibrary.Helpers.Extensions
{
    public static class ObservableCollectionExtensions
    {
        public static void AddRange<T>(this ObservableCollection<T> targetCollection, IEnumerable<T> sourceCollection)
        {
            foreach (T item in sourceCollection) targetCollection.Add(item);
        }

        public static void AddRangeInAscendingOrder<TSource>(this ObservableCollection<TSource> targetCollection, IEnumerable<TSource> sourceCollection, Func<TSource, IComparable> selector)
        {
            foreach (TSource item in sourceCollection) targetCollection.AddInAscendingOrder(item, selector);
        }

        public static void AddRangeInDescendingOrder<TSource>(this ObservableCollection<TSource> targetCollection, IEnumerable<TSource> sourceCollection, Func<TSource, IComparable> selector)
        {
            foreach (TSource item in sourceCollection) targetCollection.AddInDescendingOrder(item, selector);
        }
    }
}
