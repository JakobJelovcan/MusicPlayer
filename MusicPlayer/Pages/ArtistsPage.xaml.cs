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
    public sealed partial class ArtistsPage : Page, IMusicPlayerPage
    {
        public ArtistsPage()
        {
            InitializeComponent();
#if DEBUG
            Debug.WriteLine($"ArtistsPage {GetHashCode()} Constructed");
#endif
        }

        ~ArtistsPage()
        {
#if DEBUG
            Debug.WriteLine($"ArtistsPage {GetHashCode()} Finalized");
#endif
        }

        public MusicPlayerModel MusicPlayer
        {
            get => (MusicPlayerModel)GetValue(MusicPlayerProperty);
            set => SetValue(MusicPlayerProperty, value);
        }
        public static readonly DependencyProperty MusicPlayerProperty = DependencyProperty.Register("MusicPlayer", typeof(MusicPlayerModel), typeof(ArtistsPage), new PropertyMetadata(null));

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

        private void ArtistPage_Loaded(object sender, RoutedEventArgs e)
        {
            Settings.ArtistTileStyleChanged += Settings_ArtistTileStyleChanged;
            switch (PageAction)
            {
                case PageActions.ScrollInToView:
                    {
                        if (PageActionTarget is ArtistModel artist) ArtistsGridView.ScrollIntoView(artist);
                        else ArtistsGridView.ScrollIntoView(MusicPlayer?.CurrentPlayingContent);
                        break;
                    }
            }
        }

        private void ArtistsPage_Unloaded(object sender, RoutedEventArgs e)
        {
            Settings.ArtistTileStyleChanged += Settings_ArtistTileStyleChanged;
        }

        private void ArtistTile_PlayPause(object sender, BaseMusicModel e)
        {
            MusicPlayer.PlayMusicModel(e, PlayingLocation.Artist).FireAndForget();
        }

        private void ArtistsGridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem is ArtistModel artist) Frame.Navigate(typeof(ArtistContentPage), new PageParameters(MusicPlayer, artist));
        }

        private void Settings_ArtistTileStyleChanged(ArtistTile artistTile)
        {
            ArtistsGridView.ItemTemplateSelector = null;
            ArtistsGridView.ItemTemplateSelector = (ArtistTemplateSelector)Resources["ArtistTemplateSelector"];
        }
    }
}
