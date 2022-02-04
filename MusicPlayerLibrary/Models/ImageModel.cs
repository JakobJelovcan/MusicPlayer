using ExtensionsLibrary.Extensions;
using MusicPlayerLibrary.Constants;
using MusicPlayerLibrary.Data.DataBase;
using MusicPlayerLibrary.Helpers.ImageHelpers;
using MusicPlayerLibrary.Helpers.StorageHelpers;
using MusicPlayerLibrary.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.UI;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace MusicPlayerLibrary.Models
{
    public class ImageModel : IEntity
    {
        public string ID { get; private set; }

        public string Path { get; set; }

        public string RelativePath => $"ms-appdata:///local/ImageCache/{System.IO.Path.GetFileName(Path)}";

        public uint PrimaryUIntColor { get; set; }

        public uint SecondaryUIntColor { get; set; }

        public uint TertiaryUIntColor { get; set; }

        [NotMapped]
        public Color PrimaryColor => PrimaryUIntColor.ToColor();

        [NotMapped]
        public Color SecondaryColor => SecondaryUIntColor.ToColor();

        [NotMapped]
        public Color TertiaryColor => TertiaryUIntColor.ToColor();

        public ImageBrush ImageBrush50 => imageBrush50 ??= new ImageBrush { ImageSource = new BitmapImage { UriSource = new Uri(Path), DecodePixelHeight = 50, DecodePixelWidth = 50 } };
        private ImageBrush imageBrush50;

        public ImageBrush ImageBrush60 => imageBrush60 ??= new ImageBrush { ImageSource = new BitmapImage { UriSource = new Uri(Path), DecodePixelHeight = 60, DecodePixelWidth = 60 } };
        private ImageBrush imageBrush60;

        public ImageBrush ImageBrush130 => imageBrush130 ??= new ImageBrush { ImageSource = new BitmapImage { UriSource = new Uri(Path), DecodePixelHeight = 130, DecodePixelWidth = 130 } };
        private ImageBrush imageBrush130;

        public ImageBrush ImageBrush150 => imageBrush150 ??= new ImageBrush { ImageSource = new BitmapImage { UriSource = new Uri(Path), DecodePixelHeight = 150, DecodePixelWidth = 150 } };
        private ImageBrush imageBrush150;

        public ImageBrush ImageBrush180 => imageBrush180 ??= new ImageBrush { ImageSource = new BitmapImage { UriSource = new Uri(Path), DecodePixelHeight = 180, DecodePixelWidth = 180 } };
        private ImageBrush imageBrush180;

        public ImageBrush ImageBrush200 => imageBrush200 ??= new ImageBrush { ImageSource = new BitmapImage { UriSource = new Uri(Path), DecodePixelHeight = 200, DecodePixelWidth = 200 } };
        private ImageBrush imageBrush200;

        public IEnumerable<BaseMusicModel> SmallImageReferences { get; private set; }

        public IEnumerable<BaseMusicModel> LargeImageReferences { get; private set; }
        private static readonly ConcurrentBag<ImageModel> ImageCache;

        public static async Task<ImageModel> GetOrCreateImageFromThumbnailAsync(string id, StorageItemThumbnail thumbnail, bool getColors = false)
        {
            if (ImageCache.FirstOrDefault(I => I.ID == id) is ImageModel imageModel) return imageModel;
            imageModel = ImageCache.AddAndReturn(new ImageModel(id));
            imageModel.Path = (await SaveThumbnailAsync($"{id}.jpeg", thumbnail)).Path;
            if (getColors) (imageModel.PrimaryUIntColor, imageModel.SecondaryUIntColor, imageModel.TertiaryUIntColor) = await LoadColorsFromThumbnailAsync(thumbnail);
            return imageModel;
        }

        public static async Task<ImageModel> GetOrCreateImageFromFileAsync(string id, StorageFile storageFile, bool getColors = false)
        {
            if (ImageCache.FirstOrDefault(I => I.ID == id || I.Path == storageFile.Path) is ImageModel imageModel) return imageModel;
            imageModel = ImageCache.AddAndReturn(new ImageModel(id));
            imageModel.Path = (await storageFile.TryCopyAsync(StorageConstants.ImageCacheFolder, storageFile.Name, NameCollisionOption.ReplaceExisting)).Path;
            if (getColors) (imageModel.PrimaryUIntColor, imageModel.SecondaryUIntColor, imageModel.TertiaryUIntColor) = await LoadColorsFromFileAsync(storageFile);
            return imageModel;
        }

        private static async Task<StorageFile> SaveThumbnailAsync(string fileName, StorageItemThumbnail thumbnail)
        {
            if (await StorageConstants.ImageCacheFolder.TryGetItemAsync(fileName) is not StorageFile storageFile) storageFile = await thumbnail.SaveThumbnailAsync(StorageConstants.ImageCacheFolder, fileName, CreationCollisionOption.ReplaceExisting);
            return storageFile;
        }

        public static async Task<(uint, uint, uint)> LoadColorsFromFileAsync(StorageFile storageFile)
        {
            IEnumerable<Vector3> colors = (await ImageHelpers.GetColorClustersAsync(storageFile, ColorType.Neutral)).Take(3);
            return (colors.ElementAtOrDefault(0).ToUInt32(), colors.ElementAtOrDefault(1).ToUInt32(), colors.ElementAtOrDefault(2).ToUInt32());
        }

        public static async Task<(uint, uint, uint)> LoadColorsFromThumbnailAsync(StorageItemThumbnail thumbnail)
        {
            IEnumerable<Vector3> colors = (await ImageHelpers.GetColorClustersAsync(thumbnail, ColorType.Neutral)).Take(3);
            return (colors.ElementAtOrDefault(0).ToUInt32(), colors.ElementAtOrDefault(1).ToUInt32(), colors.ElementAtOrDefault(2).ToUInt32());
        }

        public async Task DeleteImageAsync()
        {
            if (await StorageFileHelpers.TryGetFileFromPathAsync(Path) is StorageFile storageFile) await storageFile.DeleteAsync();
            DBAccess.Images.Remove(this);
        }

        static ImageModel()
        {
            ImageCache = new ConcurrentBag<ImageModel>(DBAccess.Images);
        }

        public ImageModel()
        {

        }

        public ImageModel(string id)
        {
            ID = id;
            Path = default;
        }
    }
}