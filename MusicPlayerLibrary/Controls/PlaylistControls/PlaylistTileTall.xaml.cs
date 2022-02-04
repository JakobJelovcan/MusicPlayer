using MusicPlayerLibrary.Events;
using MusicPlayerLibrary.Models;
using System.Diagnostics;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace MusicPlayerLibrary.Controls.PlaylistControls
{
    public sealed partial class PlaylistTileTall : UserControl
    {
        public PlaylistTileTall()
        {
            InitializeComponent();
#if DEBUG
            Debug.WriteLine($"PlaylistTileTall {GetHashCode()} Constructed");
#endif
        }

        ~PlaylistTileTall()
        {
#if DEBUG
            Debug.WriteLine($"PlaylistTileTall {GetHashCode()} Finalized");
#endif
        }

        public event PlayPauseEvent PlayPause
        {
            add => EventRegistrationTokenTable<PlayPauseEvent>.GetOrCreateEventRegistrationTokenTable(ref playPauseEventTable).AddEventHandler(value);
            remove => EventRegistrationTokenTable<PlayPauseEvent>.GetOrCreateEventRegistrationTokenTable(ref playPauseEventTable).RemoveEventHandler(value);
        }
        private EventRegistrationTokenTable<PlayPauseEvent> playPauseEventTable;

        public PlaylistModel Playlist
        {
            get => (PlaylistModel)GetValue(PlaylistProperty);
            set => SetValue(PlaylistProperty, value);
        }
        public static readonly DependencyProperty PlaylistProperty = DependencyProperty.Register("Playlist", typeof(PlaylistModel), typeof(PlaylistTileTall), new PropertyMetadata(null));

        public bool ImagePointerOver
        {
            get => (bool)GetValue(ImagePointerOverProperty);
            set => SetValue(ImagePointerOverProperty, value);
        }
        public static readonly DependencyProperty ImagePointerOverProperty = DependencyProperty.Register("ImagePointerOver", typeof(bool), typeof(PlaylistTileTall), new PropertyMetadata(false));

        private void PlaylistTileTall_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            if (args.NewValue is PlaylistModel playlist) Playlist = playlist;
        }

        private void PlayPauseButton_Click(object sender, RoutedEventArgs e)
        {
            playPauseEventTable?.InvocationList?.Invoke(this, Playlist);
        }

        private void PlaylistImageBorder_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            ImagePointerOver = true;
        }

        private void PlaylistImageBorder_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            ImagePointerOver = false;
        }

        private void PlaylistTileTall_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, nameof(PointerOver), true);
        }

        private void PlaylistTileTall_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, nameof(Normal), true);
        }

        private void PlaylistTileTall_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, nameof(Pressed), true);
        }

        private void PlaylistTileTall_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, nameof(Normal), true);
        }
    }
}