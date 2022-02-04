using MusicPlayerLibrary.Models;
using MusicPlayerLibrary.MusicPlayer;
using System.Collections.Generic;

namespace MusicPlayerLibrary.DataProperties
{
    public class ArtistProperties
    {
        internal ArtistProperties(string artist, ImageModel image)
        {
            Artist = artist;
            Image = image;
        }

        public string Artist { get; private set; }

        public ImageModel Image { get; private set; }

        public ArtistModel ToArtistModel(MusicPlayerModel parent, bool isSaveEnabled = true)
        {
            ArtistModel artistModel = new ArtistModel(parent, Artist, true, isSaveEnabled, Image, Image);
            artistModel.Add();
            return artistModel;
        }

        public static IEnumerable<ArtistProperties> LoadProperties(IEnumerable<AlbumProperties> albumsProperties)
        {
            foreach (AlbumProperties albumProperties in albumsProperties) yield return albumProperties.ToArtistProperties();
        }
    }
}