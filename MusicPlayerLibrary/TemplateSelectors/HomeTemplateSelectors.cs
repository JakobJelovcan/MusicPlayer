using MusicPlayerLibrary.Models;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace MusicPlayerLibrary.TemplateSelectors
{
    public class HomeTemplateSelectors : DataTemplateSelector
    {
        public DataTemplate AlbumTileTall { get; set; }

        public DataTemplate ArtistTileTall { get; set; }

        public DataTemplate PlaylistTileTall { get; set; }

        protected override DataTemplate SelectTemplateCore(object item)
        {
            switch (item)
            {
                case AlbumModel: return AlbumTileTall;
                case ArtistModel: return ArtistTileTall;
                case PlaylistModel: return PlaylistTileTall;
                default: return base.SelectTemplateCore(item);
            }
        }
    }
}
