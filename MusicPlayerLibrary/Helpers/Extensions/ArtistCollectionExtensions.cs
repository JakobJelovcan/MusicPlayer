using ExtensionsLibrary.Extensions;
using MusicPlayerLibrary.DataProperties;
using MusicPlayerLibrary.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace MusicPlayerLibrary.Helpers.Extensions
{
    public static class ArtistCollectionExtensions
    {
        public static ArtistModel GetParentArtist(this IEnumerable<ArtistModel> artists, AlbumProperties album)
        {
            return artists.FirstOrDefault(A => A.Artist == album.Artist);
        }

        public static void AddInAscendingOrder(this ObservableCollection<ArtistModel> collection, ArtistModel artist)
        {
            collection.AddInAscendingOrder(artist, A => A.Artist);
        }

        public static void AddInDescendingOrder(this ObservableCollection<ArtistModel> collection, ArtistModel artist)
        {
            collection.AddInDescendingOrder(artist, A => A.Artist);
        }
    }
}
