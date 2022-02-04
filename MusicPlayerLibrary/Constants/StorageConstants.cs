using System;
using System.Threading.Tasks;
using Windows.Storage;

namespace MusicPlayerLibrary.Constants
{
    public static class StorageConstants
    {
        public const string DBName = "sqLite.db";

        public static string DBPath => $"{LocalFolder.Path}\\{DBName}";

        public static StorageFolder LocalFolder => ApplicationData.Current.LocalFolder;

        public static StorageFolder ImageCacheFolder { get; private set; }

        public static StorageFolder LyricsCacheFolder { get; private set; }

        public static async Task LoadFolders()
        {
            ImageCacheFolder = await ApplicationData.Current.LocalFolder.CreateFolderAsync("ImageCache", CreationCollisionOption.OpenIfExists);
            LyricsCacheFolder = await ApplicationData.Current.LocalFolder.CreateFolderAsync("LyricsCache", CreationCollisionOption.OpenIfExists);
        }
    }
}
