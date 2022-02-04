using MusicPlayerLibrary.Constants;
using MusicPlayerLibrary.Data.Settings;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace MusicPlayerLibrary.TemplateSelectors
{
    public class PlaylistTemplateSelector : DataTemplateSelector
    {
        public DataTemplate PlaylistTileTall { get; set; }

        public DataTemplate PlaylistTileWide { get; set; }

        protected override DataTemplate SelectTemplateCore(object item)
        {
            switch (Settings.PlaylistTileStyle)
            {
                case PlaylistTile.Tall: return PlaylistTileTall;
                case PlaylistTile.Wide: return PlaylistTileWide;
                default: return base.SelectTemplateCore(item);
            }
        }
    }
}
