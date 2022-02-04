using ExtensionsLibrary.Extensions;
using MusicPlayerLibrary.Constants;
using MusicPlayerLibrary.Events;
using MusicPlayerLibrary.Models;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;


// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace MusicPlayerLibrary.Controls.SongControls
{
    public sealed partial class SongTileCompact : UserControl
    {
        public SongTileCompact()
        {
            InitializeComponent();
#if DEBUG
            Debug.WriteLine($"SongTileCompact {GetHashCode()} Constructed");
#endif
        }

        ~SongTileCompact()
        {
#if DEBUG
            Debug.WriteLine($"SongTileCompact {GetHashCode()} Finalized");
#endif
        }

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

        public SongModel Song
        {
            get => (SongModel)GetValue(SongProperty);
            set
            {
                Song?.UnregisterPropertyChanged(Song_PropertyChanged);
                SetValue(SongProperty, value);
                Song?.RegisterPropertyChanged(Song_PropertyChanged);
                UpdatePlayingStates();
            }
        }
        public static readonly DependencyProperty SongProperty = DependencyProperty.Register("Song", typeof(SongModel), typeof(SongTileCompact), new PropertyMetadata(null));

        public bool TilePointerOver
        {
            get => (bool)GetValue(TilePointerOverProperty);
            set => SetValue(TilePointerOverProperty, value);
        }
        public static readonly DependencyProperty TilePointerOverProperty = DependencyProperty.Register("TilePointerOver", typeof(bool), typeof(SongTileCompact), new PropertyMetadata(false));

        private void UpdatePlayingStates()
        {
            switch (Song?.PlayingState ?? PlayingState.NotPlaying)
            {
                case PlayingState.NotPlaying: VisualStateManager.GoToState(this, nameof(NotPlaying), true); break;
                default: VisualStateManager.GoToState(this, nameof(Playing), true); break;
            }
        }

        private void UpdateDisabledState()
        {
            switch (IsEnabled)
            {
                case true: VisualStateManager.GoToState(this, nameof(Normal), true); break;
                default: VisualStateManager.GoToState(this, nameof(Disabled), true); break;
            }
        }

        private void SongTile_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            if (args.NewValue is SongModel song) Song = song;
        }

        private void Song_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(SongModel.PlayingState): UpdatePlayingStates(); break;
            }
        }

        private void PlayPauseButton_Click(object sender, RoutedEventArgs e)
        {
            playPauseEventTable?.InvocationList?.Invoke(this, Song);
        }

        private void SongTileCompact_DoubleTapped(object sender, Windows.UI.Xaml.Input.DoubleTappedRoutedEventArgs e)
        {
            playPauseEventTable?.InvocationList?.Invoke(this, Song);
        }

        private void GoToArtistButton_Click(object sender, RoutedEventArgs e)
        {
            goToArtistEventTable?.InvocationList?.Invoke(this, Song?.ParentAlbum?.ParentArtist);
        }

        private void SongTileCompact_PointerEntered(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            if (IsEnabled)
            {
                VisualStateManager.GoToState(this, nameof(PointerOver), true);
                TilePointerOver = true;
            }
        }

        private void SongTileCompact_PointerExited(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            if (IsEnabled)
            {
                VisualStateManager.GoToState(this, nameof(Normal), true);
                TilePointerOver = false;
            }
        }

        private void SongTileCompact_PointerPressed(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            if (IsEnabled) VisualStateManager.GoToState(this, nameof(Pressed), true);
        }

        private void SongTileCompact_PointerReleased(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            if (IsEnabled) VisualStateManager.GoToState(this, nameof(PointerOver), true);
        }

        private void SongTileCompact_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            UpdateDisabledState();
        }

        private void SongTileCompact_Loaded(object sender, RoutedEventArgs e)
        {
            Song?.RegisterPropertyChanged(Song_PropertyChanged);
            UpdatePlayingStates();
            UpdateDisabledState();
        }

        private void SongTileCompact_Unloaded(object sender, RoutedEventArgs e)
        {
            Song?.UnregisterPropertyChanged(Song_PropertyChanged);
        }
    }
}