using System;
using System.Collections.Generic;
using System.Text;

namespace ExtensionsLibrary.Extensions
{
    public static class ArraySegmentExtensions
    {
        public static T[] GetArraySegment<T>(this ArraySegment<T> arraySegment)
        {
            T[] buffer = new T[arraySegment.Count];
            Array.Copy(arraySegment.Array, arraySegment.Offset, buffer, 0, arraySegment.Count);
            return buffer;
        }

        public static IEnumerable<string> ToTerminatedStringCollection(this ArraySegment<byte> arraySegment, Encoding encoding)
        {
            int position = 0;
            char[] trimCollection = new char[] { '\0', '\uFFFE', '\uFEFF', '\u200B' };
            while (position < arraySegment.Count) yield return encoding.GetNullTerminatedString(arraySegment, ref position).Trim(trimCollection);
        }
    }
}