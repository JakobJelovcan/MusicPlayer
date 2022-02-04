using ExtensionsLibrary.Extensions;
using MusicPlayerLibrary.Constants;
using MusicPlayerLibrary.Events;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Storage;
using Windows.UI.Xaml;

namespace MusicPlayerLibrary.Data.Settings
{
    public static partial class Settings
    {
        private static readonly ApplicationDataContainer settingsContainer;

        public static ElementTheme ApplicationTheme
        {
            get => (ElementTheme)settingsContainer.TryGetSetting(nameof(ApplicationTheme), (int)ElementTheme.Light);
            set
            {
                if (value != ApplicationTheme)
                {
                    settingsContainer.TrySaveSetting(nameof(ApplicationTheme), (int)value);
                    themeChangedEventTable?.InvocationList?.Invoke(value);
                }
            }
        }

        public static bool FirstBoot
        {
            get => (bool)settingsContainer.TryGetSetting(nameof(FirstBoot), true);
            set => settingsContainer.TrySaveSetting(nameof(FirstBoot), value);
        }

        public static bool IncludeMusicFolder
        {
            get => (bool)settingsContainer.TryGetSetting(nameof(IncludeMusicFolder), true);
            set => settingsContainer.TrySaveSetting(nameof(IncludeMusicFolder), value);
        }

        public static bool Muted
        {
            get => muted;
            set
            {
                if (muted != value)
                {
                    muted = value;
                    muteChangedEventTable?.InvocationList?.Invoke(value);
                }
            }
        }
        private static bool muted;

        public static double Volume
        {
            get => volume;
            set
            {
                if (volume != value)
                {
                    volume = value < 0 ? 0 : value > 1 ? 1 : value;
                    volumeChangedEventTable?.InvocationList?.Invoke(volume);
                }
            }
        }
        private static double volume;

        public static bool EnableOverlay
        {
            get => (bool)settingsContainer.TryGetSetting(nameof(EnableOverlay), true);
            set
            {
                if (value != EnableOverlay)
                {
                    settingsContainer.TrySaveSetting(nameof(EnableOverlay), value);
                    enableOverlayChangedEventTable?.InvocationList?.Invoke(value);
                }
            }
        }

        public static bool SaveOpenWithContent
        {
            get => (bool)settingsContainer.TryGetSetting(nameof(SaveOpenWithContent), true);
            set => settingsContainer.TrySaveSetting(nameof(SaveOpenWithContent), value);
        }

        public static bool NavigationButtonVisibility
        {
            get => (bool)settingsContainer.TryGetSetting(nameof(NavigationButtonVisibility), true);
            set
            {
                if (value != NavigationButtonVisibility)
                {
                    settingsContainer.TrySaveSetting(nameof(NavigationButtonVisibility), value);
                    navigationButtonVisibilityChangedEventTable?.InvocationList?.Invoke(value);
                }
            }
        }

        public static bool ShowSongMissingError
        {
            get => (bool)settingsContainer.TryGetSetting(nameof(ShowSongMissingError), true);
            set => settingsContainer.TrySaveSetting(nameof(ShowSongMissingError), value);
        }

        public static bool HideFullScreenAlbumArt
        {
            get => (bool)settingsContainer.TryGetSetting(nameof(HideFullScreenAlbumArt), true);
            set => settingsContainer.TrySaveSetting(nameof(HideFullScreenAlbumArt), value);
        }

        public static bool EnableSmallControlBar
        {
            get => (bool)settingsContainer.TryGetSetting(nameof(EnableSmallControlBar), false);
            set
            {
                if(value != EnableSmallControlBar)
                {
                    settingsContainer.TrySaveSetting(nameof(EnableSmallControlBar), value);
                    enableSmallControlBarChangedEventTable?.InvocationList?.Invoke(value);
                }
            }
        }

        public static uint NumOfGenresForYou
        {
            get => (uint)settingsContainer.TryGetSetting(nameof(NumOfGenresForYou), 3u);
            set
            {
                if (value != NumOfGenresForYou)
                {
                    settingsContainer.TrySaveSetting(nameof(NumOfGenresForYou), value);
                    numOfGenresChangedEventTable?.InvocationList?.Invoke(value);
                }
            }
        }

        public static uint NumOfAlbumsPerGenre
        {
            get => (uint)settingsContainer.TryGetSetting(nameof(NumOfAlbumsPerGenre), 3u);
            set
            {
                if (value != NumOfAlbumsPerGenre)
                {
                    settingsContainer.TrySaveSetting(nameof(NumOfAlbumsPerGenre), value);
                    numOfAlbumsChangedEventTable?.InvocationList?.Invoke(value);
                }
            }
        }

        public static uint NumOfSongsPerGenre
        {
            get => (uint)settingsContainer.TryGetSetting(nameof(NumOfSongsPerGenre), 10u);
            set
            {
                if (value != NumOfSongsPerGenre)
                {
                    settingsContainer.TrySaveSetting(nameof(NumOfSongsPerGenre), value);
                    numOfSongsChangedEventTable?.InvocationList?.Invoke(value);
                }
            }
        }

