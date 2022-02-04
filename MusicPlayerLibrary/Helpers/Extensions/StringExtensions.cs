using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace MusicPlayerLibrary.Helpers.Extensions
{
    public static class StringExtensions
    {
        public static string ToMD5(this string value)
        {
            StringBuilder stringBuilder = new StringBuilder();
            using (MD5 md5 = MD5.Create()) foreach (byte data in md5.ComputeHash(Encoding.UTF8.GetBytes(value))) stringBuilder.Append(data.ToString("x"));
            return stringBuilder.ToString();
        }

        public static TimeSpan ToTimeSpan(this string value)
        {
            int hours = 0;
            int minutes = 0;
            int seconds = 0;
            int miliseconds = 0;
            if (!string.IsNullOrWhiteSpace(value))
            {
                Queue<string> data = new Queue<string>(value.Split(':'));
                if (data.Count == 3) int.TryParse(data.Dequeue(), out hours);
                if (data.Count == 2) int.TryParse(data.Dequeue(), out minutes);
                if (data.Any())
                {
                    Queue<string> subData = new Queue<string>(data.Dequeue().Split('.'));
                    if (subData.Any())
                    {
                        int.TryParse(subData.Dequeue(), out seconds);
                        if (subData.Any())
                        {
                            string ms = subData.Dequeue();
                            if (ms.Length > 3) int.TryParse(ms.Remove(3), out miliseconds);
                            else int.TryParse(ms, out miliseconds);
                        }
                    }
                }
            }
            return new TimeSpan(0, hours, minutes, seconds, miliseconds);
        }

        public static string Clean(this string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return value;
            while (value.Contains("  ")) value = value.Replace("  ", " ");
            return value;
        }

        public static bool IsInt32(this string value)
        {
            return int.TryParse(value, out int _);
        }

        public static bool IsUInt32(this string value)
        {
            return uint.TryParse(value, out uint _);
        }

        public static bool IsDouble(this string value)
        {
            return double.TryParse(value, out double _);
        }

        public static bool IsFloat(this string value)
        {
            return float.TryParse(value, out float _);
        }

        public static byte[] ToByteArray(this string value)
        {
            return Encoding.Default.GetBytes(value);
        }

        public static byte[] ToByteArray(this string value, Encoding encoding)
        {
            if (encoding is null) throw new ArgumentNullException();
            return encoding.GetBytes(value);
        }
    }
}