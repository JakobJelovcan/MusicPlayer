using MusicPlayerLibrary.Events;
using MusicPlayerLibrary.Models;
using System.ComponentModel;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace MusicPlayerLibrary.Controls.Headers
{
    public sealed partial class PlaylistHeader : UserControl, INotifyPropertyChanged
    {
        public PlaylistHeader()
        {
            InitializeComponent();
        }

        ~PlaylistHeader()
        {
        }

        public PlaylistModel Playlist
        {
            get => playlist;
            set
            {
                if (playlist != value)
                {
                    playlist = value;
                    RaisePropertyChanged(nameof(Playlist));
                }
            }
        }
        private PlaylistModel playlist;

        public event PlayPauseEvent PlayPause
        {
            add => EventRegistrationTokenTable<PlayPauseEvent>.GetOrCreateEventRegistrationTokenTable(ref playPauseEventTable).AddEventHandler(value);
            remove => EventRegistrationTokenTable<PlayPauseEvent>.GetOrCreateEventRegistrationTokenTable(ref playPauseEventTable).RemoveEventHandler(value);
        }
        private EventRegistrationTokenTable<PlayPauseEvent> playPauseEventTable;

        private void PlaylistHeader_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            if (args.NewValue is PlaylistModel playlist) Playlist = playlist;
        }

        private void PlayPlaylistButton_Click(object sender, RoutedEventArgs e)
        {
            playPauseEventTable?.InvocationList?.Invoke(this, Playlist);
        }

        private void RaisePropertyChanged(params string[] values)
        {
            foreach (string value in values) PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(value));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
