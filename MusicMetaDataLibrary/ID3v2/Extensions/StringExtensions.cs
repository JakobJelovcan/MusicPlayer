using System.Linq;
using System.Text;

namespace MusicMetaDataLibrary.ID3v2.Extensions
{
    public static class StringExtensions
    {
        public static (int Current, int Total) SplitToInt32(this string value)
        {
            string[] values = value.Split('/');
            bool currentSuccess = int.TryParse(values.FirstOrDefault(), out int current);
            current = currentSuccess ? current : -1;
            bool totalSuccess = int.TryParse(values.LastOrDefault(), out int total);
            total = totalSuccess ? total : -1;
            return (current, total);
        }

        public static (uint Current, uint Total) SplitToUInt32(this string value)
        {
            string[] values = value.Split('/');
            bool currentSuccess = uint.TryParse(values.FirstOrDefault(), out uint current);
            current = currentSuccess ? current : 0;
            bool totalSuccess = uint.TryParse(values.LastOrDefault(), out uint total);
            total = totalSuccess ? total : 0;
            return (current, total);
        }

        public static string AddPremable(this string value, Encoding encoding)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(encoding.GetBOM());
            stringBuilder.Append(value);
            return stringBuilder.ToString();
        }

        public static string AddPremableAndTermination(this string value, Encoding encoding)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(encoding.GetBOM());
            stringBuilder.Append(value);
            stringBuilder.Append(encoding.GetTerminator());
            return stringBuilder.ToString();
        }
    }
}