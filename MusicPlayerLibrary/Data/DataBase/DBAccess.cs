using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using MusicPlayerLibrary.Lyrics;
using MusicPlayerLibrary.Models;
using System.Threading.Tasks;

namespace MusicPlayerLibrary.Data.DataBase
{
    public static class DBAccess
    {
        private static readonly MusicPlayerDBContext db;

        public static void EnsureCreated()
        {
            ((RelationalDatabaseCreator)db.Database.GetService<IDatabaseCreator>()).EnsureCreated();
        }

        public static DbSet<SongModel> Songs => db.Songs;

        public static DbSet<AlbumModel> Albums => db.Albums;

        public static DbSet<ArtistModel> Artists => db.Artists;

        public static DbSet<PlaylistModel> Playlists => db.Playlists;

        public static DbSet<GenreModel> Genres => db.Genres;

        public static DbSet<ImageModel> Images => db.Images;

        public static DbSet<LyricsModel> Lyrics => db.Lyrics;

        public static DbSet<StorageFolderModel> StorageFolders => db.StorageFolders;

        public static void SaveChanges()
        {
            db.SaveChanges();
        }

        public static async Task SaveChangesAsync()
        {
            await db.SaveChangesAsync();
        }

        static DBAccess()
        {
            db = new MusicPlayerDBContext();
            EnsureCreated();
        }
    }
}