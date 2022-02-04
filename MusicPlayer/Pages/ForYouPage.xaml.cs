using ExtensionsLibrary.Extensions;
using MusicPlayerLibrary.Constants;
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
    public sealed partial class ForYouPage : Page, IMusicPlayerPage
    {
        public ForYouPage()
        {
            InitializeComponent();
#if DEBUG
            Debug.WriteLine($"ForYouPage {GetHashCode()} Constructed");
#endif
        }

        ~ForYouPage()
        {
#if DEBUG
            Debug.WriteLine($"ForYouPage {GetHashCode()} Finalized");
#endif
        }

        public MusicPlayerModel MusicPlayer
        {
            get => (MusicPlayerModel)GetValue(MusicPlayerProperty);
            set => SetValue(MusicPlayerProperty, value);
        }
        public static readonly DependencyProperty MusicPlayerProperty = DependencyProperty.Register("MusicPlayer", typeof(MusicPlayerModel), typeof(ForYouPage), new PropertyMetadata(null));

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter is PageParameters parameters) MusicPlayer = parameters.MusicPlayer;
        }

        private void AlbumTileTall_PlayPause(object sender, BaseMusicModel e)
        {
            MusicPlayer.PlayMusicModel(e, PlayingLocation.Album).FireAndForget();
        }

        private void AlbumTileTall_GoToArtist(object sender, ArtistModel e)
        {
            Frame.Navigate(typeof(ArtistContentPage), new PageParameters(MusicPlayer, e));
        }

        private void AlbumsForYouListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            Frame.Navigate(typeof(AlbumContentPage), new PageParameters(MusicPlayer, e.ClickedItem));
        }

        private void SongTileCompact_PlayPause(object sender, BaseMusicModel e)
        {
            MusicPlayer.PlayMusicModel(e, PlayingLocation.SongsForYou, MusicPlayer.SongsForYou).FireAndForget();
        }

        private void SongTileCompact_GoToArtist(object sender, ArtistModel e)
        {
            Frame.Navigate(typeof(ArtistContentPage), new PageParameters(MusicPlayer, e));
        }

        private void SongsForYouGridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            Frame.Navigate(typeof(AlbumContentPage), new PageParameters(MusicPlayer, (e.ClickedItem as SongModel).ParentAlbum, PageActions.ScrollInToView));
        }
    }
}