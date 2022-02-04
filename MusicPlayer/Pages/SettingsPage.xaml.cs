using MusicPlayerLibrary.Constants;
using MusicPlayerLibrary.ContentDialogs;
using MusicPlayerLibrary.Data.Settings;
using MusicPlayerLibrary.Info;
using MusicPlayerLibrary.Interfaces;
using MusicPlayerLibrary.Models;
using MusicPlayerLibrary.MusicPlayer;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace MusicPlayer.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SettingsPage : Page, INotifyPropertyChanged, IMusicPlayerPage
    {
        public SettingsPage()
        {
            InitializeComponent();
#if DEBUG
            Debug.WriteLine($"SettingsPage {GetHashCode()} Constructed");
#endif
        }

        ~SettingsPage()
        {
#if DEBUG
            Debug.WriteLine($"SettingsPage {GetHashCode()} Finalized");
#endif
        }

        public MusicPlayerModel MusicPlayer
        {
            get => (MusicPlayerModel)GetValue(MusicPlayerProperty);
            set => SetValue(MusicPlayerProperty, value);
        }
        public static readonly DependencyProperty MusicPlayerProperty = DependencyProperty.Register("MusicPlayer", typeof(MusicPlayerModel), typeof(SettingsPage), new PropertyMetadata(null));

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter is PageParameters parameters) MusicPlayer = parameters.MusicPlayer;
        }

        private bool EnableOverlay
        {
            get => Settings.EnableOverlay;
            set
            {
                if (value != EnableOverlay)
                {
                    Settings.EnableOverlay = value;
                    RaisePropertyChanged(nameof(EnableOverlay));
                }
            }
        }

        private bool SaveOpenWithContent
        {
            get => Settings.SaveOpenWithContent;
            set
            {
                if (value != SaveOpenWithContent)
                {
                    Settings.SaveOpenWithContent = value;
                    RaisePropertyChanged(nameof(SaveOpenWithContent));
                }
            }
        }

        private bool NavigationButtonVisibility
        {
            get => Settings.NavigationButtonVisibility;
            set
            {
                if (value != NavigationButtonVisibility)
                {
                    Settings.NavigationButtonVisibility = value;
                    RaisePropertyChanged(nameof(NavigationButtonVisibility));
                }
            }
        }

        private bool ShowSongMissingError
        {
            get => Settings.ShowSongMissingError;
            set
            {
                if (value != ShowSongMissingError)
                {
                    Settings.ShowSongMissingError = value;
                    RaisePropertyChanged(nameof(ShowSongMissingError));
                }
            }
        }

        private bool HideFullScreenAlbumArt
        {
            get => Settings.HideFullScreenAlbumArt;
            set
            {
                if (value != HideFullScreenAlbumArt)
                {
                    Settings.HideFullScreenAlbumArt = value;
                    RaisePropertyChanged(nameof(HideFullScreenAlbumArt));
                }
            }
        }

        private bool EnableSmallControlBar
        {
            get => Settings.EnableSmallControlBar;
            set
            {
                if (value != EnableSmallControlBar)
                {
                    Settings.EnableSmallControlBar = value;
                    RaisePropertyChanged(nameof(EnableSmallControlBar));
                }
            }
        }

        private string ThemeName
        {
            get
            {
                switch (ApplicationTheme)
                {
                    case ElementTheme.Dark: return ResourceLoader.GetForCurrentView("SettingResources").GetString("Theme_Dark/Text");
                    case ElementTheme.Light: return ResourceLoader.GetForCurrentView("SettingResources").GetString("Theme_Light/Text");
                    default: return ResourceLoader.GetForCurrentView("SettingResources").GetString("Error/Text");
                }
            }
        }

        private ElementTheme ApplicationTheme
        {
            get => Settings.ApplicationTheme;
            set
            {
                if (value != ApplicationTheme)
                {
                    Settings.ApplicationTheme = value;
                    RaisePropertyChanged(nameof(ApplicationTheme), nameof(ThemeName));
                }
            }
        }

        private uint NumOfGenresForYou
        {
            get => Settings.NumOfGenresForYou;
            set
            {
                if (value != NumOfGenresForYou)
                {
                    Settings.NumOfGenresForYou = value;
                    RaisePropertyChanged(nameof(NumOfGenresForYou));
                }
            }
        }
            
        private uint NumOfAlbumsForYou
        {
            get => Settings.NumOfAlbumsPerGenre;
            set
            {
                if (value != NumOfAlbumsForYou)
                {
                    Settings.NumOfAlbumsPerGenre = value;
                    RaisePropertyChanged(nameof(NumOfAlbumsForYou));
                }
            }
        }

        private uint NumOfSongsForYou
        {
            get => Settings.NumOfSongsPerGenre;
            set
            {
                if (value != NumOfSongsForYou)
                {
                    Settings.NumOfSongsPerGenre = value;
                    RaisePropertyChanged(nameof(NumOfSongsForYou));
                }
            }
        }

        private uint LyricsDuration
        {
            get => Settings.LyricsDuration;
            set
            {
                if (value != LyricsDuration)
                {
                    Settings.LyricsDuration = value;
                    RaisePropertyChanged(nameof(LyricsDuration));
                }
            }
        }

        private string AlbumTileName
        {
            get
            {
                switch (AlbumTileStyle)
                {
                    case AlbumTile.Tall: return ResourceLoader.GetForCurrentView("SettingResources").GetString("Tall_Tile_Style/Text");
                    case AlbumTile.Wide: return ResourceLoader.GetForCurrentView("SettingResources").GetString("Wide_Tile_Style/Text");
                    default: return ResourceLoader.GetForCurrentView("SettingResources").GetString("Error/Text");
                }
            }
        }

        private AlbumTile AlbumTileStyle
        {
            get => Settings.AlbumTileStyle;
            set
            {
                if (value != AlbumTileStyle)
                {
                    Settings.AlbumTileStyle = value;
                    RaisePropertyChanged(nameof(AlbumTileStyle), nameof(AlbumTileName));
                }
            }
        }

        private string ArtistTileName
        {
            get
            {
                switch (ArtistTileStyle)
                {
                    case ArtistTile.Tall: return ResourceLoader.GetForCurrentView("SettingResources").GetString("Tall_Tile_Style/Text");
                    case ArtistTile.Wide: return ResourceLoader.GetForCurrentView("SettingResources").GetString("Wide_Tile_Style/Text");
                    default: return ResourceLoader.GetForCurrentView("SettingResources").GetString("Error/Text");
                }
            }
        }

        private ArtistTile ArtistTileStyle
        {
            get => Settings.ArtistTileStyle;
            set
            {
                if (value != ArtistTileStyle)
                {
                    Settings.ArtistTileStyle = value;
                    RaisePropertyChanged(nameof(ArtistTileStyle), nameof(ArtistTileName));
                }
            }
        }

        private string PlaylistTileName
        {
            get
            {
                switch (PlaylistTileStyle)
                {
                    case PlaylistTile.Tall: return ResourceLoader.GetForCurrentView("SettingResources").GetString("Tall_Tile_Style/Text");
                    case PlaylistTile.Wide: return ResourceLoader.GetForCurrentView("SettingResources").GetString("Wide_Tile_Style/Text");
                    default: return ResourceLoader.GetForCurrentView("SettingResources").GetString("Error/Text");
                }
            }
        }

        private PlaylistTile PlaylistTileStyle
        {
            get => Settings.PlaylistTileStyle;
            set
            {
                if (value != PlaylistTileStyle)
                {
                    Settings.PlaylistTileStyle = value;
                    RaisePropertyChanged(nameof(PlaylistTileStyle), nameof(PlaylistTileName));
                }
            }
        }

        private void ThemeLightMenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {
            ApplicationTheme = ElementTheme.Light;
        }

        private void ThemeDarkMenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {
            ApplicationTheme = ElementTheme.Dark;
        }

        private void AlbumTileTallMenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {
            AlbumTileStyle = AlbumTile.Tall;
        }

        private void AlbumTileWideMenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {
            AlbumTileStyle = AlbumTile.Wide;            
        }

        private void ArtistTileTallMenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {
            ArtistTileStyle = ArtistTile.Tall;
        }

        private void ArtistTileWideMenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {
            ArtistTileStyle = ArtistTile.Wide;
        }

        private void PlaylistTileTallMenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {
            PlaylistTileStyle = PlaylistTile.Tall;
        }

        private void PlaylistTileWideMenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {
            PlaylistTileStyle = PlaylistTile.Wide;
        }

        private async void OpenStorageFoldersDialogButton_Click(object sender, RoutedEventArgs e)
        {
            await new StorageLocationsContentDialog(MusicPlayer).ShowAsync();
        }

        private async void ReloadButton_Click(object sender, RoutedEventArgs e)
        {
            int oldCount = MusicPlayer.Songs.Count;
            await MusicPlayer.LoadDataFromStorageAsync(true);
            int newCount = MusicPlayer.Songs.Count;
            if (newCount - oldCount >= 0) InfoMessage.ShowMessage($"{newCount - oldCount} new songs have been added.", InfoTileSeverity.Success, true);
        }

        private async void VerifyButton_Click(object _, RoutedEventArgs _1)
        {
            await Task.WhenAll(MusicPlayer?.Songs?.Select(S => S.ValidateSong()));
            if (MusicPlayer?.Songs?.Where(S => !S.IsEnabled).Count() is int num && num > 0) InfoMessage.ShowMessage($"There has been a problem loading {num} of your songs.", InfoTileSeverity.Error, true);
            else InfoMessage.ShowMessage($"All your songs have passed validation.", InfoTileSeverity.Success, true);
        }

        private async void ClearImageCacheButton_Click(object sender, RoutedEventArgs e)
        {
            await MusicPlayer.ClearImageCache();
        }

        private async void ClearLyricsCacheButton_Click(object sender, RoutedEventArgs e)
        {
            await MusicPlayer.ClearLyricsCache();
        }

        private async void RestartButton_Click(object sender, RoutedEventArgs e)
        {
            await CoreApplication.RequestRestartAsync(string.Empty);
        }

        private void RaisePropertyChanged(params string[] values)
        {
            foreach (string value in values) PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(value));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}