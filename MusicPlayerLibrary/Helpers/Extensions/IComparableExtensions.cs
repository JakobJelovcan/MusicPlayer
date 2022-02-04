using System;
using System.Linq;

namespace MusicPlayerLibrary.Helpers.Extensions
{
    public static class IComparableExtensions
    {
        public static int CompareTo<TSource>(this TSource item1, TSource item2, params Func<TSource, IComparable>[] selectors)
        {
            if (selectors.Any()) return (selectors[0](item1).CompareTo(selectors[0](item2)) is int value && value != 0) ? value : item1.CompareTo(item2, selectors.Skip(1).ToArray());
            return 0;
        }
    }
}
