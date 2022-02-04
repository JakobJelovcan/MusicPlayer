using ExtensionsLibrary.Extensions;
using MusicPlayerLibrary.Constants;
using MusicPlayerLibrary.Data.Settings;
using System;
using System.Linq;

namespace MusicPlayerLibrary.Lyrics
{
    public class LyricParsers
    {
        public static (TimeSpan, TimeSpan, string, LyricsSinger) ParseLRC(string lyricData)
        {
            if (lyricData.Trim().Split(']') is string[] lyricDataArray && lyricDataArray.Length == 2)
            {
                (string text, LyricsSinger singer) = ParseLyricText(lyricDataArray[1]);
                string[] timeData = lyricDataArray[0].Split(';', StringSplitOptions.RemoveEmptyEntries);
                TimeSpan start = timeData[0].ToTimeSpan();
                TimeSpan duration = (timeData.Length == 2 && timeData[1].ToTimeSpan() is TimeSpan timeSpan && timeSpan > TimeSpan.Zero) ? timeSpan : TimeSpan.FromMilliseconds(Settings.LyricsDuration);
                return (start, duration, text, singer);
            }
            return (default, default, default, default);
        }

        private static (string, LyricsSinger) ParseLyricText(string lyricText)
        {
            switch (string.Concat(lyricText.ToUpper().Take(2)))
            {
                case "D:": return (lyricText.Substring(2), LyricsSinger.Duet);
                case "M:": return (lyricText.Substring(2), LyricsSinger.Male);
                case "F:": return (lyricText.Substring(2), LyricsSinger.Female);
                default: return (lyricText, LyricsSinger.None);
            }
        }
    }
}