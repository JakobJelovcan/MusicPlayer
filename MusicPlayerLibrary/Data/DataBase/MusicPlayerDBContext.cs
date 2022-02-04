using Microsoft.EntityFrameworkCore;
using MusicPlayerLibrary.Constants;
using MusicPlayerLibrary.Data.JoinLinks;
using MusicPlayerLibrary.Lyrics;
using MusicPlayerLibrary.Models;

namespace MusicPlayerLibrary.Data.DataBase
{
    internal class MusicPlayerDBContext : DbContext
    {
        public DbSet<SongModel> Songs { get; set; }

        public DbSet<AlbumModel> Albums { get; set; }

        public DbSet<ArtistModel> Artists { get; set; }

        public DbSet<PlaylistModel> Playlists { get; set; }

        public DbSet<GenreModel> Genres { get; set; }

        public DbSet<ImageModel> Images { get; set; }

        public DbSet<LyricsModel> Lyrics { get; set; }

        public DbSet<StorageFolderModel> StorageFolders { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Filename={StorageConstants.DBName}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PlaylistSongLink>().HasKey(PSL => new { PSL.SongID, PSL.PlaylistID });
            modelBuilder.Entity<PlaylistSongLink>().HasOne(PSL => PSL.Playlist).WithMany(P => P.SongLinks).HasForeignKey(P => P.PlaylistID);
            modelBuilder.Entity<BaseMusicModel>().HasOne(I => I.Image).WithMany(I => I.SmallImageReferences);
            modelBuilder.Entity<BaseMusicModel>().HasOne(I => I.LargeImage).WithMany(I => I.LargeImageReferences);
            modelBuilder.Entity<PlaylistSongLink>().HasOne(L => L.Song).WithMany(S => S.PlaylistSongLinks);
            modelBuilder.Entity<LyricsModel>().HasMany(L => L.LyricsReferences).WithOne(S => S.Lyrics);
        }
    }
}