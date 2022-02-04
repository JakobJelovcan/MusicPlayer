using MusicPlayerLibrary.Events;
using MusicPlayerLibrary.Models;
using System.ComponentModel;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace MusicPlayerLibrary.Controls.Headers
{
    public sealed partial class AlbumSmallHeader : UserControl, INotifyPropertyChanged
    {
        public AlbumSmallHeader()
        {
            InitializeComponent();
        }

        ~AlbumSmallHeader()
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

        public event GoToAlbumEvent GoToAlbum
        {
            add => EventRegistrationTokenTable<GoToAlbumEvent>.GetOrCreateEventRegistrationTokenTable(ref goToAlbumEventTable).AddEventHandler(value);
            remove => EventRegistrationTokenTable<GoToAlbumEvent>.GetOrCreateEventRegistrationTokenTable(ref goToAlbumEventTable).RemoveEventHandler(value);
        }
        private EventRegistrationTokenTable<GoToAlbumEvent> goToAlbumEventTable;

        public bool ImagePointerOver
        {
            get => imagePointerOver;
            set
            {
                if (imagePointerOver != value)
                {
                    imagePointerOver = value;
                    RaisePropertyChanged(nameof(ImagePointerOver));
                }
            }
        }
        private bool imagePointerOver;

        private void AlbumSmallHeader_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            if (args.NewValue is AlbumModel album) Album = album;
        }

        private void AlbumImageBorder_PointerEntered(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            ImagePointerOver = true;
        }

        private void AlbumImageBorder_PointerExited(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            ImagePointerOver = false;
        }

        private void PlayPauseButton_Click(object sender, RoutedEventArgs e)
        {
            playPauseEventTable?.InvocationList?.Invoke(this, Album);
        }

        private void GoToAlbumButton_Click(object sender, RoutedEventArgs e)
        {
            goToAlbumEventTable?.InvocationList?.Invoke(this, Album);
        }

        private void RaisePropertyChanged(params string[] values)
        {
            foreach (string value in values) PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(value));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
