using ExtensionsLibrary.Extensions;
using MusicPlayerLibrary.Constants;
using MusicPlayerLibrary.Data.Settings;
using MusicPlayerLibrary.Events;
using MusicPlayerLibrary.Lyrics;
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
using Windows.ApplicationModel.Resources;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace MusicPlayerLibrary.Controls.ControlTile
{
    public sealed partial class ControlTile : UserControl, INotifyPropertyChanged
    {
        public ControlTile()
        {
            InitializeComponent();
            DataContextChanged += ControlBar_DataContextChanged;
        }

        ~ControlTile()
        {

        }

        public MusicPlayerModel MusicPlayer
        {
            get => (MusicPlayerModel)GetValue(MusicPlayerProperty);
            set
            {
                MusicPlayer?.UnregisterPropertyChanged(MusicPlayer_PropertyChanged);
                MusicPlayer?.LyricsPlayer?.UnregisterPropertyChanged(LyricsPlayer_PropertyChanged);
                MusicPlayer?.MediaPlayer?.PlaybackSession?.UnregisterPositionChanged(PlaybackSession_PositionChanged);
                SetValue(MusicPlayerProperty, value);
                MusicPlayer?.RegisterPropertyChanged(MusicPlayer_PropertyChanged);
                MusicPlayer?.LyricsPlayer?.RegisterPropertyChanged(LyricsPlayer_PropertyChanged);
                MusicPlayer?.MediaPlayer?.PlaybackSession?.RegisterPositionChanged(PlaybackSession_PositionChanged);
                UpdateLoopStates();
                UpdateShuffleStates();
                UpdateMicStates();
                UpdatePlayingState();
                RaisePropertyChanged(nameof(PlayPauseButtonSymbol), nameof(LoopButtonSymbol), nameof(VolumeButtonSymbol));
            }
        }
        public static readonly DependencyProperty MusicPlayerProperty = DependencyProperty.Register("MusicPlayer", typeof(MusicPlayerModel), typeof(ControlTile), new PropertyMetadata(null));

        private string ChangeViewButtonIcon => (CurrentView == View.Main) ? Symbols.FullScreen : Symbols.BackToWindow;

        private string ChangeViewButtonHint => (CurrentView == View.Main) ? ResourceLoader.GetForViewIndependentUse("MusicPlayerLibrary/ControlTileResources").GetString("Fullscreen_Hint/Text") : ResourceLoader.GetForViewIndependentUse("MusicPlayerLibrary/ControlTileResources").GetString("Mainscreen_Hint/Text");

        private string LoopButtonSymbol
        {
            get
            {
                switch (MusicPlayer?.Loop ?? LoopType.None)
                {
                    case LoopType.RepeatOne: return Symbols.RepeatOne;
                    case LoopType.RepeatAll: return Symbols.RepeatAll;
                    default: return Symbols.RepeatOff;
                }
            }
        }

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

        public Visibility LyricsButtonVisibility
        {
            get => (Visibility)GetValue(LyricsButtonVisibilityProperty);
            set => SetValue(LyricsButtonVisibilityProperty, value);
        }
        public static readonly DependencyProperty LyricsButtonVisibilityProperty = DependencyProperty.Register("LyricsButtonVisibility", typeof(Visibility), typeof(ControlTile), new PropertyMetadata(Visibility.Collapsed));

        public View CurrentView
        {
            get => (View)GetValue(CurrentViewProperty);
            set
            {
                SetValue(CurrentViewProperty, value);
                RaisePropertyChanged(nameof(ChangeViewButtonIcon), nameof(ChangeViewButtonHint));
            }
        }
        public static readonly DependencyProperty CurrentViewProperty = DependencyProperty.Register("CurrentView", typeof(View), typeof(ControlTile), new PropertyMetadata(View.Main));

        public ImageModel Image
        {
            get => (ImageModel)GetValue(ImageProperty);
            set => SetValue(ImageProperty, value);
        }
        public static readonly DependencyProperty ImageProperty = DependencyProperty.Register("Image", typeof(ImageModel), typeof(ControlTile), new PropertyMetadata(null));

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

        private void UpdateLoopStates()
        {
            switch (MusicPlayer?.Loop)
            {
                case LoopType.None: VisualStateManager.GoToState(this, nameof(LoopOff), true); break;
                default: VisualStateManager.GoToState(this, nameof(LoopOn), true); break;
            }
        }

        private void UpdateMicStates()
        {
            switch (MusicPlayer?.LyricsPlayer.IsEnabled & MusicPlayer?.LyricsPlayer?.IsLoaded)
            {
                case true: VisualStateManager.GoToState(this, nameof(MicOn), true); break;
                case false: VisualStateManager.GoToState(this, nameof(MicOff), true); break;
            }
        }

        private void UpdatePosition(MediaPlaybackSession session = null)
        {
            session ??= MusicPlayer?.MediaPlayer?.PlaybackSession;
            if (session is not null && Math.Abs(CurrentPosition - session.Position.TotalMilliseconds) > 200)
            {
                CurrentTime = session?.Position.ToFormatedString() ?? TimeSpan.Zero.ToFormatedString();
                CurrentPosition = session?.Position.TotalMilliseconds ?? 0;
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
    public partial class ControlTile
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
        public event RoutedEventHandler LyricsVisibilityChanged;
        public event RoutedEventHandler MuteClick;
        public event RoutedEventHandler ShuffleClick;
        public event RoutedEventHandler LoopClick;
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
                case nameof(MusicPlayer.Loop): UpdateLoopStates(); RaisePropertyChanged(nameof(LoopButtonSymbol)); break;
                case nameof(MusicPlayer.CurrentPlayingSong): UpdatePlayingState(); break;
                case nameof(MusicPlayer.CurrentPlayingState): UpdatePlayingState(); RaisePropertyChanged(nameof(PlayPauseButtonSymbol)); break;
            }
        }

        private void LyricsPlayer_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(LyricsPlayer.IsEnabled):
                case nameof(LyricsPlayer.IsLoaded):
                    {
                        UpdateMicStates();
                        break;
                    }
            }
        }

        private async void PlaybackSession_PositionChanged(MediaPlaybackSession sender, object args)
        {
            await CoreApplication.MainView.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => UpdatePosition(sender));
        }

        private void LoopButton_Click(object sender, RoutedEventArgs e)
        {
            LoopClick?.Invoke(this, e);
        }

        private void ShuffleButton_Click(object sender, RoutedEventArgs e)
        {
            ShuffleClick?.Invoke(this, e);
        }

        private void MuteButton_Click(object sender, RoutedEventArgs e)
        {
            MuteClick?.Invoke(this, e);
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

        private void LyricsVisibilityButton_Click(object sender, RoutedEventArgs e)
        {
            LyricsVisibilityChanged?.Invoke(this, e);
        }

        private void ControlBar_Loaded(object sender, RoutedEventArgs e)
        {
            MusicPlayer?.RegisterPropertyChanged(MusicPlayer_PropertyChanged);
            MusicPlayer?.LyricsPlayer?.RegisterPropertyChanged(LyricsPlayer_PropertyChanged);
            MusicPlayer?.MediaPlayer?.PlaybackSession?.RegisterPositionChanged(PlaybackSession_PositionChanged);
            Settings.VolumeChanged += Settings_VolumeChanged;
            Settings.MuteChanged += Settings_MuteChanged;
            RaisePropertyChanged(nameof(PlayPauseButtonSymbol), nameof(LoopButtonSymbol), nameof(VolumeButtonSymbol));
        }

        private void ControlBar_Unloaded(object sender, RoutedEventArgs e)
        {
            MusicPlayer?.UnregisterPropertyChanged(MusicPlayer_PropertyChanged);
            MusicPlayer?.LyricsPlayer?.UnregisterPropertyChanged(LyricsPlayer_PropertyChanged);
            MusicPlayer?.MediaPlayer?.PlaybackSession?.UnregisterPositionChanged(PlaybackSession_PositionChanged);
            Settings.VolumeChanged -= Settings_VolumeChanged;
            Settings.MuteChanged -= Settings_MuteChanged;
        }

        private void RaisePropertyChanged(params string[] values)
        {
            foreach (string value in values) PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(value));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}