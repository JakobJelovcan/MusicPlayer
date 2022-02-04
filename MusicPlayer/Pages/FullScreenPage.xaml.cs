using ExtensionsLibrary.Extensions;
using MusicPlayer.Helpers.NavigationHelpers;
using MusicPlayerLibrary.Constants;
using MusicPlayerLibrary.Data.Settings;
using MusicPlayerLibrary.Interfaces;
using MusicPlayerLibrary.Models;
using MusicPlayerLibrary.MusicPlayer;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Numerics;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;


// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace MusicPlayer.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class FullScreenPage : Page, IMusicPlayerPage
    {
        public FullScreenPage()
        {
            InitializeComponent();
            HideControlBarTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(4) };
            HideControlBarTimer.Tick += HideControlBarTimer_Tick;
#if DEBUG
            Debug.WriteLine($"FullScreenPage {GetHashCode()} Constructed");
#endif
        }

        ~FullScreenPage()
        {
#if DEBUG
            Debug.WriteLine($"FullScreenPage {GetHashCode()} Finalized");
#endif
        }

        public MusicPlayerModel MusicPlayer
        {
            get => (MusicPlayerModel)GetValue(MusicPlayerProperty);
            set => SetValue(MusicPlayerProperty, value);
        }
        public static readonly DependencyProperty MusicPlayerProperty = DependencyProperty.Register("MusicPlayer", typeof(MusicPlayerModel), typeof(FullScreenPage), new PropertyMetadata(null));

        public ImageModel Image
        {
            get => (ImageModel)GetValue(ImageProperty);
            set => SetValue(ImageProperty, value);
        }
        public static readonly DependencyProperty ImageProperty = DependencyProperty.Register("Image", typeof(ImageModel), typeof(FullScreenPage), new PropertyMetadata(null));

        public ImageModel LargeImage
        {
            get => (ImageModel)GetValue(LargeImageProperty);
            set => SetValue(LargeImageProperty, value);
        }
        public static readonly DependencyProperty LargeImageProperty = DependencyProperty.Register("LargeImage", typeof(ImageModel), typeof(FullScreenPage), new PropertyMetadata(null));
        private readonly DispatcherTimer HideControlBarTimer;

        public Vector3 ControlBarPosition
        {
            get => (Vector3)GetValue(ControlBarPositionProperty);
            set => SetValue(ControlBarPositionProperty, value);
        }
        public static readonly DependencyProperty ControlBarPositionProperty = DependencyProperty.Register("ControlBarPosition", typeof(Vector3), typeof(FullScreenPage), new PropertyMetadata(new Vector3(0)));

        public float ImageLayerOpacity
        {
            get => (float)GetValue(ImageLayerOpacityProperty);
            set => SetValue(ImageLayerOpacityProperty, value);
        }
        public static readonly DependencyProperty ImageLayerOpacityProperty = DependencyProperty.Register("ImageLayerOpacity", typeof(float), typeof(FullScreenPage), new PropertyMetadata(1f));

        private void HideControlBarTimer_Tick(object sender, object e)
        {
            ControlBarPosition = new Vector3(0, 70, 0);
            if (Settings.HideFullScreenAlbumArt) ImageLayerOpacity = 0;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter is PageParameters parameters)
            {
                MusicPlayer = parameters.MusicPlayer;
                MusicPlayer?.RegisterPropertyChanged(MusicPlayer_PropertyChanged);
                (Image, LargeImage) = (MusicPlayer?.CurrentPlayingSong?.Image, MusicPlayer?.CurrentPlayingSong?.LargeImage);
            }
        }

        private void MusicPlayer_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(MusicPlayerModel.CurrentPlayingSong)) (Image, LargeImage) = (MusicPlayer?.CurrentPlayingSong?.Image, MusicPlayer?.CurrentPlayingSong?.LargeImage);
        }

        private void FullScreenPage_Loaded(object sender, RoutedEventArgs e)
        {
            Window.Current.SetTitleBar(TitleBar);
        }

        private void FullScreenPage_Unloaded(object sender, RoutedEventArgs e)
        {
            MusicPlayer?.UnregisterPropertyChanged(MusicPlayer_PropertyChanged);
        }
        //
        //ControlBar
        //
        private void ControlBar_ChangeView(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage), new PageParameters(MusicPlayer));
        }

        private void ControlBar_GoToSong(object sender, SongModel e)
        {
            try
            {
                Frame.Navigate(typeof(MainPage), new PageParameters(MusicPlayer, PageActions.NavigateToPage, NavigationHelpers.GetPageTypeFromPlayingLocation(MusicPlayer.CurrentPlayingLocation), new PageParameters(MusicPlayer, MusicPlayer?.CurrentPlayingContent, PageActions.ScrollInToView, e)));
            }
            catch (Exception ex)
            {
#if DEBUG
                Debug.WriteLine(ex.Message);
#endif
            }
        }

        private void ControlBar_GoToArtist(object sender, ArtistModel e)
        {
            Frame.Navigate(typeof(MainPage), new PageParameters(MusicPlayer, PageActions.NavigateToPage, typeof(ArtistContentPage), new PageParameters(MusicPlayer, e, PageActions.ScrollInToView)));
        }

        private async void ControlBar_PlayPreviousClick(object sender, RoutedEventArgs e)
        {
            await MusicPlayer?.PlayPrevious();
        }

        private void ControlBar_PlayPauseClick(object sender, RoutedEventArgs e)
        {
            MusicPlayer?.PlayPause();
        }

        private async void ControlBar_PlayNextClick(object sender, RoutedEventArgs e)
        {
            await MusicPlayer?.PlayNext();
        }

        private void ControlBar_MuteClick(object sender, RoutedEventArgs e)
        {
            MusicPlayer?.CycleMute();
        }

        private void ControlBar_ShuffleClick(object sender, RoutedEventArgs e)
        {
            MusicPlayer?.CycleShuffle();
        }

        private void ControlBar_LoopClick(object sender, RoutedEventArgs e)
        {
            MusicPlayer.CycleLoop();
        }

        private void ControlBar_PointerExited(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            HideControlBarTimer.Start();
        }

        private void FullScreenPage_PointerMoved(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            if (ControlBarPosition.Y > 0) if (CoreWindow.GetForCurrentThread().PointerPosition.Y - Window.Current.Bounds.Y > ActualHeight - 70)
                {
                    ControlBarPosition = new Vector3(0);
                    ImageLayerOpacity = 1;
                }
        }

        private void ControlBar_PointerEntered(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            HideControlBarTimer.Stop();
        }

        private void ControlBar_LyricsVisibilityChanged(object sender, RoutedEventArgs e)
        {
            MusicPlayer.LyricsPlayer.CycleIsEnabled();
        }

        private void ControlBar_PositionChanged(double newValue)
        {
            MusicPlayer.MediaPlayer.PlaybackSession.Position = TimeSpan.FromMilliseconds(newValue);
            MusicPlayer?.LyricsPlayer?.PlayLyrics();
        }
    }
}