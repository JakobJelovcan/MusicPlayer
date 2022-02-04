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
    public sealed partial class AlbumContentPage : Page, IMusicPlayerPage, IScrollablePage
    {
        public AlbumContentPage()
        {
            InitializeComponent();
#if DEBUG
            Debug.WriteLine($"AlbumContentPage {GetHashCode()} Constructed");
#endif
        }

        ~AlbumContentPage()
        {
#if DEBUG
            Debug.WriteLine($"AlbumContentPage {GetHashCode()} Finalized");
#endif
        }

        public MusicPlayerModel MusicPlayer
        {
            get => (MusicPlayerModel)GetValue(MusicPlayerProperty);
            set => SetValue(MusicPlayerProperty, value);
        }
        public static readonly DependencyProperty MusicPlayerProperty = DependencyProperty.Register("MusicPlayer", typeof(MusicPlayerModel), typeof(AlbumContentPage), new PropertyMetadata(null));

        public AlbumModel Album
        {
            get => (AlbumModel)GetValue(AlbumProperty);
            set => SetValue(AlbumProperty, value);
        }
        public static readonly DependencyProperty AlbumProperty = DependencyProperty.Register("Album", typeof(AlbumModel), typeof(AlbumContentPage), new PropertyMetadata(null));

        public MoreFromArtistModel MoreFromArtist
        {
            get => (MoreFromArtistModel)GetValue(MoreFromArtistProperty);
            set => SetValue(MoreFromArtistProperty, value);
        }
        public static readonly DependencyProperty MoreFromArtistProperty = DependencyProperty.Register("MoreFromArtist", typeof(MoreFromArtistModel), typeof(AlbumContentPage), new PropertyMetadata(null));

        public PageActions PageAction { get; set; }

        public object PageActionTarget { get; set; }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter is PageParameters parameters)
            {
                MusicPlayer = parameters.MusicPlayer;
                Album = parameters.PageParameter as AlbumModel;
                MoreFromArtist = new MoreFromArtistModel(Album);
                PageAction = parameters.PageAction;
                PageActionTarget = parameters.PageActionTarget;
            }
        }

        private void AlbumContentPage_Loaded(object _, RoutedEventArgs e)
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
            if (IsLoaded) AlbumContentListView.ScrollIntoView(obj ?? MusicPlayer.CurrentPlayingSong);
            return IsLoaded;
        }

        private void SongTileMinimalistic_PlayPause(object _, BaseMusicModel e)
        {
            MusicPlayer.PlayMusicModel(e as SongModel, Album, PlayingLocation.Album).FireAndForget();
        }

        private void AlbumHeader_PlayPause(object _, BaseMusicModel e)
        {
            MusicPlayer.PlayMusicModel(e, PlayingLocation.Album).FireAndForget();
        }

        private void AlbumHeader_GoToArtist(object _, ArtistModel e)
        {
            Frame.Navigate(typeof(ArtistContentPage), new PageParameters(MusicPlayer, e));
        }

        private void MoreFromArtistControl_GoToAlbum(object _, AlbumModel e)
        {
            if (e is AlbumModel album) Frame.Navigate(typeof(AlbumContentPage), new PageParameters(MusicPlayer, album));
        }

        private void MoreFromArtistControl_GoToArtist(object _, ArtistModel e)
        {
            if (e is ArtistModel artist) Frame.Navigate(typeof(ArtistContentPage), new PageParameters(MusicPlayer, artist));
        }

        private void MoreFromArtistControl_PlayPause(object sender, BaseMusicModel e)
        {
            MusicPlayer.PlayMusicModel(e, PlayingLocation.Album).FireAndForget();
        }
    }
}