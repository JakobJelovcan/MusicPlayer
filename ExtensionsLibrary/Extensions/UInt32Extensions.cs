using System;
using Windows.UI;

namespace ExtensionsLibrary.Extensions
{
    public static class UInt32Extensions
    {
        public static Color ToColor(this uint value)
        {
            return Color.FromArgb((byte)((value & 0xFF000000) >> 24), (byte)((value & 0x00FF0000) >> 16), (byte)((value & 0x0000FF00) >> 8), (byte)(value & 0x000000FF));
        }

        public static void MakeWithin(this ref uint value, uint min, uint max)
        {
            if (min >= max) throw new ArgumentException();
            else if (value < min) value = min;
            else if (value >= max) value = max - 1;
        }
    }
}