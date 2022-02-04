using MusicPlayerLibrary.Events;
using MusicPlayerLibrary.Models;
using System.Diagnostics;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace MusicPlayerLibrary.Controls.ArtistControls
{
    public sealed partial class ArtistTileWide : UserControl
    {
        public ArtistTileWide()
        {
            InitializeComponent();
#if DEBUG
            Debug.WriteLine($"ArtistTileWide {GetHashCode()} Constructed");
#endif
        }

        ~ArtistTileWide()
        {
#if DEBUG
            Debug.WriteLine($"ArtistTileWide {GetHashCode()} Finalized");
#endif
        }

        public event PlayPauseEvent PlayPause
        {
            add => EventRegistrationTokenTable<PlayPauseEvent>.GetOrCreateEventRegistrationTokenTable(ref playPauseEventTable).AddEventHandler(value);
            remove => EventRegistrationTokenTable<PlayPauseEvent>.GetOrCreateEventRegistrationTokenTable(ref playPauseEventTable).RemoveEventHandler(value);
        }
        private EventRegistrationTokenTable<PlayPauseEvent> playPauseEventTable;

        public ArtistModel Artist
        {
            get => (ArtistModel)GetValue(ArtistProperty);
            set => SetValue(ArtistProperty, value);
        }
        public static readonly DependencyProperty ArtistProperty = DependencyProperty.Register("Artist", typeof(ArtistModel), typeof(ArtistTileWide), new PropertyMetadata(null));

        public bool ImagePointerOver
        {
            get => (bool)GetValue(ImagePointerOverProperty);
            set => SetValue(ImagePointerOverProperty, value);
        }
        public static readonly DependencyProperty ImagePointerOverProperty = DependencyProperty.Register("ImagePointerOver", typeof(bool), typeof(ArtistTileWide), new PropertyMetadata(false));

        private void ArtistTileWide_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            if (args.NewValue is ArtistModel artist) Artist = artist;
        }

        private void PlayPauseButton_Click(object sender, RoutedEventArgs e)
        {
            playPauseEventTable?.InvocationList?.Invoke(this, Artist);
        }

        private void ArtistImageBorder_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            ImagePointerOver = true;
        }

        private void ArtistImageBorder_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            ImagePointerOver = false;
        }

        private void ArtistTileWide_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, nameof(PointerOver), true);
        }

        private void ArtistTileWide_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, nameof(Normal), true);
        }

        private void ArtistTileWide_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, nameof(Pressed), true);
        }

        private void ArtistTileWide_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, nameof(Normal), true);
        }
    }
}