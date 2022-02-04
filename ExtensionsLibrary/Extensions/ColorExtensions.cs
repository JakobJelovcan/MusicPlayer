using System.Numerics;
using Windows.UI;

namespace ExtensionsLibrary.Extensions
{
    public static class ColorExtensions
    {
        public static Vector3 ToVector3(this Color color)
        {
            return new Vector3(color.R, color.G, color.B);
        }

        public static int ToInt32(this Color color)
        {
            return (((((color.A << 8) | color.R) << 8) | color.G) << 8) | color.B;
        }

        public static uint ToUInt32(this Color color)
        {
            return (uint)((((((color.A << 8) | color.R) << 8) | color.G) << 8) | color.B);
        }
    }
}
