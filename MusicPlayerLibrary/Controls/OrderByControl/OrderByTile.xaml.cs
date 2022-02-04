using MusicPlayerLibrary.Constants;
using MusicPlayerLibrary.MusicPlayer;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace MusicPlayerLibrary.Controls.OrderByControl
{
    public sealed partial class OrderByTile : UserControl
    {
        public OrderByTile()
        {
            InitializeComponent();
        }

        public MusicPlayerModel MusicPlayer
        {
            get => (MusicPlayerModel)GetValue(MusicPlayerProperty);
            set
            {
                SetValue(MusicPlayerProperty, value);
                UpdateButtons();
            }
        }
        public static readonly DependencyProperty MusicPlayerProperty = DependencyProperty.Register("MusicPlayer", typeof(MusicPlayerModel), typeof(OrderByTile), new PropertyMetadata(null));

        private void TitleOrderButton_Click(object sender, RoutedEventArgs e)
        {
            ChangeOrder(SongOrderType.Title);
        }

        private void AlbumOrderButton_Click(object sender, RoutedEventArgs e)
        {
            ChangeOrder(SongOrderType.Album);
        }

        private void ArtistOrderButton_Click(object sender, RoutedEventArgs e)
        {
            ChangeOrder(SongOrderType.Artist);
        }

        private void ChangeOrder(SongOrderType orderType)
        {
            MusicPlayer.UpdateSongOrder(orderType, (orderType == MusicPlayer.SongOrderType) ? ((MusicPlayer.SongOrder == Order.Ascending) ? Order.Descending : Order.Ascending ) : Order.Ascending);
            UpdateButtons();
        }

        private void UpdateButtons()
        {
            if (MusicPlayer?.SongOrderType == SongOrderType.Title) (TitleOrderButton.IsActive, TitleOrderButton.ItemOrder) = (true, MusicPlayer.SongOrder);
            else (TitleOrderButton.IsActive, TitleOrderButton.ItemOrder) = (false, Order.Ascending);
            if (MusicPlayer?.SongOrderType == SongOrderType.Album) (AlbumOrderButton.IsActive, AlbumOrderButton.ItemOrder) = (true, MusicPlayer.SongOrder);
            else (AlbumOrderButton.IsActive, AlbumOrderButton.ItemOrder) = (false, Order.Ascending);
            if (MusicPlayer?.SongOrderType == SongOrderType.Artist) (ArtistOrderButton.IsActive, ArtistOrderButton.ItemOrder) = (true, MusicPlayer.SongOrder);
            else (ArtistOrderButton.IsActive, ArtistOrderButton.ItemOrder) = (false, Order.Ascending);
        }

        private void OrderByTile_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            if (args.NewValue is MusicPlayerModel musicPlayer) MusicPlayer = musicPlayer;
        }
    }
}