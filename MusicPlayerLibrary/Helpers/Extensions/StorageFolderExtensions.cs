using ExtensionsLibrary.Extensions;
using MusicPlayerLibrary.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Search;

namespace MusicPlayerLibrary.Helpers.Extensions
{
    public static class StorageFolderExtensions
    {
        public static async Task<IEnumerable<StorageFile>> GetSongFilesAsync(this StorageFolder storageFolder)
        {
            QueryOptions queryOptions = new QueryOptions { FolderDepth = FolderDepth.Deep };
            queryOptions.SetThumbnailPrefetch(Windows.Storage.FileProperties.ThumbnailMode.SingleItem, 1000, Windows.Storage.FileProperties.ThumbnailOptions.ResizeThumbnail);
            FileTypes.SongFileTypes.AsEnumerable().ForEach(Type => queryOptions.FileTypeFilter.Add(Type));
            StorageFileQueryResult storageFileQueryResult = storageFolder.CreateFileQueryWithOptions(queryOptions);
            return await storageFileQueryResult.GetFilesAsync();
        }
    }
}
