using ExtensionsLibrary.Extensions;
using MusicPlayerLibrary.Constants;
using MusicPlayerLibrary.Data.JoinLinks;
using MusicPlayerLibrary.Helpers.Extensions;
using MusicPlayerLibrary.MusicPlayer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace MusicPlayerLibrary.Models
{
    public class PlaylistModel : BaseMusicModel
    {
        [NotMapped]
        public MusicPlayerModel Parent { get; set; }

        public string Playlist
        {
            get => playlist;
            set
            {
                if (playlist != value)
                {
                    playlist = value;
                    RaisePropertyChanged(nameof(Playlist));
                }
            }
        }
        private string playlist;

        public TimeSpan Duration => SongLinks.Sum(SL => SL.Song.Duration);

        public int NumOfSongs => SongLinks.Count;

        public string Info => $"{NumOfSongs} songs, {Duration.ToInfoString()}";

        public ObservableCollection<PlaylistSongLink> SongLinks
        {
            get => songLinks;
            set
            {
                if (value != songLinks)
                {
                    songLinks = value;
                    RaisePropertyChanged(nameof(SongLinks));
                }
            }
        }
        private ObservableCollection<PlaylistSongLink> songLinks;

        public PlaylistModel Add()
        {
            Parent.AddPlaylist(this);
            return this;
        }

        public override void Remove()
        {
            Parent.RemovePlaylist(this);
            SongLinks.Clear();
        }

        public void AddSong(SongModel song)
        {
            if (!SongLinks.Any(SL => SL.Song == song))
            {
                SongLinks.AddInAscendingOrder(new PlaylistSongLink(this, song), SL => SL.Song.Title);
                RaisePropertyChanged(nameof(NumOfSongs), nameof(Duration), nameof(Info));
            }
        }

        public void RemoveSong(SongModel song)
        {
            if (SongLinks.FirstOrDefault(SL => SL.Song.Path == song.Path) is PlaylistSongLink songLink)
            {
                SongLinks.Remove(songLink);
                RaisePropertyChanged(nameof(NumOfSongs), nameof(Duration), nameof(Info));
            }
        }

        public void SongUpdated(SongModel song, SongUpdateParamater updateParamater)
        {
            if (updateParamater.HasFlag(SongUpdateParamater.Title)) SongLinks.UpdateItemInAscendingOrder(SongLinks.FirstOrDefault(SL => SL.Song == song), SL => SL.Song.Title);
        }

        public void AddMusicModel(BaseMusicModel musicModel)
        {
            switch (musicModel)
            {
                case SongModel song: AddSong(song); break;
                case AlbumModel album: album.GetSongs().ForEach(S => AddSong(S)); break;
                case ArtistModel artist: artist.GetSongs().ForEach(S => AddSong(S)); break;
                case PlaylistModel playlist: playlist.GetSongs().ForEach(S => AddSong(S)); break;
                default: throw new ArgumentException();
            }
        }

        private string CreateNewName(string baseName)
        {
            if (Parent?.Playlists.Select(P => P.Playlist) is IEnumerable<string> existingNames && existingNames.Any())
            {
                if (!existingNames.Contains(baseName)) return baseName;
                for (int i = 1; i < 100; i++) if ($"{baseName}-{i}" is string newName && !existingNames.Contains(newName)) return newName;
            }
            return baseName;
        }

        public void RemoveDulpicates()
        {
            SongLinks.Where(SL => !SongLinks.DistinctBy(SL => new { SL.Song.Title, SL.Song.Artist }).Contains(SL)).ToArray().ForEach(SL => RemoveSong(SL.Song));
        }

        public override string GetName()
        {
            return Playlist;
        }

        public override MusicPlayerModel GetMusicPlayer()
        {
            return Parent;
        }

        public override IList<SongModel> GetSongs()
        {
            return SongLinks.Select(S => S.Song).ToList();
        }

        public override PlayingLocation GetPlayingLocation()
        {
            return PlayingLocation.Playlist;
        }

        public PlaylistModel() : base(true, 0, 0, default, default)
        {
            Playlist = default;
            SongLinks = new ObservableCollection<PlaylistSongLink>();
        }

        public PlaylistModel(MusicPlayerModel parent, string baseName, bool isEnabled, bool isSaveEnabled, ImageModel image, ImageModel largeImage) : base(isEnabled, isSaveEnabled, 0, 0, image, largeImage)
        {
            Parent = parent;
            Playlist = CreateNewName($"{baseName}-Playlist");
            SongLinks = new ObservableCollection<PlaylistSongLink>();
        }
    }
}