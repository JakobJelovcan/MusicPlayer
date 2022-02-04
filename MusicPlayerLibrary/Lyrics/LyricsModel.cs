using ExtensionsLibrary.Extensions;
using MusicPlayerLibrary.Constants;
using MusicPlayerLibrary.Data.DataBase;
using MusicPlayerLibrary.Helpers.StorageHelpers;
using MusicPlayerLibrary.Interfaces;
using MusicPlayerLibrary.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;

namespace MusicPlayerLibrary.Lyrics
{
    public class LyricsModel : IEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string ID { get; set; }

        public string Path { get; set; }

        public IEnumerable<SongModel> LyricsReferences { get; private set; }

        public static async Task<LyricsModel> GetOrCreateLyricsFromFileAsync(StorageFile storageFile)
        {
            if (DBAccess.Lyrics.FirstOrDefault(L => L.Path == storageFile.Path) is LyricsModel lyricsModel) return lyricsModel;
            else
            {
                string path = (await storageFile.TryCopyAsync(StorageConstants.LyricsCacheFolder, storageFile.Name, NameCollisionOption.ReplaceExisting)).Path;
                return (path != null) ? new LyricsModel(path) : null;
            }
        }

        public async Task DeleteLyricsAsync()
        {
            if (await StorageFileHelpers.TryGetFileFromPathAsync(Path) is StorageFile storageFile) await storageFile.DeleteAsync();
            DBAccess.Lyrics.Remove(this);
        }

        public LyricsModel()
        {

        }

        public LyricsModel(string path)
        {
            Path = path;
        }
    }
}