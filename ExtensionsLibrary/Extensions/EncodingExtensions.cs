using System;
using System.Text;

namespace ExtensionsLibrary.Extensions
{
    public static class EncodingExtensions
    {
        public static byte[] GetTerminator(this Encoding encoding)
        {
            switch (encoding.CodePage)
            {
                case 20127:
                case 65001:
                    return new byte[] { 0x00 };
                case 1200:
                case 1201:
                    return new byte[] { 0x00, 0x00 };
                default: throw new ArgumentException();
            }
        }

        public static string GetNullTerminatedString(this Encoding encoding, ArraySegment<byte> buffer, ref int startIndex)
        {
            byte[] nullTerminator = encoding.GetTerminator();
            if (startIndex + nullTerminator.Length > buffer.Count) throw new IndexOutOfRangeException();
            int absolteStartIndex = buffer.Offset + startIndex;
            int endIndex = buffer.Array.IndexOf(nullTerminator, absolteStartIndex, buffer.Count - startIndex);
            if (endIndex < 0 || endIndex == startIndex)
            {
                ++startIndex;
                return null;
            }
            int count = endIndex - absolteStartIndex + nullTerminator.Length;
            startIndex += count;
            return encoding.GetString(buffer.Array, absolteStartIndex, count - nullTerminator.Length);
        }
    }
}