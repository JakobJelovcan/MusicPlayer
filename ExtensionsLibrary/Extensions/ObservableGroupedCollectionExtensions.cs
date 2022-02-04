using Microsoft.Toolkit.Collections;
using System.Collections.Generic;

namespace ExtensionsLibrary.Extensions
{
    public static class ObservableGroupedCollectionExtensions
    {
        public static IEnumerable<TKey> GetKeys<TKey, TValue>(this ObservableGroupedCollection<TKey, TValue> collection)
        {
            foreach (ObservableGroup<TKey, TValue> item in collection) yield return item.Key;
        }
    }
}
