using ExtensionsLibrary.Extensions;
using MusicMetaDataLibrary.Helpers.Extensions;
using MusicPlayerLibrary.Constants;
using MusicPlayerLibrary.Data.JoinLinks;
using MusicPlayerLibrary.Helpers.StorageHelpers;
using MusicPlayerLibrary.Info;
using MusicPlayerLibrary.Lyrics;
using MusicPlayerLibrary.MusicPlayer;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.FileProperties;

namespace MusicPlayerLibrary.Models
{
    public class SongModel : BaseMusicModel
    {
        public AlbumModel ParentAlbum
        {
            get => parentAlbum;
            set
            {
                if (parentAlbum != value)
                {
                    parentAlbum?.UnregisterPropertyChanged(ParentAlbum_PropertyChanged);
                    parentAlbum = value;
                    parentAlbum?.RegisterPropertyChanged(ParentAlbum_PropertyChanged);
                    RaisePropertyChanged(nameof(Album));
                }
            }
        }
        private AlbumModel parentAlbum;

        public string Title
        {
            get => title;
            set
            {
                if (value != title)
                {
                    title = value;
                    RaisePropertyChanged(nameof(Title));
                }
            }
        }
        private string title;

        public string Album => ParentAlbum.Album;

        public string Artist => ParentAlbum.Artist;

        public TimeSpan LastPlaybackPosition { get; set; }

        public TimeSpan Duration { get; private set; }

        public uint Track
        {
            get => track;
            set
            {
                if (value != track)
                {
                    track = value;
                    RaisePropertyChanged(nameof(Track));
                }
            }
        }
        private uint track;

        public int Year
        {
            get => year;
            set
            {
                if (value != year)
                {
                    year = value;
                    RaisePropertyChanged(nameof(Year));
                }
            }
        }
        private int year;

        public uint Rating
        {
            get => rating;
            set
            {
                if (rating != value)
                {
                    rating = value;
                    RaisePropertyChanged(nameof(Rating));
                }
            }
        }
        private uint rating;

        public string Path { get; private set; }

        public string Composers { get; set; }

        public string Writers { get; set; }

        public LyricsModel Lyrics
        {
            get => lyrics;
            set
            {
                if (value != lyrics)
                {
                    lyrics = value;
                    RaisePropertyChanged(nameof(Lyrics));
                }
            }
        }
        private LyricsModel lyrics;

        public GenreModel Genre
        {
            get => genre;
            set
            {
                if (value != genre)
                {
                    genre = value;
                    RaisePropertyChanged(nameof(Genre));
                }
            }
        }
        private GenreModel genre;

        public List<PlaylistSongLink> PlaylistSongLinks { get; set; }

        [NotMapped]
        private StorageFile File;

        public async Task<StorageFile> GetStorageFileAsync()
        {
            return File ?? await LoadStorageFileAsync();
        }

        private async Task<StorageFile> LoadStorageFileAsync()
        {
            return File = await StorageFileHelpers.TryGetFileFromPathAsync(Path, (status) => IsEnabled = status);
        }

        public async Task<bool> ValidateSong()
        {
            return IsEnabled = (await GetStorageFileAsync())?.IsAvailable ?? false;
        }

        public SongModel Add()
        {
            ParentAlbum.AddSong(this);
            return this;
        }

        public override void Remove()
        {
            ParentAlbum.RemoveSong(this);
            PlaylistSongLinks.ForEach(PSL => PSL.Playlist.RemoveSong(this));
        }

        public void SongUpdated(SongUpdateParamater updateParamater)
        {
            ParentAlbum.SongUpdated(this, updateParamater);
        }

        public override string GetName()
        {
            return Title;
        }

        public override MusicPlayerModel GetMusicPlayer()
        {
            return ParentAlbum?.GetMusicPlayer();
        }

        public override IList<SongModel> GetSongs()
        {
            return null;
        }

        public override PlayingLocation GetPlayingLocation()
        {
            return PlayingLocation.Songs;
        }

        public async Task SaveChangesToFileAsync()
        {
            //try
            //{
            //    MusicMetaDataLibrary.ID3v2.ID3Tag tag = await (await GetStorageFileAsync()).GetID3TagAsync();
            //    await tag?.Picture?.SetImageAsync(await StorageFile.GetFileFromPathAsync(Image.Path), BitmapEncoder.JpegEncoderId);
            //    tag.Album = Album;
            //    tag.Title = Title;
            //    tag.Artist = Artist;
            //    tag.Track = Track;
            //    tag.Year = Year;
            //    tag.Rating = Rating;
            //    tag.Genre = Genre?.GenreName;
            //    tag.Writers = Writers.Replace("; ", ";").Split(";");
            //    tag.Composers = Composers.Replace("; ", ";").Split(";");
            //    await tag.SaveTagAsync();
            //}
            //catch
            //{
                
            //}
            try
            {
                MusicProperties musicProperties = await (await GetStorageFileAsync()).Properties.GetMusicPropertiesAsync();
                musicProperties.Title = Title;
                musicProperties.Album = Album;
                musicProperties.AlbumArtist = Artist;
                musicProperties.TrackNumber = Track;
                musicProperties.Year = (uint)Year;
                musicProperties.Rating = Rating * 20 - 1;
                await musicProperties.SavePropertiesAsync();
            }
            catch (Exception e)
            {
                InfoMessage.ShowMessage($"The file could not be saved. {e.Message}", InfoTileSeverity.Error, true);
#if DEBUG
                Debug.WriteLine(e.Message);
#endif
            }
        }

        public void ChangeAlbum(AlbumModel newAlbum)
        {
            ParentAlbum.RemoveSongFromAlbum(this);
            ParentAlbum = newAlbum;
            newAlbum.AddSongToAlbum(this);
        }

        public void ChangeAlbum(string album)
        {
            if (ParentAlbum?.ParentArtist?.Albums.FirstOrDefault(A => A.Album == album) is AlbumModel newAlbum && newAlbum != ParentAlbum) ChangeAlbum(newAlbum);
            else ChangeAlbum(new AlbumModel(ParentAlbum?.ParentArtist, album, Year, true, true, Image, LargeImage).Add());
            SongUpdated(SongUpdateParamater.Album);
        }

        private void ParentAlbum_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(AlbumModel.Album): RaisePropertyChanged(nameof(Album)); break;
            }
        }

        public SongModel() : base()
        {
            ParentAlbum = default;
            Title = default;
            Duration = default;
            Track = default;
            Year = default;
            Path = default;
            Composers = default;
            Writers = default;
            Lyrics = default;
            File = default;
        }

        public SongModel(AlbumModel parentAlbum, string title, TimeSpan duration, uint track, int year, uint rating, string path, string composers, string writers, bool isEnabled, bool isSaveEnabled, ImageModel image, ImageModel largeImage, GenreModel genre, StorageFile file) : base(isEnabled, isSaveEnabled, 0, 0, image, largeImage)
        {
            ParentAlbum = parentAlbum;
            Title = title;
            Duration = duration;
            Track = track;
            Year = year;
            Path = path;
            Composers = composers;
            Writers = writers;
            Genre = genre;
            File = file;
            Rating = rating;
            Lyrics = default;
        }
    }
}