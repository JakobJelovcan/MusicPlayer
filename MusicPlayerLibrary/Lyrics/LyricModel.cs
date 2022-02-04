using MusicPlayerLibrary.Constants;
using System;
using System.ComponentModel;

namespace MusicPlayerLibrary.Lyrics
{
    public class LyricModel : INotifyPropertyChanged
    {
        public LyricModel()
        {

        }

        public LyricModel(TimeSpan start, TimeSpan duration, string text, LyricsSinger singer)
        {

            Start = start;
            Duration = duration;
            Text = text;
            Singer = singer;
        }

        public string Text { get; set; }

        public bool IsHighlighted
        {
            get => isHighlighted;
            set
            {
                if (isHighlighted != value)
                {
                    isHighlighted = value;
                    RaisePropertyChanged(nameof(IsHighlighted));
                }
            }
        }
        private bool isHighlighted;

        public TimeSpan Start { get; private set; }

        public TimeSpan Duration { get; private set; }

        public LyricsSinger Singer { get; private set; }

        private void RaisePropertyChanged(params string[] values)
        {
            foreach (string value in values) PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(value));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}