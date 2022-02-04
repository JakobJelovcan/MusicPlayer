using MusicPlayerLibrary.Constants;
using MusicPlayerLibrary.Data.Settings;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace MusicPlayerLibrary.TemplateSelectors
{
    public class ArtistTemplateSelector : DataTemplateSelector
    {
        public DataTemplate ArtistTileTall { get; set; }

        public DataTemplate ArtistTileWide { get; set; }

        protected override DataTemplate SelectTemplateCore(object item)
        {
            switch (Settings.ArtistTileStyle)
            {
                case ArtistTile.Tall: return ArtistTileTall;
                case ArtistTile.Wide: return ArtistTileWide;
                default: return base.SelectTemplateCore(item);
            }
        }
    }
}
