using ExtensionsLibrary.Extensions;
using Microsoft.Toolkit.Collections;
using MusicPlayerLibrary.Constants;
using MusicPlayerLibrary.Helpers.Extensions;
using MusicPlayerLibrary.MusicPlayer;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace MusicPlayerLibrary.Models
{
    public class ArtistModel : BaseMusicModel
    {
        [NotMapped]
        public MusicPlayerModel Parent { get; set; }

        public string Artist { get; private set; }

        public TimeSpan Duration => Albums.Sum(A => A.Duration);

        public int NumOfAlbums => Albums.Count;

        public int NumOfSongs => Albums.Sum(A => A.NumOfSongs);

        public string Info => $"{NumOfAlbums} albums, {Duration.ToInfoString()}";

        private List<SongModel> songs;

        public List<SongModel> Songs
        {
            get => songs;
            set
            {
                if (value is not null) songs = value.OrderByDescending(S => S.Year).ThenBy(S => S.Album).ThenBy(S => S.Track).ToList();
            }
        }


        public List<AlbumModel> Albums { get; set; }

        [NotMapped]
        public ObservableGroupedCollection<AlbumModel, SongModel> GroupedContent
        {
            get => groupedContent;
            set
            {
                if (value != groupedContent)
                {
                    groupedContent = value;
                    RaisePropertyChanged(nameof(GroupedContent));
                }
            }
        }
        private ObservableGroupedCollection<AlbumModel, SongModel> groupedContent;

        public ArtistModel Add()
        {
            Parent.AddArtist(this);
            return this;
        }

        public override void Remove()
        {
            if (Albums.Any()) Albums.ToArray().ForEach(A => A.Remove());
            Parent.RemoveArtist(this);
        }

        internal void AddAlbum(AlbumModel album)
        {
            Albums.AddIfDoesntContainInDescendingOrder(album, A => A.Album);
            Songs.AddSongsIfDoesntContainByYearAndTrack(album.Songs);
            GroupedContent.AddIfDoesntContainInDescendingOrder(new ObservableGroup<AlbumModel, SongModel>(album, album.GetSongs()), G => G.Key.Year);
            Parent.AddAlbum(album);
            RaisePropertyChanged(nameof(Duration), nameof(Info), nameof(NumOfSongs), nameof(NumOfAlbums));
        }

        internal void RemoveAlbum(AlbumModel album)
        {
            Albums.Remove(album);
            Parent.RemoveAlbum(album);
            GroupedContent.RemoveGroup(album);
            Songs.RemoveRange(album.GetSongs());
            if (!Albums.Any()) Remove();
            RaisePropertyChanged(nameof(Duration), nameof(Info), nameof(NumOfSongs), nameof(NumOfAlbums));
        }

        internal void AddSong(SongModel song)
        {
            Songs.AddSongByYearAndTrack(song);
            GroupedContent.AddSong(song);
            Parent.AddSong(song);
            RaisePropertyChanged(nameof(Duration), nameof(Info), nameof(NumOfSongs), nameof(NumOfAlbums));
        }

        internal void AddSongToAlbum(SongModel song)
        {
            Songs.AddSongByYearAndTrack(song);
            GroupedContent.AddSongIfDoesntContain(song);
            RaisePropertyChanged(nameof(Duration), nameof(Info), nameof(NumOfSongs), nameof(NumOfAlbums));
        }

        internal void RemoveSong(SongModel song)
        {
            Songs.Remove(song);
            GroupedContent.RemoveItem(song.ParentAlbum, song);
            Parent.RemoveSong(song);
            RaisePropertyChanged(nameof(Duration), nameof(Info), nameof(NumOfSongs), nameof(NumOfAlbums));
        }

        internal void RemoveSongFromAlbum(SongModel song)
        {
            Songs.Remove(song);
            GroupedContent.RemoveItem(song.ParentAlbum, song);
            RaisePropertyChanged(nameof(Duration), nameof(Info), nameof(NumOfSongs), nameof(NumOfAlbums));
        }

        public void SongUpdated(SongModel song, SongUpdateParamater updateParamater)
        {
            if (updateParamater.HasFlag(SongUpdateParamater.Track))
            {
                Songs.UpdateItemInAscendingOrder(song, S => S.ParentAlbum.Year, S => S.Track);
                GroupedContent.UpdateSong(song);
            }
            Parent.SongUpdated(song, updateParamater);
        }

        public void SongsUpdated(IEnumerable<SongModel> songs, SongUpdateParamater updateParamater)
        {
            if (updateParamater.HasFlag(SongUpdateParamater.Track)) songs.ForEach(S => { Songs.UpdateItemInAscendingOrder(S, S1 => S1.ParentAlbum.Year, S1 => S1.Track); GroupedContent.UpdateSong(S); });
            Parent.SongsUpdated(songs, updateParamater);
        }

        public override string GetName()
        {
            return Artist;
        }

        public override MusicPlayerModel GetMusicPlayer()
        {
            return Parent;
        }

        public override IList<SongModel> GetSongs()
        {
            return Songs;
        }

        public override PlayingLocation GetPlayingLocation()
        {
            return PlayingLocation.Artist;
        }

        public ArtistModel() : base()
        {
            Parent = default;
            Artist = default;
            LargeImage = default;
            Albums = new List<AlbumModel>();
            Songs = new List<SongModel>();
            GroupedContent = new ObservableGroupedCollection<AlbumModel, SongModel>();
        }

        public ArtistModel(MusicPlayerModel parent, string artist, bool isEnabled, bool isSaveEnabled, ImageModel image, ImageModel largeImage) : base(isEnabled, isSaveEnabled, 0, 0, image, largeImage)
        {
            Parent = parent;
            Artist = artist;
            Albums = new List<AlbumModel>();
            Songs = new List<SongModel>();
            GroupedContent = new ObservableGroupedCollection<AlbumModel, SongModel>();
        }
    }
}