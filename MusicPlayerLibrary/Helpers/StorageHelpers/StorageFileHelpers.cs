using ExtensionsLibrary.Extensions;
using MusicPlayerLibrary.Events;
using MusicPlayerLibrary.Lyrics;
using MusicPlayerLibrary.Models;
using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;

namespace MusicPlayerLibrary.Helpers.StorageHelpers
{
    public static class StorageFileHelpers
    {
        public static async Task<StorageFile> TryGetFileFromPathAsync(string path)
        {
            try
            {
                return await StorageFile.GetFileFromPathAsync(path);
            }
            catch
            {
                return null;
            }
        }

        public static async Task<StorageFile> TryGetFileFromPathAsync(string path, StorageItemAccessEvent storageItemAccessEvent)
        {
            try
            {
                storageItemAccessEvent.Invoke(true);
                return await StorageFile.GetFileFromPathAsync(path);
            }
            catch
            {
                storageItemAccessEvent.Invoke(false);
                return null;
            }
        }

        public static async Task<StorageFile> TryGetFileFromApplicationURIAsync(Uri uri, StorageItemAccessEvent storageItemAccessEvent)
        {
            try
            {
                storageItemAccessEvent.Invoke(true);
                return await StorageFile.GetFileFromApplicationUriAsync(uri);
            }
            catch
            {
                storageItemAccessEvent.Invoke(false);
                return null;
            }
        }

        public static async Task<ImageModel> PickAndSaveImageAsync()
        {
            FileOpenPicker fileOpenPicker = new FileOpenPicker();
            fileOpenPicker.FileTypeFilter.Add(".png");
            fileOpenPicker.FileTypeFilter.Add(".jpg");
            fileOpenPicker.FileTypeFilter.Add(".jpeg");
            fileOpenPicker.SuggestedStartLocation = PickerLocationId.ComputerFolder;
            if (await fileOpenPicker.PickSingleFileAsync() is StorageFile imageFile) return await ImageModel.GetOrCreateImageFromFileAsync(imageFile.DisplayName.ToMD5(), imageFile);
            return null;
        }

        public static async Task<LyricsModel> PickAndSaveLyricsAsync()
        {
            FileOpenPicker fileOpenPicker = new FileOpenPicker();
            fileOpenPicker.FileTypeFilter.Add(".txt");
            fileOpenPicker.FileTypeFilter.Add(".lrc");
            fileOpenPicker.FileTypeFilter.Add(".lrcx");
            fileOpenPicker.SuggestedStartLocation = PickerLocationId.ComputerFolder;
            if (await fileOpenPicker.PickSingleFileAsync() is StorageFile storageFile) return await LyricsModel.GetOrCreateLyricsFromFileAsync(storageFile);
            return null;
        }

        public static bool Exists(string path)
        {
            return new FileInfo(path).Exists;
        }
    }
}
