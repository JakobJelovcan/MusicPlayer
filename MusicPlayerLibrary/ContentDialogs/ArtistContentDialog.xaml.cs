using MusicPlayerLibrary.Data.DataBase;
using MusicPlayerLibrary.Helpers.StorageHelpers;
using MusicPlayerLibrary.Models;
using MusicPlayerLibrary.MusicPlayer;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace MusicPlayerLibrary.ContentDialogs
{
    public sealed partial class ArtistContentDialog : ContentDialog
    {
        public ArtistContentDialog(ArtistModel artist)
        {
            InitializeComponent();
            MusicPlayer = artist.GetMusicPlayer();
            Artist = artist;
            Image = artist.Image;
            LargeImage = artist.LargeImage;
        }

        public MusicPlayerModel MusicPlayer { get; private set; }

        public ArtistModel Artist
        {
            get => (ArtistModel)GetValue(ArtistProperty);
            set => SetValue(ArtistProperty, value);
        }
        public static readonly DependencyProperty ArtistProperty = DependencyProperty.Register("Artist", typeof(ArtistModel), typeof(ArtistContentDialog), new PropertyMetadata(null));

        public ImageModel Image
        {
            get => (ImageModel)GetValue(ImageProperty);
            set => SetValue(ImageProperty, value);
        }
        public static readonly DependencyProperty ImageProperty = DependencyProperty.Register("Image", typeof(ImageModel), typeof(ArtistContentDialog), new PropertyMetadata(null));

        public ImageModel LargeImage
        {
            get => (ImageModel)GetValue(LargeImageProperty);
            set => SetValue(LargeImageProperty, value);
        }
        public static readonly DependencyProperty LargeImageProperty = DependencyProperty.Register("LargeImage", typeof(ImageModel), typeof(ArtistContentDialog), new PropertyMetadata(null));

        private void Confirm_ButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            Artist.Image = Image ?? Artist.Image;
            Artist.LargeImage = LargeImage ?? Artist.LargeImage;
            DBAccess.SaveChanges();
            Hide();
        }

        private void Cancel_ButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            Hide();
        }

        private async void PickALargeImageButton_Click(object sender, RoutedEventArgs e)
        {
            LargeImage = await StorageFileHelpers.PickAndSaveImageAsync();
        }

        private async void PickAnImageButton_Click(object sender, RoutedEventArgs e)
        {
            Image = await StorageFileHelpers.PickAndSaveImageAsync();
        }
    }
}
