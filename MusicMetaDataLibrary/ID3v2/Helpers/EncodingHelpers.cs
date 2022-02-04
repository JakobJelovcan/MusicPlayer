using System;
using System.Text;

namespace MusicMetaDataLibrary.ID3v2.Helpers
{
    public class EncodingHelpers
    {
        public static Encoding GetEncoding(byte value)
        {
            switch (value)
            {
                case 0x00: return Encoding.ASCII;
                case 0x01: return Encoding.Unicode;
                case 0x02: return Encoding.BigEndianUnicode;
                case 0x03: return Encoding.UTF8;
                default: throw new ArgumentException();
            }
        }
    }
}
