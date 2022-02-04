using MusicPlayerLibrary.DataProperties;
using System.Threading.Tasks;
using Windows.Storage;

namespace MusicPlayerLibrary.Helpers.Extensions
{
    public static class StorageFileExtensions
    {
        public static async Task<SongProperties> GetSongFilePropertiesAsync(this StorageFile storageFile)
        {
            SongProperties songProperties = await SongProperties.LoadPropertiesAsync(storageFile);
            return songProperties;
        }

        //public static async Task<StorageFile> TryCopyAsync(this StorageFile storageFile, StorageFolder storageFolder)
        //{
        //    try
        //    {
        //        return await storageFile.CopyAsync(storageFolder);
        //    }
        //    catch (UnauthorizedAccessException)
        //    {
        //        throw;                
        //    }
        //    catch
        //    {
        //        return storageFile;
        //    }
        //}

        //public static async Task<StorageFile> TryCopyAsync(this StorageFile storageFile, StorageFolder storageFolder, string desiredNewName)
        //{
        //    try
        //    {
        //        return await storageFile.CopyAsync(storageFolder, desiredNewName);
        //    }
        //    catch (UnauthorizedAccessException)
        //    {
        //        throw;
        //    }
        //    catch
        //    {
        //        return storageFile;
        //    }
        //}

        //public static async Task<StorageFile> TryCopyAsync(this StorageFile storageFile, StorageFolder storageFolder, string desiredNewName, NameCollisionOption option)
        //{
        //    try
        //    {
        //        return await storageFile.CopyAsync(storageFolder, desiredNewName, option);
        //    }
        //    catch (UnauthorizedAccessException)
        //    {
        //        throw;
        //    }
        //    catch
        //    {
        //        return storageFile;
        //    }
        //}
    }
}