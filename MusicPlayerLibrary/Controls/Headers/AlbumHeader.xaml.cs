using MusicPlayerLibrary.Events;
using MusicPlayerLibrary.Models;
using System.ComponentModel;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace MusicPlayerLibrary.Controls.Headers
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AlbumHeader : UserControl, INotifyPropertyChanged
    {
        public AlbumHeader()
        {
            InitializeComponent();
        }

        ~AlbumHeader()
        {
        }

        public AlbumModel Album
        {
            get => album;
            set
            {
                if (album != value)
                {
                    album = value;
                    RaisePropertyChanged(nameof(Album));
                }
            }
        }
        private AlbumModel album;

        public event PlayPauseEvent PlayPause
        {
            add => EventRegistrationTokenTable<PlayPauseEvent>.GetOrCreateEventRegistrationTokenTable(ref playPauseEventTable).AddEventHandler(value);
            remove => EventRegistrationTokenTable<PlayPauseEvent>.GetOrCreateEventRegistrationTokenTable(ref playPauseEventTable).RemoveEventHandler(value);
        }
        private EventRegistrationTokenTable<PlayPauseEvent> playPauseEventTable;

        public event GoToArtistEvent GoToArtist
        {
            add => EventRegistrationTokenTable<GoToArtistEvent>.GetOrCreateEventRegistrationTokenTable(ref goToArtistEventTable).AddEventHandler(value);
            remove => EventRegistrationTokenTable<GoToArtistEvent>.GetOrCreateEventRegistrationTokenTable(ref goToArtistEventTable).RemoveEventHandler(value);
        }
        private EventRegistrationTokenTable<GoToArtistEvent> goToArtistEventTable;

        private void GoToArtistButton_Click(object sender, RoutedEventArgs e)
        {
            goToArtistEventTable?.InvocationList?.Invoke(this, Album?.ParentArtist);
        }

        private void PlayAlbumButton_Click(object sender, RoutedEventArgs e)
        {
            playPauseEventTable?.InvocationList?.Invoke(this, Album);
        }

        private void AlbumHeader_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            if (args.NewValue is AlbumModel album) Album = album;
        }

        private void RaisePropertyChanged(params string[] values)
        {
            foreach (string value in values) PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(value));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
