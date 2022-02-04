using MusicPlayerLibrary.Models;
using System.ComponentModel;

namespace MusicPlayerLibrary.ViewModels
{
    public class SongViewModel : INotifyPropertyChanged
    {
        public SongViewModel(SongModel song)
        {
            Song = song;
            Track = song.Track;
        }

        public SongModel Song { get; private set; }

        public uint Track
        {
            get => track;
            set
            {
                if (track != value)
                {
                    track = value;
                    RaisePropertyChanged(nameof(Track));
                }
            }
        }
        private uint track;

        public void SaveChanges()
        {
            Song.Track = Track;
            Song.SongUpdated(Constants.SongUpdateParamater.Track);
        }

        private void RaisePropertyChanged(params string[] values)
        {
            foreach (string value in values) PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(value));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
