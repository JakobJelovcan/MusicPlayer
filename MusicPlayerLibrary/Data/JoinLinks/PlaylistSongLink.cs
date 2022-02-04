using MusicPlayerLibrary.Models;

namespace MusicPlayerLibrary.Data.JoinLinks
{
    public class PlaylistSongLink
    {
        public string PlaylistID { get; set; }

        public PlaylistModel Playlist { get; set; }

        public string SongID { get; set; }

        public SongModel Song { get; set; }

        public PlaylistSongLink()
        {
            PlaylistID = default;
            Playlist = default;
            SongID = default;
            Song = default;
        }

        public PlaylistSongLink(PlaylistModel playlist, SongModel song)
        {
            Playlist = playlist;
            PlaylistID = playlist.ID;
            Song = song;
            SongID = song.ID;
        }
    }
}
