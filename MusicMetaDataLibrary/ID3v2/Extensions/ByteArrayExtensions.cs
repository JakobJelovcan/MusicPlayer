using ExtensionsLibrary.Exceptions;
using System;

namespace MusicMetaDataLibrary.ID3v2.Extensions
{
    public static class ByteArrayExtensions
    {
        public static int ToInt28(this byte[] array, ref int index)
        {
            if (array.Length - index < 4) throw new ArraySizeException();
            if (((array[index] | array[index + 1] | array[index + 2] | array[index + 3]) & 0x80) != 0) throw new ArgumentException();
            int temp = array[index] << 21 | array[index + 1] << 14 | array[index + 2] << 7 | array[index + 3];
            index += 4;
            return temp;
        }

        public static int ToInt32(this byte[] array, ref int index)
        {
            if (array.Length - index < 4) throw new ArraySizeException();
            int temp = array[index] << 24 | array[index + 1] << 16 | array[index + 2] << 8 | array[index + 3];
            index += 4;
            return temp;
        }

        public static short ToInt16(this byte[] array, ref int index)
        {
            if (array.Length - index < 2) throw new ArraySizeException();
            short temp = (short)(array[index] << 8 | array[index + 1]);
            index += 2;
            return temp;
        }
    }
}
