namespace MusicPlayerLibrary.Constants
{
    public enum PlayingState { NotPlaying, Playing, Paused }

    public enum PlayingLocation { None, Songs, Album, Artist, Playlist, SongsForYou, SearchSongs, OpenWith }

    public enum PageActions { None, ScrollInToView, NavigateToPage }

    public enum AlbumTile { None, Tall, Wide }

    public enum ArtistTile { None, Tall, Wide }

    public enum PlaylistTile { None, Tall, Wide }

    public enum LoopType { None, RepeatOne, RepeatAll }

    public enum PlaylistDialogTask { None, Create, Edit }

    public enum View { None, Main, FullScreen }

    public enum PointerType { None, LeftButton, RightButton, MiddleButton, X1Button, X2Button }

    public enum LyricsSinger { None, Male, Female, Duet }

    public enum Order { Ascending, Descending }

    public enum SongOrderType { Title, Album, Artist }

    public enum LiveTileStyle { None, CurrentSong, LastPlayed, MostPlayed, AlbumsForYou }

    public enum PlayButtonType { None, Small, SmallNoTint, Medium, Large }

    public enum ColorType { None, Random, Neutral, Grayscale }

    public enum SongUpdateParamater { None = 0, Title = 1, Track = 2, Year = 4, Album = 8 }

    public enum InfoTileSeverity { Informational, Warning, Error, Success }

    public enum ContentDisplay { Less, More }
}