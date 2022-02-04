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
    public sealed partial class PlaylistsPage : Page, IMusicPlayerPage
    {
        public PlaylistsPage()
        {
            InitializeComponent();
#if DEBUG
            Debug.WriteLine($"PlaylistsPage {GetHashCode()} Constructed");
#endif
        }

        ~PlaylistsPage()
        {
#if DEBUG
            Debug.WriteLine($"PlaylistsPage {GetHashCode()} Finalized");
#endif
        }

        public MusicPlayerModel MusicPlayer
        {
            get => (MusicPlayerModel)GetValue(MusicPlayerProperty);
            set => SetValue(MusicPlayerProperty, value);
        }
        public static readonly DependencyProperty MusicPlayerProperty = DependencyProperty.Register("MusicPlayer", typeof(MusicPlayerModel), typeof(PlaylistsPage), new PropertyMetadata(null));

        private PageActions PageAction { get; set; }
        private object PageActionTarget { get; set; }

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

        private void PlaylistsPage_Loaded(object sender, RoutedEventArgs e)
        {
            Settings.PlaylistTileStyleChanged += Settings_PlaylistTileStyleChanged;
            switch (PageAction)
            {
                case PageActions.ScrollInToView:
                    {
                        if (PageActionTarget is PlaylistModel playlist) PlaylistsGridView.ScrollIntoView(playlist);
                        else PlaylistsGridView.ScrollIntoView(MusicPlayer?.CurrentPlayingContent);
                        break;
                    }
            }
        }

        private void PlaylistsPage_Unloaded(object sender, RoutedEventArgs e)
        {
            Settings.PlaylistTileStyleChanged -= Settings_PlaylistTileStyleChanged;
        }

        private void PlaylistsGridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem is PlaylistModel playlist) Frame.Navigate(typeof(PlaylistContentPage), new PageParameters(MusicPlayer, playlist));
        }

        private void PlaylistTile_PlayPause(object sender, BaseMusicModel e)
        {
            MusicPlayer.PlayMusicModel(e, PlayingLocation.Playlist).FireAndForget();
        }

        private void Settings_PlaylistTileStyleChanged(PlaylistTile playlistTile)
        {
            PlaylistsGridView.ItemTemplateSelector = null;
            PlaylistsGridView.ItemTemplateSelector = (PlaylistTemplateSelector)Resources["PlaylistTemplateSelector"];
        }
    }
}