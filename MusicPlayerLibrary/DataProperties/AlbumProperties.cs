using MusicPlayerLibrary.Models;
using System.Collections.Generic;

namespace MusicPlayerLibrary.DataProperties
{
    public class AlbumProperties
    {
        internal AlbumProperties(string album, string artist, ImageModel image, int year)
        {
            Album = album;
            Artist = artist;
            Image = image;
            Year = year;
        }

        public string Album { get; private set; }

        public string Artist { get; private set; }

        public ImageModel Image { get; private set; }

        public int Year { get; private set; }

        public AlbumModel ToAlbumModel(ArtistModel parentArtist, bool isSaveEnabled = true)
        {
            AlbumModel albumModel = new AlbumModel(parentArtist, Album, Year, true, isSaveEnabled, Image, Image);
            albumModel.Add();
            return albumModel;
        }

        internal ArtistProperties ToArtistProperties()
        {
            return new ArtistProperties(Artist, Image);
        }

        public static IEnumerable<AlbumProperties> LoadProperties(IEnumerable<SongProperties> songsProperties)
        {
            foreach (SongProperties songProperties in songsProperties) yield return songProperties.ToAlbumProperties();
        }
    }
}