using ExtensionsLibrary.Extensions;
using MusicPlayerLibrary.Constants;
using MusicPlayerLibrary.Helpers.Extensions;
using MusicPlayerLibrary.MusicPlayer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace MusicPlayerLibrary.Models
{
    public class AlbumModel : BaseMusicModel
    {
        public ArtistModel ParentArtist { get; private set; }

        public string Album
        {
            get => album;
            set
            {
                if (album != value)
                {
                    album = value;
                    RaisePropertyChanged(nameof(Album));
                }
            }
        }
        private string album;

        public string Artist => ParentArtist.Artist;

        public int Year
        {
            get => year;
            set
            {
                if (value != year)
                {
                    year = value;
                    RaisePropertyChanged(nameof(Year), nameof(Info));
                }
            }
        }
        private int year;

        public TimeSpan Duration => Songs.Sum(S => S.Duration);

        public int NumOfSongs => Songs.Count;

        public string Info => $"{Year} • {NumOfSongs} songs, {Duration.ToInfoString()}";

        public ObservableCollection<SongModel> Songs
        {
            get => songs;
            set
            {
                if (value != songs)
                {
                    songs = value;
                    Genres.Update(songs.Select(S => S.Genre).ToList());
                    RaisePropertyChanged(nameof(Songs), nameof(Duration), nameof(Info), nameof(NumOfSongs));
                }
            }
        }
        private ObservableCollection<SongModel> songs;

        [NotMapped]
        public ObservableCollection<GenreModel> Genres { get; private set; }

        public AlbumModel Add()
        {
            ParentArtist.AddAlbum(this);
            return this;
        }

        public override void Remove()
        {
            if (Songs.Any()) Songs.ToArray().ForEach(S => S.Remove());
            ParentArtist.RemoveAlbum(this);
        }

        internal void AddSong(SongModel song)
        {
            Songs.AddInAscendingOrder(song, S => S.Track);
            Genres.AddIfDoesntContain(song.Genre, G => G?.ID);
            ParentArtist.AddSong(song);
            RaisePropertyChanged(nameof(Duration), nameof(NumOfSongs), nameof(Info));
        }

        internal void AddSongToAlbum(SongModel song)
        {
            Songs.AddIfDoesntContainInAscendingOrder(song, S => S.Track);
            Genres.AddIfDoesntContain(song.Genre, G => G?.ID);
            ParentArtist.AddSongToAlbum(song);
            RaisePropertyChanged(nameof(Duration), nameof(NumOfSongs), nameof(Info));
        }

        internal void RemoveSong(SongModel song)
        {
            Songs.Remove(song);
            ParentArtist.RemoveSong(song);
            if (!Songs.Any()) Remove();
            RaisePropertyChanged(nameof(Duration), nameof(NumOfSongs), nameof(Info));
        }

        internal void RemoveSongFromAlbum(SongModel song)
        {
            Songs.Remove(song);
            ParentArtist.RemoveSongFromAlbum(song);
            if (!Songs.Any()) Remove();
            RaisePropertyChanged(nameof(Duration), nameof(NumOfSongs), nameof(Info));
        }

        public void SongUpdated(SongModel song, SongUpdateParamater updateParamater)
        {
            if (updateParamater.HasFlag(SongUpdateParamater.Track)) Songs.UpdateItemInAscendingOrder(song, S => S.Track);
            ParentArtist.SongUpdated(song, updateParamater);
        }

        public void SongsUpdated(IEnumerable<SongModel> songs, SongUpdateParamater updateParamater)
        {
            if (updateParamater.HasFlag(SongUpdateParamater.Track)) songs.ForEach(S => Songs.UpdateItemInAscendingOrder(S, S1 => S1.Track));
            ParentArtist.SongsUpdated(songs, updateParamater);
        }

        public override string GetName()
        {
            return Album;
        }

        public override MusicPlayerModel GetMusicPlayer()
        {
            return ParentArtist?.GetMusicPlayer();
        }

        public override IList<SongModel> GetSongs()
        {
            return Songs;
        }

        public override PlayingLocation GetPlayingLocation()
        {
            return PlayingLocation.Album;
        }

        public AlbumModel() : base()
        {
            ParentArtist = default;
            Album = default;
            Year = default;
            Genres = new ObservableCollection<GenreModel>();
            Songs = new ObservableCollection<SongModel>();
        }

        public AlbumModel(ArtistModel parentArtist, string album, int year, bool isEnabled, bool isSaveEnabled, ImageModel image, ImageModel largeImage) : base(isEnabled, isSaveEnabled, 0, 0, image, largeImage)
        {
            ParentArtist = parentArtist;
            Album = album;
            Year = year;
            Genres = new ObservableCollection<GenreModel>();
            Songs = new ObservableCollection<SongModel>();
        }
    }
}