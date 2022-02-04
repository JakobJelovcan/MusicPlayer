using System;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.FileProperties;

namespace ExtensionsLibrary.Extensions
{
    public static class StorageItemThumbnailExtensions
    {
        public static async Task<StorageFile> SaveThumbnailAsync(this StorageItemThumbnail thumbnail, StorageFolder storageFolder, string fileName)
        {
            StorageFile storageFile = await storageFolder.CreateFileAsync(fileName);
            BitmapEncoder bitmapEncoder = await BitmapEncoder.CreateAsync(BitmapEncoder.PngEncoderId, await storageFile.OpenAsync(FileAccessMode.ReadWrite));
            bitmapEncoder.SetSoftwareBitmap(await (await BitmapDecoder.CreateAsync(thumbnail.CloneStream())).GetSoftwareBitmapAsync());
            await bitmapEncoder.FlushAsync();
            return storageFile;
        }

        public static async Task<StorageFile> SaveThumbnailAsync(this StorageItemThumbnail thumbnail, StorageFolder storageFolder, string fileName, CreationCollisionOption option)
        {
            StorageFile storageFile = await storageFolder.CreateFileAsync(fileName, option);
            BitmapEncoder bitmapEncoder = await BitmapEncoder.CreateAsync(BitmapEncoder.JpegEncoderId, await storageFile.OpenAsync(FileAccessMode.ReadWrite));
            bitmapEncoder.SetSoftwareBitmap(await (await BitmapDecoder.CreateAsync(thumbnail.CloneStream())).GetSoftwareBitmapAsync());
            await bitmapEncoder.FlushAsync();
            return storageFile;
        }
    }
}
