using ExtensionsLibrary.Extensions;
using MusicPlayerLibrary.Data.DataBase;
using MusicPlayerLibrary.Interfaces;
using System.Collections.Concurrent;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace MusicPlayerLibrary.Models
{
    public class GenreModel : IEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string ID { get; set; }

        public string GenreName { get; set; }

        public int TimesPlayed { get; set; }
        private static readonly ConcurrentBag<GenreModel> GenreCache;

        private static string FormatGenreName(string genreName)
        {
            return (string.IsNullOrWhiteSpace(genreName)) ? genreName : genreName.Trim().Replace("-", " ").Replace("/", " ").Clean().Split(';').FirstOrDefault();
        }

        public override bool Equals(object obj)
        {
            if (obj is GenreModel genre) return GenreName == genre.GenreName;
            return base.Equals(obj);
        }

        public static GenreModel GetOrCreateGenre(string genreName)
        {
            genreName = FormatGenreName(genreName);
            return string.IsNullOrWhiteSpace(genreName) ? null : GenreCache.FirstOrDefault(G => G.GenreName == genreName) ?? GenreCache.AddAndReturn(new GenreModel(genreName));
        }

        static GenreModel()
        {
            GenreCache = new ConcurrentBag<GenreModel>(DBAccess.Genres);
        }

        public GenreModel()
        {

        }

        public GenreModel(string genreName)
        {
            GenreName = genreName;
            TimesPlayed = default;
        }
    }
}