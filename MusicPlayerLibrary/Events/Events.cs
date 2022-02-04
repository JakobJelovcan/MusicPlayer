using MusicPlayerLibrary.Constants;
using MusicPlayerLibrary.Models;
using MusicPlayerLibrary.MusicPlayer;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace MusicPlayerLibrary.Events
{
    public delegate void PlayPauseEvent(object sender, BaseMusicModel e);

    public delegate void GoToSongEvent(object sender, SongModel e);

    public delegate void GoToArtistEvent(object sender, ArtistModel e);

    public delegate void GoToAlbumEvent(object sender, AlbumModel e);

    public delegate Task LoadingCompletedCallbackEvent(MusicPlayerModel musicPlayer);

    public delegate void StorageItemAccessEvent(bool successful);

    public delegate void ThemeChangedEvent(ElementTheme newTheme);

    public delegate void MuteChangedEvent(bool newValue);

    public delegate void VolumeChangedEvent(double newValue);

    public delegate void PositionChangedEvent(double newValue);

    public delegate void NumOfGenresForYouChangedEvent(uint newValue);

    public delegate void NumOfAlbumsForYouChangedEvent(uint newValue);

    public delegate void NumOfSongsForYouChangedEvent(uint newValue);

    public delegate void NavigationButtonVisibilityChangedEvent(bool newValue);

    public delegate void EnableOverlayChangedEvent(bool newValue);

    public delegate void AlbumTileStyleChangedEvent(AlbumTile albumTile);

    public delegate void ArtistTileStyleChangedEvent(ArtistTile artistTile);

    public delegate void PlaylistTileStyleChangedEvent(PlaylistTile playlistTile);

    public delegate void EnableSmallControlBarChangedEvent(bool newValue);

    public delegate void ContentDisplayChanged(ContentDisplay newValue);
}
