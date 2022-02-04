using ExtensionsLibrary.Extensions;
using MusicPlayerLibrary.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.FileProperties;

namespace MusicPlayerLibrary.DataProperties
{
    public class SongProperties
    {
        private SongProperties()
        {

        }

        public MusicProperties Properties { get; private set; }

        public string Title { get; private set; }

        public string Album { get; private set; }

        public string Artist { get; private set; }

        private GenreModel Genre { get; set; }

        private ImageModel Image { get; set; }

        private StorageFile File { get; set; }

        public static async Task<SongProperties> LoadPropertiesAsync(StorageFile storageFile)
        {
            SongProperties @this = new SongProperties
            {
                Properties = await storageFile.Properties.GetMusicPropertiesAsync()
            };
            @this.Title = @this.Properties.Title;
            @this.Album = @this.Properties.Album;
            @this.Artist = @this.Properties.AlbumArtist;
            @this.File = storageFile;
            @this.Image = await ImageModel.GetOrCreateImageFromThumbnailAsync(@this.Album.ToMD5(), await @this.File.GetScaledImageAsThumbnailAsync(ThumbnailMode.SingleItem, 1000));
            @this.Genre = GenreModel.GetOrCreateGenre(@this.Properties.Genre.FirstOrDefault());
            return @this;
        }

        public static async Task<List<SongProperties>> LoadPropertiesAsync(IEnumerable<StorageFile> storageFiles)
        {
            ConcurrentBag<SongProperties> songProperties = new ConcurrentBag<SongProperties>();
            await Task.WhenAll(storageFiles.Select(async F => songProperties.Add(await LoadPropertiesAsync(F))));
            return songProperties.ToList();
        }

        public SongModel ToSongModel(AlbumModel parentAlbum, bool isSaveEnabled = true)
        {
            return new SongModel(parentAlbum, Title, Properties.Duration, Properties.TrackNumber, (int)Properties.Year, (uint)Math.Round(Properties.Rating / 25.0) + 1, File.Path, string.Join("; ", Properties.Composers), string.Join("; ", Properties.Writers), true, isSaveEnabled, Image, Image, Genre, File).Add();
        }

        internal AlbumProperties ToAlbumProperties()
        {
            return new AlbumProperties(Properties.Album, Properties.AlbumArtist, Image, (int)Properties.Year);
        }
    }
}