        public static uint LyricsDuration
        {
            get => (uint)settingsContainer.TryGetSetting(nameof(LyricsDuration), 3000u);
            set => settingsContainer.TrySaveSetting(nameof(LyricsDuration), value);
        }

        public static AlbumTile AlbumTileStyle
        {
            get => (AlbumTile)settingsContainer.TryGetSetting(nameof(AlbumTileStyle), (int)AlbumTile.Wide);
            set
            {
                settingsContainer.TrySaveSetting(nameof(AlbumTileStyle), (int)value);
                albumTileStyleChangedEventTable?.InvocationList?.Invoke(value);
            }
        }

        public static ArtistTile ArtistTileStyle
        {
            get => (ArtistTile)settingsContainer.TryGetSetting(nameof(ArtistTileStyle), (int)ArtistTile.Wide);
            set
            {
                settingsContainer.TrySaveSetting(nameof(ArtistTileStyle), (int)value);
                artistTileStyleChangedEventTable?.InvocationList?.Invoke(value);
            }
        }

        public static PlaylistTile PlaylistTileStyle
        {
            get => (PlaylistTile)settingsContainer.TryGetSetting(nameof(PlaylistTileStyle), (int)PlaylistTile.Wide);
            set
            {
                settingsContainer.TrySaveSetting(nameof(PlaylistTileStyle), (int)value);
                playlistTileStyleChangedEventTable?.InvocationList?.Invoke(value);
            }
        }

        public static int NumOfMostPlayedItems
        {
            get => (int)settingsContainer.TryGetSetting(nameof(NumOfMostPlayedItems), 10);
            set
            {
                if (value != NumOfMostPlayedItems)
                {
                    settingsContainer.TrySaveSetting(nameof(NumOfMostPlayedItems), value);
                }
            }
        }

        public static int NumOfLastPlayedItems
        {
            get => (int)settingsContainer.TryGetSetting(nameof(NumOfLastPlayedItems), 10);
            set
            {
                if (value != NumOfLastPlayedItems)
                {
                    settingsContainer.TrySaveSetting(nameof(NumOfLastPlayedItems), value);
                }
            }
        }

        public static void SaveChanges()
        {
            settingsContainer.TrySaveSetting(nameof(Volume), Volume);
            settingsContainer.TrySaveSetting(nameof(Muted), muted);
        }

        static Settings()
        {
            settingsContainer = ApplicationData.Current.LocalSettings;
            volume = (double)settingsContainer.TryGetSetting(nameof(Volume), 1.0);
            muted = (bool)settingsContainer.TryGetSetting(nameof(Muted), false);
        }
    }
    //
    //Events
    //
    public static partial class Settings
    {
        public static event ThemeChangedEvent ThemeChanged
        {
            add => EventRegistrationTokenTable<ThemeChangedEvent>.GetOrCreateEventRegistrationTokenTable(ref themeChangedEventTable).AddEventHandler(value);
            remove => EventRegistrationTokenTable<ThemeChangedEvent>.GetOrCreateEventRegistrationTokenTable(ref themeChangedEventTable).RemoveEventHandler(value);
        }
        private static EventRegistrationTokenTable<ThemeChangedEvent> themeChangedEventTable;

        public static event MuteChangedEvent MuteChanged
        {
            add => EventRegistrationTokenTable<MuteChangedEvent>.GetOrCreateEventRegistrationTokenTable(ref muteChangedEventTable).AddEventHandler(value);
            remove => EventRegistrationTokenTable<MuteChangedEvent>.GetOrCreateEventRegistrationTokenTable(ref muteChangedEventTable).RemoveEventHandler(value);
        }
        private static EventRegistrationTokenTable<MuteChangedEvent> muteChangedEventTable;

        public static event VolumeChangedEvent VolumeChanged
        {
            add => EventRegistrationTokenTable<VolumeChangedEvent>.GetOrCreateEventRegistrationTokenTable(ref volumeChangedEventTable).AddEventHandler(value);
            remove => EventRegistrationTokenTable<VolumeChangedEvent>.GetOrCreateEventRegistrationTokenTable(ref volumeChangedEventTable).RemoveEventHandler(value);
        }
        private static EventRegistrationTokenTable<VolumeChangedEvent> volumeChangedEventTable;

        public static event NumOfGenresForYouChangedEvent NumOfGenresForYouChanged
        {
            add => EventRegistrationTokenTable<NumOfGenresForYouChangedEvent>.GetOrCreateEventRegistrationTokenTable(ref numOfGenresChangedEventTable).AddEventHandler(value);
            remove => EventRegistrationTokenTable<NumOfGenresForYouChangedEvent>.GetOrCreateEventRegistrationTokenTable(ref numOfGenresChangedEventTable).RemoveEventHandler(value);
        }
        private static EventRegistrationTokenTable<NumOfGenresForYouChangedEvent> numOfGenresChangedEventTable;

