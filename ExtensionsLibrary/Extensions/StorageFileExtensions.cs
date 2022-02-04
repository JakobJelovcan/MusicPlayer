using System;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;

namespace ExtensionsLibrary.Extensions
{
    public static class StorageFileExtensions
    {
        public static async Task<StorageFile> TryCopyAsync(this StorageFile storageFile, StorageFolder storageFolder)
        {
            try
            {
                return await storageFile.CopyAsync(storageFolder);
            }
            catch (UnauthorizedAccessException)
            {
                throw;
            }
            catch
            {
                return storageFile;
            }
        }

        public static async Task<StorageFile> TryCopyAsync(this StorageFile storageFile, StorageFolder storageFolder, string desiredNewName)
        {
            try
            {
                return await storageFile.CopyAsync(storageFolder, desiredNewName);
            }
            catch (UnauthorizedAccessException)
            {
                throw;
            }
            catch
            {
                return storageFile;
            }
        }

        public static async Task<StorageFile> TryCopyAsync(this StorageFile storageFile, StorageFolder storageFolder, string desiredNewName, NameCollisionOption option)
        {
            try
            {
                return await storageFile.CopyAsync(storageFolder, desiredNewName, option);
            }
            catch (UnauthorizedAccessException)
            {
                throw;
            }
            catch
            {
                return storageFile;
            }
        }

        public static async Task<byte[]> ToByteArrayAsync(this StorageFile storageFile)
        {
            using (IRandomAccessStream stream = await storageFile.OpenAsync(FileAccessMode.Read))
            {
                using (DataReader dataReader = new DataReader(stream))
                {
                    byte[] buffer = new byte[stream.Size];
                    await dataReader.LoadAsync((uint)stream.Size);
                    dataReader.ReadBytes(buffer);
                    return buffer;
                }
            }
        }
    }
}