using System;
using System.Text;

namespace MusicMetaDataLibrary.ID3v2.Extensions
{
    public static class EncodingExtensions
    {
        public static byte ToByte(this Encoding encoding)
        {
            switch (encoding.CodePage)
            {
                case 20127: return 0x00;
                case 1200: return 0x01;
                case 1201: return 0x02;
                case 65001: return 0x03;
                default: throw new ArgumentException();
            }
        }

        public static string GetBOM(this Encoding encoding)
        {
            switch (encoding.CodePage)
            {
                case 20127:
                case 65001:
                    return string.Empty;
                case 1200: return "\uFEFF";
                case 1201: return string.Empty;
                default: throw new ArgumentException();
            }
        }

        public static string GetTerminator(this Encoding encoding)
        {
            switch (encoding.CodePage)
            {
                case 20127:
                case 65001:
                    return "\0";
                case 1200:
                case 1201:
                    return "\0\0";
                default: throw new ArgumentException();
            }
        }
    }
}
