using System;
using System.Collections.Generic;
using System.Linq;

namespace MusicPlayerLibrary.Helpers.Extensions
{
    public static class TimespanExtensions
    {
        public static TimeSpan Sum<Tsource>(this IEnumerable<Tsource> source, Func<Tsource, TimeSpan> selector)
        {
            return source.Select(selector).Aggregate(TimeSpan.Zero, (ts1, ts2) => ts1 + ts2);
        }

        public static string ToFormatedString(this TimeSpan ts)
        {
            return ts.ToString(ts.Hours > 0 ? @"h\:mm\:ss" : @"m\:ss");
        }

        public static string ToInfoString(this TimeSpan ts)
        {
            if (ts.Hours > 0) return $"{ts.Hours} hr {ts.Minutes} min";
            return $"{ts.Minutes} min";
        }
    }
}
