using ExtensionsLibrary.Extensions;
using MusicPlayerLibrary.Constants;
using MusicPlayerLibrary.Events;
using MusicPlayerLibrary.Helpers.Extensions;
using MusicPlayerLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Search;

namespace MusicPlayerLibrary.Helpers.StorageHelpers
{
    public static class StorageFolderHelpers
    {
        public static async Task<StorageFolder> TryGetFolderFromPathAsync(string path)
        {
            try
            {
                return await StorageFolder.GetFolderFromPathAsync(path);
            }
            catch
            {
                return null;
            }
        }

        public static async Task<StorageFolder> TryGetFolderFromPathAsync(string path, StorageItemAccessEvent storageItemAccessEvent)
        {
            try
            {
                storageItemAccessEvent.Invoke(true);
                return await StorageFolder.GetFolderFromPathAsync(path);
            }
            catch
            {
                storageItemAccessEvent.Invoke(false);
                return null;
            }
        }

        public static async Task<StorageFolder> CopyFolderAsync(this StorageFolder sourceFolder, StorageFolder destinationFolder)
        {
            if (sourceFolder != destinationFolder)
            {
                IEnumerable<IStorageItem> storageItems = await sourceFolder.CreateItemQueryWithOptions(new QueryOptions { FolderDepth = FolderDepth.Shallow }).GetItemsAsync();
                foreach (StorageFile storageFile in storageItems.Where(I => I is StorageFile)) await storageFile.CopyAsync(destinationFolder, storageFile.Name, NameCollisionOption.ReplaceExisting);
                foreach (StorageFolder storageFolder in storageItems.Where(I => I is StorageFolder)) await storageFolder.CopyFolderAsync(await destinationFolder.CreateFolderAsync(storageFolder.Name, CreationCollisionOption.OpenIfExists));
            }
            return destinationFolder;
        }

        public static async Task<IEnumerable<StorageFile>> GetSongFilesFromFolderAsync(StorageFolder storageFolder)
        {
            QueryOptions queryOptions = new QueryOptions { FolderDepth = FolderDepth.Deep };
            queryOptions.SetThumbnailPrefetch(Windows.Storage.FileProperties.ThumbnailMode.SingleItem, 1000, Windows.Storage.FileProperties.ThumbnailOptions.ResizeThumbnail);
            FileTypes.SongFileTypes.AsEnumerable().ForEach(Type => queryOptions.FileTypeFilter.Add(Type));
            StorageFileQueryResult storageFileQueryResult = storageFolder.CreateFileQueryWithOptions(queryOptions);
            return await storageFileQueryResult.GetFilesAsync();
        }

        public static async Task<List<StorageFile>> GetSongFilesFromFoldersAsync(IEnumerable<StorageFolder> storageFolders)
        {
            List<StorageFile> storageFiles = new List<StorageFile>();
            foreach (StorageFolder storageFolder in storageFolders) if (storageFolder != null) storageFiles.AddRange(await storageFolder.GetSongFilesAsync());
            return storageFiles.DistinctBy(F => F.Path).ToList();
        }

        public static async Task<StorageFolderModel> GetStorageFolderModelAsync()
        {
            FolderPicker folderPicker = new FolderPicker();
            folderPicker.FileTypeFilter.Add("*");
            folderPicker.SuggestedStartLocation = PickerLocationId.Desktop;
            if (await folderPicker.PickSingleFolderAsync() is StorageFolder storageFolder) return new StorageFolderModel(storageFolder);
            return null;
        }
    }
}
