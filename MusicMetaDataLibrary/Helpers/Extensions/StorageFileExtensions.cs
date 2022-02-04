using MusicMetaDataLibrary.ID3v2;
using System.Threading.Tasks;
using Windows.Storage;

namespace MusicMetaDataLibrary.Helpers.Extensions
{
    public static class StorageFileExtensions
    {
        public static async Task<ID3Tag> GetID3TagAsync(this StorageFile storageFile)
        {
            if (storageFile.FileType != ".mp3") return null;
            ID3Tag tag = new ID3Tag();
            await tag.LoadTagAsync(storageFile);
            return tag;
        }
    }
}
