using System.Numerics;
using Windows.UI;

namespace MusicPlayerLibrary.Helpers.Extensions
{
    public static class Vector3Extensions
    {
        public static Color ToColor(this Vector3 vector)
        {
            return Color.FromArgb(0xFF, (byte)vector.X, (byte)vector.Y, (byte)vector.Z);
        }

        public static int ToInt32(this Vector3 vector)
        {
            return (((((0xFF << 8) | (byte)vector.X) << 8) | (byte)vector.Y) << 8) | (byte)vector.Z;
        }

        public static uint ToUInt32(this Vector3 vector)
        {
            return (uint)((((((0xFF << 8) | (byte)vector.X) << 8) | (byte)vector.Y) << 8) | (byte)vector.Z);
        }
    }
}
