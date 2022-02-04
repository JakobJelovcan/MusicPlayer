using ExtensionsLibrary.Extensions;
using MusicPlayerLibrary.Constants;
using MusicPlayerLibrary.Helpers.Extensions;
using MusicPlayerLibrary.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace MusicPlayerLibrary.Helpers.Extensions
{
    public static class SongCollectionExtensions
    {
        private static AlbumModel ChangeAlbum(this IEnumerable<SongModel> songs, AlbumModel newAlbum)
        {
            songs.ForEach(S => S.ChangeAlbum(newAlbum));
            return newAlbum;
        }

        public static IEnumerable<SongModel> ChangeAlbum(this IEnumerable<SongModel> songs, string album)
        {
            IEnumerable<IGrouping<AlbumModel, SongModel>> groups = songs.GroupBy(S => S.ParentAlbum);
            foreach (IGrouping<AlbumModel, SongModel> group in groups)
            {
                if (group.Key.ParentArtist.Albums.FirstOrDefault(A => A.Album == album) is AlbumModel newAlbum && newAlbum != group.Key) group.AsEnumerable().ChangeAlbum(newAlbum);
                else group.Key.Album = album;
            }
            return songs.SongsUpdated(SongUpdateParamater.Album);
        }

        public static IEnumerable<SongModel> SongsUpdated(this IEnumerable<SongModel> songs, SongUpdateParamater updateParamater)
        {
            songs.GroupBy(S => S.ParentAlbum).ForEach(G => G.Key.SongsUpdated(G.AsEnumerable(), updateParamater));
            return songs;
        }

        public static void AddSongByYearAndTrack(this IList<SongModel> songs, SongModel song)
        {
            for (int i = 0; i < songs.Count(); i++)
            {
                if (songs[i].Year <= song.ParentAlbum.Year && songs[i].Track >= song.Track)
                {
                    songs.Insert(i, song);
                    return;
                }
                else if (songs[i].Year < song.ParentAlbum.Year)
                {
                    songs.Insert(i, song);
                    return;
                }
            }
            songs.Add(song);
        }

        public static void AddSongIfDoesntContainByYearAndTrack(this IList<SongModel> songs, SongModel song)
        {
            if (songs.Any() && !(songs?.Contains(song) ?? false))
            {
                for (int i = 0; i < songs.Count(); i++)
                {
                    if (songs[i].Year <= song.ParentAlbum.Year && songs[i].Track >= song.Track)
                    {
                        songs.Insert(i, song);
                        return;
                    }
                    else if (songs[i].Year < song.ParentAlbum.Year)
                    {
                        songs.Insert(i, song);
                        return;
                    }
                }
            }
            songs.Add(song);
        }

        public static void AddSongsByYearAndTrack(this IList<SongModel> targetSongs, IEnumerable<SongModel> sourceSongs)
        {
            foreach (SongModel song in sourceSongs) targetSongs.AddSongByYearAndTrack(song);
        }

        public static void AddSongsIfDoesntContainByYearAndTrack(this IList<SongModel> targetSongs, IEnumerable<SongModel> sourceSongs)
        {
            foreach (SongModel song in sourceSongs) targetSongs.AddSongIfDoesntContainByYearAndTrack(song);
        }

        public static void AddInAscendingOrder(this ObservableCollection<SongModel> collection, SongModel song)
        {
            collection.AddInAscendingOrder(song, S => S.Title);
        }

        public static void AddInDescendingOrder(this ObservableCollection<SongModel> collection, SongModel song)
        {
            collection.AddInDescendingOrder(song, S => S.Title);
        }
    }
}