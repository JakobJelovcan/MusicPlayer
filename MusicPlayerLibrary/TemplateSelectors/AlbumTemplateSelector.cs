using MusicPlayerLibrary.Constants;
using MusicPlayerLibrary.Data.Settings;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace MusicPlayerLibrary.TemplateSelectors
{
    public class AlbumTemplateSelector : DataTemplateSelector
    {
        public DataTemplate AlbumTileTall { get; set; }

        public DataTemplate AlbumTileWide { get; set; }

        protected override DataTemplate SelectTemplateCore(object item)
        {
            switch (Settings.AlbumTileStyle)
            {
                case AlbumTile.Tall: return AlbumTileTall;
                case AlbumTile.Wide: return AlbumTileWide;
                default: return base.SelectTemplateCore(item);
            }
        }
    }
}
