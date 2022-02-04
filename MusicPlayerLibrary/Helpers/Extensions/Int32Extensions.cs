using Windows.UI;
using System;

namespace MusicPlayerLibrary.Helpers.Extensions
{
    public static class Int32Extensions
    {
        public static Color ToColor(this int value)
        {
            return Color.FromArgb((byte)((value & 0xFF000000) >> 24), (byte)((value & 0x00FF0000) >> 16), (byte)((value & 0x0000FF00) >> 8), (byte)(value & 0x000000FF));
        }

        public static void MakeWithin(this ref int value, int min, int max)
        {
            if (min >= max) throw new ArgumentException();
            else if (value < min) value = min;
            else if (value >= max) value = max - 1;            
        }
    }
}
