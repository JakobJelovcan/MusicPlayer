using ExtensionsLibrary.Extensions;
using MusicPlayerLibrary.Constants;
using MusicPlayerLibrary.Controls.Buttons;
using MusicPlayerLibrary.Interfaces;
using MusicPlayerLibrary.Models;
using MusicPlayerLibrary.MusicPlayer;
using System.Diagnostics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace MusicPlayer.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SongsPage : Page, IMusicPlayerPage, IScrollablePage
    {
        public SongsPage()
        {
            InitializeComponent();
#if DEBUG
            Debug.WriteLine($"SongsPage {GetHashCode()} Constructed");
#endif
        }

        ~SongsPage()
        {
#if DEBUG
            Debug.WriteLine($"SongsPage {GetHashCode()} Finalized");
#endif
        }

        public MusicPlayerModel MusicPlayer
        {
            get => (MusicPlayerModel)GetValue(MusicPlayerProperty);
            set => SetValue(MusicPlayerProperty, value);
        }
        public static readonly DependencyProperty MusicPlayerProperty = DependencyProperty.Register("MusicPlayer", typeof(MusicPlayerModel), typeof(SongsPage), new PropertyMetadata(null));

        public PageActions PageAction { get; set; }

        public object PageActionTarget { get; set; }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter is PageParameters parameters)
            {
                MusicPlayer = parameters.MusicPlayer;
                PageAction = parameters.PageAction;
                PageActionTarget = parameters.PageActionTarget;
            }
        }

        private void SongsPage_Loaded(object sender, RoutedEventArgs e)
        {
            switch (PageAction)
            {
                case PageActions.ScrollInToView:
                    {
                        ScrollInToView(PageActionTarget);
                        break;
                    }
            }
        }

        public bool ScrollInToView(object obj)
        {
            if (IsLoaded) SongListView.ScrollIntoView(obj ?? MusicPlayer.CurrentPlayingSong);
            return IsLoaded;
        }

        private void SongTile_PlayPause(object sender, BaseMusicModel e)
        {
            MusicPlayer.PlayMusicModel(e, PlayingLocation.Songs, MusicPlayer.Songs).FireAndForget();
        }

        private void SongTile_GoToAlbum(object sender, BaseMusicModel e)
        {
            Frame.Navigate(typeof(AlbumContentPage), new PageParameters(MusicPlayer, e));
        }

        private void SongTile_GoToArtist(object sender, ArtistModel e)
        {
            Frame.Navigate(typeof(ArtistContentPage), new PageParameters(MusicPlayer, e));
        }
    }
}