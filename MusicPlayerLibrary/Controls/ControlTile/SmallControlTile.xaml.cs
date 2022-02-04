using ExtensionsLibrary.Extensions;
using MusicPlayerLibrary.Constants;
using MusicPlayerLibrary.Data.Settings;
using MusicPlayerLibrary.Events;
using MusicPlayerLibrary.Helpers.Extensions;
using MusicPlayerLibrary.Models;
using MusicPlayerLibrary.MusicPlayer;
using System;
using System.ComponentModel;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Core;
using Windows.Media.Playback;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace MusicPlayerLibrary.Controls.ControlTile
{
    public sealed partial class SmallControlTile : UserControl, INotifyPropertyChanged
    {
        public SmallControlTile()
        {
            InitializeComponent();
            DataContextChanged += ControlBar_DataContextChanged;
        }

        ~SmallControlTile()
        {

        }

        public MusicPlayerModel MusicPlayer
        {
            get => (MusicPlayerModel)GetValue(MusicPlayerProperty);
            set
            {
                MusicPlayer?.UnregisterPropertyChanged(MusicPlayer_PropertyChanged);
                MusicPlayer?.MediaPlayer?.PlaybackSession?.UnregisterPositionChanged(PlaybackSession_PositionChanged);
                SetValue(MusicPlayerProperty, value);
                MusicPlayer?.RegisterPropertyChanged(MusicPlayer_PropertyChanged);
                MusicPlayer?.MediaPlayer?.PlaybackSession?.RegisterPositionChanged(PlaybackSession_PositionChanged);
                UpdateShuffleStates();
                UpdatePlayingState();
                RaisePropertyChanged(nameof(PlayPauseButtonSymbol), nameof(VolumeButtonSymbol));
            }
        }
        public static readonly DependencyProperty MusicPlayerProperty = DependencyProperty.Register("MusicPlayer", typeof(MusicPlayerModel), typeof(SmallControlTile), new PropertyMetadata(null));

        private string PlayPauseButtonSymbol => MusicPlayer?.CurrentPlayingState == PlayingState.Playing ? Symbols.Pause : Symbols.Play;

        private string VolumeButtonSymbol
        {
            get
            {
                double volume = Settings.Volume;
                if (Settings.Muted) return Symbols.Mute;
                else if (volume == 0) return Symbols.Volume0;
                else if (volume < .33) return Symbols.Volume1;
                else if (volume < .66) return Symbols.Volume2;
                else return Symbols.Volume3;
            }
        }

        public string CurrentTime
        {
            get => (string)GetValue(CurrentTimeProperty);
            set => SetValue(CurrentTimeProperty, value);
        }
        public static readonly DependencyProperty CurrentTimeProperty = DependencyProperty.Register("CurrentTime", typeof(string), typeof(ControlTile), new PropertyMetadata(string.Empty));

        public string TotalTime
        {
            get => (string)GetValue(TotalTimeProperty);
            set => SetValue(TotalTimeProperty, value);
        }
        public static readonly DependencyProperty TotalTimeProperty = DependencyProperty.Register("TotalTime", typeof(string), typeof(ControlTile), new PropertyMetadata(string.Empty));

        public ImageModel Image
        {
            get => (ImageModel)GetValue(ImageProperty);
            set => SetValue(ImageProperty, value);
        }
        public static readonly DependencyProperty ImageProperty = DependencyProperty.Register("Image", typeof(ImageModel), typeof(SmallControlTile), new PropertyMetadata(null));

        public double MaximumPosition
        {
            get => (double)GetValue(MaximumPositionProperty);
            set => SetValue(MaximumPositionProperty, value);
        }
        public static readonly DependencyProperty MaximumPositionProperty = DependencyProperty.Register("MaximumPosition", typeof(double), typeof(ControlTile), new PropertyMetadata(1000d));

        public double CurrentPosition
        {
            get => (double)GetValue(CurrentPositionProperty);
            set
            {
                SetValue(CurrentPositionProperty, value);
                positionChangedEventTable?.InvocationList?.Invoke(CurrentPosition);
            }
        }
        public static readonly DependencyProperty CurrentPositionProperty = DependencyProperty.Register("CurrentPosition", typeof(double), typeof(ControlTile), new PropertyMetadata(0d));

        private double Volume
        {
            get => (double)(MusicPlayer?.MediaPlayer?.Volume ?? 1) * 100;
            set
            {
                if (Settings.Volume != value)
                {
                    Settings.Volume = value / 100;
                    RaisePropertyChanged(nameof(Volume), nameof(VolumeButtonSymbol));
                }
            }
        }

        private void UpdateShuffleStates()
        {
            switch (MusicPlayer?.Shuffle)
            {
                case true: VisualStateManager.GoToState(this, nameof(ShuffleOn), true); break;
                case false: VisualStateManager.GoToState(this, nameof(ShuffleOff), true); break;
            }
        }

        private void UpdatePosition(MediaPlaybackSession session = null)
        {
            session ??= MusicPlayer?.MediaPlayer?.PlaybackSession;
            if (Math.Abs(CurrentPosition - session.Position.TotalMilliseconds) > 200)
            {
                CurrentTime = session?.Position.ToFormatedString() ?? TimeSpan.Zero.ToFormatedString();
                CurrentPosition = session?.Position.TotalMilliseconds ?? 0;
                positionChangedEventTable?.InvocationList?.Invoke(CurrentPosition);
            }
        }

        private void UpdatePlayingState()
        {
            TotalTime = (MusicPlayer?.MediaPlayer?.PlaybackSession?.NaturalDuration != TimeSpan.Zero) ? MusicPlayer?.MediaPlayer?.PlaybackSession?.NaturalDuration.ToFormatedString() ?? string.Empty : string.Empty;
            MaximumPosition = MusicPlayer?.MediaPlayer?.PlaybackSession?.NaturalDuration.TotalMilliseconds ?? 1000;
            Image = MusicPlayer?.CurrentPlayingSong?.Image;
            UpdatePosition();
        }
    }
    //
    //Events
    //
    public partial class SmallControlTile
    {
        public event GoToSongEvent GoToSong
        {
            add => EventRegistrationTokenTable<GoToSongEvent>.GetOrCreateEventRegistrationTokenTable(ref goToSongEventTable).AddEventHandler(value);
            remove => EventRegistrationTokenTable<GoToSongEvent>.GetOrCreateEventRegistrationTokenTable(ref goToSongEventTable).RemoveEventHandler(value);
        }
        private EventRegistrationTokenTable<GoToSongEvent> goToSongEventTable;

        public event GoToArtistEvent GoToArtist
        {
            add => EventRegistrationTokenTable<GoToArtistEvent>.GetOrCreateEventRegistrationTokenTable(ref goToArtistEventTable).AddEventHandler(value);
            remove => EventRegistrationTokenTable<GoToArtistEvent>.GetOrCreateEventRegistrationTokenTable(ref goToArtistEventTable).RemoveEventHandler(value);
        }
        private EventRegistrationTokenTable<GoToArtistEvent> goToArtistEventTable;

        public event PositionChangedEvent PositionChanged
        {
            add => EventRegistrationTokenTable<PositionChangedEvent>.GetOrCreateEventRegistrationTokenTable(ref positionChangedEventTable).AddEventHandler(value);
            remove => EventRegistrationTokenTable<PositionChangedEvent>.GetOrCreateEventRegistrationTokenTable(ref positionChangedEventTable).RemoveEventHandler(value);
        }
        private EventRegistrationTokenTable<PositionChangedEvent> positionChangedEventTable;

        public event RoutedEventHandler ChangeViewClick;
        public event RoutedEventHandler MuteClick;
        public event RoutedEventHandler ShuffleClick;
        public event RoutedEventHandler PlayPauseClick;
        public event RoutedEventHandler PlayPreviousClick;
        public event RoutedEventHandler PlayNextClick;

        private void ControlBar_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            if (args.NewValue is MusicPlayerModel musicPlayer) MusicPlayer = musicPlayer;
        }

        private void MusicPlayer_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(MusicPlayer.Shuffle): UpdateShuffleStates(); break;
                case nameof(MusicPlayer.CurrentPlayingSong): UpdatePlayingState(); break;
                case nameof(MusicPlayer.CurrentPlayingState): UpdatePlayingState(); RaisePropertyChanged(nameof(PlayPauseButtonSymbol)); break;
            }
        }

        private void PlaybackSession_PositionChanged(MediaPlaybackSession sender, object args)
        {
            CoreApplication.MainView.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => UpdatePosition(sender));
        }

        private void ShuffleButton_Click(object sender, RoutedEventArgs e)
        {
            ShuffleClick?.Invoke(this, e);
        }

        private void MuteButton_Click(object sender, RoutedEventArgs e)
        {
            MuteClick?.Invoke(this, e);
        }

        private void MuteButton_PointerPressed(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            if (e.GetPointerType(this) == PointerType.RightButton) FlyoutBase.ShowAttachedFlyout(sender as FrameworkElement);
        }

        private void PlayPreviousButton_Click(object sender, RoutedEventArgs e)
        {
            PlayPreviousClick?.Invoke(this, e);
        }

        private void PlayPauseButton_Click(object sender, RoutedEventArgs e)
        {
            PlayPauseClick?.Invoke(this, e);
        }

        private void PlayNextButton_Click(object sender, RoutedEventArgs e)
        {
            PlayNextClick?.Invoke(this, e);
        }

        private void Settings_MuteChanged(bool newValue)
        {
            RaisePropertyChanged(nameof(VolumeButtonSymbol));
        }

        private void Settings_VolumeChanged(double newValue)
        {
            RaisePropertyChanged(nameof(Volume));
        }

        private void GoToSongButton_Click(object sender, RoutedEventArgs e)
        {
            goToSongEventTable?.InvocationList?.Invoke(this, MusicPlayer?.CurrentPlayingSong);
        }

        private void GoToArtistButton_Click(object sender, RoutedEventArgs e)
        {
            goToArtistEventTable?.InvocationList?.Invoke(this, MusicPlayer?.CurrentPlayingSong?.ParentAlbum?.ParentArtist);
        }

        private void ChangeViewButton_Click(object sender, RoutedEventArgs e)
        {
            ChangeViewClick?.Invoke(this, e);
        }

        private void ControlBar_Loaded(object sender, RoutedEventArgs e)
        {
            Settings.VolumeChanged += Settings_VolumeChanged;
            Settings.MuteChanged += Settings_MuteChanged;
            MusicPlayer?.RegisterPropertyChanged(MusicPlayer_PropertyChanged);
            MusicPlayer?.MediaPlayer?.PlaybackSession?.RegisterPositionChanged(PlaybackSession_PositionChanged);
            RaisePropertyChanged(nameof(PlayPauseButtonSymbol), nameof(VolumeButtonSymbol));
        }

        private void ControlBar_Unloaded(object sender, RoutedEventArgs e)
        {
            Settings.VolumeChanged -= Settings_VolumeChanged;
            Settings.MuteChanged -= Settings_MuteChanged;
            MusicPlayer?.UnregisterPropertyChanged(MusicPlayer_PropertyChanged);
            MusicPlayer?.MediaPlayer?.PlaybackSession?.UnregisterPositionChanged(PlaybackSession_PositionChanged);
        }

        private void RaisePropertyChanged(params string[] values)
        {
            foreach (string value in values) PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(value));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}