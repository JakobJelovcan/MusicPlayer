using ExtensionsLibrary.Extensions;
using MusicPlayerLibrary.DataProperties;
using MusicPlayerLibrary.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace MusicPlayerLibrary.Helpers.Extensions
{
    public static class AlbumCollectionExtensions
    {
        public static AlbumModel GetParentAlbum(this IEnumerable<AlbumModel> albums, SongProperties song)
        {
            return albums.FirstOrDefault(A => A.Artist == song.Artist && A.Album == song.Album);
        }

        public static void AddInAscendingOrder(this ObservableCollection<AlbumModel> collection, AlbumModel album)
        {
            collection.AddInAscendingOrder(album, A => A.Album);
        }

        public static void AddInDescendingOrder(this ObservableCollection<AlbumModel> collection, AlbumModel album)
        {
            collection.AddInDescendingOrder(album, A => A.Album);
        }
    }
}