        public static event NumOfAlbumsForYouChangedEvent NumOfAlbumsForYouChanged
        {
            add => EventRegistrationTokenTable<NumOfAlbumsForYouChangedEvent>.GetOrCreateEventRegistrationTokenTable(ref numOfAlbumsChangedEventTable).AddEventHandler(value);
            remove => EventRegistrationTokenTable<NumOfAlbumsForYouChangedEvent>.GetOrCreateEventRegistrationTokenTable(ref numOfAlbumsChangedEventTable).RemoveEventHandler(value);
        }
        private static EventRegistrationTokenTable<NumOfAlbumsForYouChangedEvent> numOfAlbumsChangedEventTable;

        public static event NumOfSongsForYouChangedEvent NumOfSongsForYouChanged
        {
            add => EventRegistrationTokenTable<NumOfSongsForYouChangedEvent>.GetOrCreateEventRegistrationTokenTable(ref numOfSongsChangedEventTable).AddEventHandler(value);
            remove => EventRegistrationTokenTable<NumOfSongsForYouChangedEvent>.GetOrCreateEventRegistrationTokenTable(ref numOfSongsChangedEventTable).RemoveEventHandler(value);
        }
        private static EventRegistrationTokenTable<NumOfSongsForYouChangedEvent> numOfSongsChangedEventTable;

        public static event NavigationButtonVisibilityChangedEvent NavigationButtonVisibilityChanged
        {
            add => EventRegistrationTokenTable<NavigationButtonVisibilityChangedEvent>.GetOrCreateEventRegistrationTokenTable(ref navigationButtonVisibilityChangedEventTable).AddEventHandler(value);
            remove => EventRegistrationTokenTable<NavigationButtonVisibilityChangedEvent>.GetOrCreateEventRegistrationTokenTable(ref navigationButtonVisibilityChangedEventTable).RemoveEventHandler(value);
        }
        private static EventRegistrationTokenTable<NavigationButtonVisibilityChangedEvent> navigationButtonVisibilityChangedEventTable;

        public static event EnableOverlayChangedEvent EnableOverlayChanged
        {
            add => EventRegistrationTokenTable<EnableOverlayChangedEvent>.GetOrCreateEventRegistrationTokenTable(ref enableOverlayChangedEventTable).AddEventHandler(value);
            remove => EventRegistrationTokenTable<EnableOverlayChangedEvent>.GetOrCreateEventRegistrationTokenTable(ref enableOverlayChangedEventTable).RemoveEventHandler(value);
        }
        private static EventRegistrationTokenTable<EnableOverlayChangedEvent> enableOverlayChangedEventTable;

        public static event AlbumTileStyleChangedEvent AlbumTileStyleChanged
        {
            add => EventRegistrationTokenTable<AlbumTileStyleChangedEvent>.GetOrCreateEventRegistrationTokenTable(ref albumTileStyleChangedEventTable).AddEventHandler(value);
            remove => EventRegistrationTokenTable<AlbumTileStyleChangedEvent>.GetOrCreateEventRegistrationTokenTable(ref albumTileStyleChangedEventTable).RemoveEventHandler(value);
        }
        private static EventRegistrationTokenTable<AlbumTileStyleChangedEvent> albumTileStyleChangedEventTable;

        public static event ArtistTileStyleChangedEvent ArtistTileStyleChanged
        {
            add => EventRegistrationTokenTable<ArtistTileStyleChangedEvent>.GetOrCreateEventRegistrationTokenTable(ref artistTileStyleChangedEventTable).AddEventHandler(value);
            remove => EventRegistrationTokenTable<ArtistTileStyleChangedEvent>.GetOrCreateEventRegistrationTokenTable(ref artistTileStyleChangedEventTable).RemoveEventHandler(value);
        }
        private static EventRegistrationTokenTable<ArtistTileStyleChangedEvent> artistTileStyleChangedEventTable;

        public static event PlaylistTileStyleChangedEvent PlaylistTileStyleChanged
        {
            add => EventRegistrationTokenTable<PlaylistTileStyleChangedEvent>.GetOrCreateEventRegistrationTokenTable(ref playlistTileStyleChangedEventTable).AddEventHandler(value);
            remove => EventRegistrationTokenTable<PlaylistTileStyleChangedEvent>.GetOrCreateEventRegistrationTokenTable(ref playlistTileStyleChangedEventTable).RemoveEventHandler(value);
        }
        private static EventRegistrationTokenTable<PlaylistTileStyleChangedEvent> playlistTileStyleChangedEventTable;

        public static event EnableSmallControlBarChangedEvent EnableSmallControlBarChanged
        {
            add => EventRegistrationTokenTable<EnableSmallControlBarChangedEvent>.GetOrCreateEventRegistrationTokenTable(ref enableSmallControlBarChangedEventTable).AddEventHandler(value);
            remove => EventRegistrationTokenTable<EnableSmallControlBarChangedEvent>.GetOrCreateEventRegistrationTokenTable(ref enableSmallControlBarChangedEventTable).RemoveEventHandler(value);
        }
        private static EventRegistrationTokenTable<EnableSmallControlBarChangedEvent> enableSmallControlBarChangedEventTable;
    }
}