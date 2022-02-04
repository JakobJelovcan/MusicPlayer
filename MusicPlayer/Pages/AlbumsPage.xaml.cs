using ExtensionsLibrary.Extensions;
using MusicPlayerLibrary.Constants;
using MusicPlayerLibrary.Data.Settings;
using MusicPlayerLibrary.Interfaces;
using MusicPlayerLibrary.Models;
using MusicPlayerLibrary.MusicPlayer;
using MusicPlayerLibrary.TemplateSelectors;
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
    public sealed partial class AlbumsPage : Page, IMusicPlayerPage
    {
        public AlbumsPage()
        {
            InitializeComponent();
#if DEBUG
            Debug.WriteLine($"AlbumsPage {GetHashCode()} Constructed");
#endif
        }

        ~AlbumsPage()
        {
#if DEBUG
            Debug.WriteLine($"AlbumsPage {GetHashCode()} Finalized");
#endif
        }

        public MusicPlayerModel MusicPlayer
        {
            get => (MusicPlayerModel)GetValue(MusicPlayerProperty);
            set => SetValue(MusicPlayerProperty, value);
        }
        public static readonly DependencyProperty MusicPlayerProperty = DependencyProperty.Register("MusicPlayer", typeof(MusicPlayerModel), typeof(AlbumsPage), new PropertyMetadata(null));

        private PageActions PageAction;
        private object PageActionTarget;

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

        private void AlbumsPage_Loaded(object sender, RoutedEventArgs e)
        {
            Settings.AlbumTileStyleChanged += Settings_AlbumTileStyleChanged;
            switch (PageAction)
            {
                case PageActions.ScrollInToView:
                    {
                        if (PageActionTarget is AlbumModel album) AlbumsGridView.ScrollIntoView(album);
                        else AlbumsGridView.ScrollIntoView(MusicPlayer?.CurrentPlayingContent);
                        break;
                    }
            }
        }

        private void AlbumsPage_Unloaded(object sender, RoutedEventArgs e)
        {
            Settings.AlbumTileStyleChanged -= Settings_AlbumTileStyleChanged;
        }

        private void AlbumsGridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem is AlbumModel album) Frame.Navigate(typeof(AlbumContentPage), new PageParameters(MusicPlayer, album));
        }

        private void AlbumTile_PlayPause(object sender, BaseMusicModel e)
        {
            MusicPlayer.PlayMusicModel(e, PlayingLocation.Album).FireAndForget();
        }

        private void AlbumTile_GoToArtist(object sender, ArtistModel e)
        {
            Frame.Navigate(typeof(ArtistContentPage), new PageParameters(MusicPlayer, e));
        }

        private void Settings_AlbumTileStyleChanged(AlbumTile albumTile)
        {
            AlbumsGridView.ItemTemplateSelector = null;
            AlbumsGridView.ItemTemplateSelector = (AlbumTemplateSelector)Resources["AlbumTemplateSelector"];
        }
    }
}