using ExtensionsLibrary.Extensions;
using MusicPlayerLibrary.Constants;
using MusicPlayerLibrary.Interfaces;
using MusicPlayerLibrary.Models;
using MusicPlayerLibrary.MusicPlayer;
using System;
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
    public sealed partial class SearchPage : Page, IMusicPlayerPage
    {
        public SearchPage()
        {
            InitializeComponent();
#if DEBUG
            Debug.WriteLine($"SearchPage {GetHashCode()} Constructed");
#endif
        }

        ~SearchPage()
        {
#if DEBUG
            Debug.WriteLine($"SearchPage {GetHashCode()} Finalized");
#endif
        }

        public MusicPlayerModel MusicPlayer
        {
            get => (MusicPlayerModel)GetValue(MusicPlayerProperty);
            set => SetValue(MusicPlayerProperty, value);
        }
        public static readonly DependencyProperty MusicPlayerProperty = DependencyProperty.Register("MusicPlayer", typeof(MusicPlayerModel), typeof(SearchPage), new PropertyMetadata(null));

        public MusicPlayerSearch Search
        {
            get => (MusicPlayerSearch)GetValue(SearchProperty);
            set => SetValue(SearchProperty, value);
        }
        public static readonly DependencyProperty SearchProperty = DependencyProperty.Register("Search", typeof(MusicPlayerSearch), typeof(SearchPage), new PropertyMetadata(null));

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter is PageParameters parameters)
            {
                MusicPlayer = parameters.MusicPlayer;
                Search = parameters.MusicPlayer.Search;
            }
        }

        private void ArtistTileTall_PlayPause(object sender, BaseMusicModel e)
        {
            MusicPlayer.PlayMusicModel(e, PlayingLocation.Artist).FireAndForget();
        }

        private void PlaylistTileTall_PlayPause(object sender, BaseMusicModel e)
        {
            MusicPlayer.PlayMusicModel(e, PlayingLocation.Playlist).FireAndForget();
        }

        private void AlbumTileTall_PlayPause(object sender, BaseMusicModel e)
        {
            MusicPlayer.PlayMusicModel(e, PlayingLocation.Album).FireAndForget();
        }

        private void AlbumTileTall_GoToArtist(object sender, ArtistModel e)
        {
            Frame.Navigate(typeof(ArtistContentPage), new PageParameters(MusicPlayer, e));
        }

        private void SongTileCompact_GoToArtist(object sender, ArtistModel e)
        {
            Frame.Navigate(typeof(ArtistContentPage), new PageParameters(MusicPlayer, e));
        }

        private void SongTileCompact_PlayPause(object sender, BaseMusicModel e)
        {
            MusicPlayer.PlayMusicModel(e as SongModel, PlayingLocation.SearchSongs, Search.Songs).FireAndForget();
        }

        private void ArtistsListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            Frame.Navigate(typeof(ArtistContentPage), new PageParameters(MusicPlayer, e.ClickedItem));
        }

        private void PlaylistsListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            Frame.Navigate(typeof(PlaylistContentPage), new PageParameters(MusicPlayer, e.ClickedItem));
        }

        private void AlbumsListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            Frame.Navigate(typeof(AlbumContentPage), new PageParameters(MusicPlayer, e.ClickedItem));
        }

        private void SongsGridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            Frame.Navigate(typeof(AlbumContentPage), new PageParameters(MusicPlayer, (e.ClickedItem as SongModel).ParentAlbum, PageActions.ScrollInToView, e.ClickedItem));
        }

        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (e.NewSize.Width != e.PreviousSize.Width) Search.NumOfVisibleSongs = (int)Math.Floor((((e.NewSize.Width < 1400) ? e.NewSize.Width : 1400) + 30) / 280) * 2;
        }

        private void ContentDisplayButton_ContentDisplayChanged(ContentDisplay newValue)
        {
            Search.ContentDisplay = newValue;
        }
    }
}