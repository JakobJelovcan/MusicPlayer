using MusicPlayerLibrary.Events;
using MusicPlayerLibrary.Models;
using System.ComponentModel;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace MusicPlayerLibrary.Controls.Headers
{
    public sealed partial class ArtistSmallHeader : UserControl, INotifyPropertyChanged
    {
        public ArtistSmallHeader()
        {
            InitializeComponent();
        }

        ~ArtistSmallHeader()
        {
        }

        public ArtistModel Artist
        {
            get => artist;
            set
            {
                if (artist != value)
                {
                    artist = value;
                    RaisePropertyChanged(nameof(Artist));
                }
            }
        }
        private ArtistModel artist;

        public event PlayPauseEvent PlayPause
        {
            add => EventRegistrationTokenTable<PlayPauseEvent>.GetOrCreateEventRegistrationTokenTable(ref playPauseEventTable).AddEventHandler(value);
            remove => EventRegistrationTokenTable<PlayPauseEvent>.GetOrCreateEventRegistrationTokenTable(ref playPauseEventTable).RemoveEventHandler(value);
        }
        private EventRegistrationTokenTable<PlayPauseEvent> playPauseEventTable;

        private void ArtistSmallHeader_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            if (args.NewValue is ArtistModel artist) Artist = artist;
        }

        private void PlayPauseButton_Click(object sender, RoutedEventArgs e)
        {
            playPauseEventTable?.InvocationList?.Invoke(this, Artist);
        }

        private void RaisePropertyChanged(params string[] values)
        {
            foreach (string value in values) PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(value));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}