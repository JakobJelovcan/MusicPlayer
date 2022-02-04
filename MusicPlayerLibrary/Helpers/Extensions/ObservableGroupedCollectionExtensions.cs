using ExtensionsLibrary.Extensions;
using Microsoft.Toolkit.Collections;
using MusicPlayerLibrary.Models;
using System.Collections.Generic;
using System.Linq;

namespace MusicPlayerLibrary.Helpers.Extensions
{
    public static class ObservableGroupedCollectionExtensions
    {
        public static IEnumerable<AlbumModel> GetAlbums(this ObservableGroupedCollection<AlbumModel, SongModel> collection)
        {
            foreach (ObservableGroup<AlbumModel, SongModel> item in collection) yield return item.Key;
        }

        public static IEnumerable<SongModel> GetSongs(this ObservableGroupedCollection<AlbumModel, SongModel> collection)
        {
            foreach (AlbumModel album in collection.GetAlbums()) foreach (SongModel song in album.GetSongs()) yield return song;
        }

        public static void AddSong(this ObservableGroupedCollection<AlbumModel, SongModel> collection, SongModel song)
        {
            if (collection.FirstOrDefault(GC => GC.Key == song.ParentAlbum) is ObservableGroup<AlbumModel, SongModel> GC) GC.AddInAscendingOrder(song, S => S.Track);
            else collection.AddInDescendingOrder(new ObservableGroup<AlbumModel, SongModel>(song.ParentAlbum, new[] { song }), GC => GC.Key.Year);
        }

        public static void AddSongIfDoesntContain(this ObservableGroupedCollection<AlbumModel, SongModel> collection, SongModel song)
        {
            if (collection.FirstOrDefault(GC => GC.Key == song.ParentAlbum) is ObservableGroup<AlbumModel, SongModel> GC) GC.AddIfDoesntContainInAscendingOrder(song, S => S.Track);
            else collection.AddInDescendingOrder(new ObservableGroup<AlbumModel, SongModel>(song.ParentAlbum, new[] { song }), GC => GC.Key.Year);
        }

        public static void UpdateSong(this ObservableGroupedCollection<AlbumModel, SongModel> collection, SongModel song)
        {
            if (collection.FirstOrDefault(GC => GC.Key == song.ParentAlbum) is ObservableGroup<AlbumModel, SongModel> GC) GC.UpdateItemInAscendingOrder(song, S => S.Track);
        }
    }
}