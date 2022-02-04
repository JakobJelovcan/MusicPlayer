using ExtensionsLibrary.Extensions;
using Microsoft.UI.Xaml.Controls;
using MusicPlayerLibrary.Constants;
using MusicPlayerLibrary.Data.DataBase;
using MusicPlayerLibrary.Helpers.StorageHelpers;
using MusicPlayerLibrary.Lyrics;
using MusicPlayerLibrary.Models;
using MusicPlayerLibrary.MusicPlayer;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace MusicPlayerLibrary.ContentDialogs
{
    public sealed partial class SongContentDialog : ContentDialog
    {
        public SongContentDialog(SongModel song)
        {
            InitializeComponent();
            MusicPlayer = song.GetMusicPlayer();
            Song = song;
            Image = song.Image;
            LargeImage = song.LargeImage;
            Lyrics = song.Lyrics;
            SongTitle = song.Title;
            Album = song.Album;
            Genre = song?.Genre?.GenreName;
            Duration = song.Duration.ToFormatedString();
            Writers = song.Writers;
            Composers = song.Composers;
            Rating = (int)Song.Rating;
        }
        public MusicPlayerModel MusicPlayer;
        private bool TitleOK;
        private bool AlbumOK;

        public SongModel Song
        {
            get => (SongModel)GetValue(SongProperty);
            set => SetValue(SongProperty, value);
        }
        public static readonly DependencyProperty SongProperty = DependencyProperty.Register("Song", typeof(SongModel), typeof(SongContentDialog), new PropertyMetadata(null));

        public string Album
        {
            get => (string)GetValue(AlbumProperty);
            set
            {
                SetValue(AlbumProperty, value);
                VerifyAlbum();
            }
        }
        public static readonly DependencyProperty AlbumProperty = DependencyProperty.Register("Album", typeof(string), typeof(SongContentDialog), new PropertyMetadata(string.Empty));

        public string SongTitle
        {
            get => (string)GetValue(SongTitleProperty);
            set
            {
                SetValue(SongTitleProperty, value);
                VerifyTitle();
            }
        }
        public static readonly DependencyProperty SongTitleProperty = DependencyProperty.Register("SongTitle", typeof(string), typeof(SongContentDialog), new PropertyMetadata(string.Empty));

        public string Duration
        {
            get => (string)GetValue(DurationProperty);
            set => SetValue(DurationProperty, value);
        }
        public static readonly DependencyProperty DurationProperty = DependencyProperty.Register("Duration", typeof(string), typeof(SongContentDialog), new PropertyMetadata(string.Empty));

        public string Genre
        {
            get => (string)GetValue(GenreProperty);
            set => SetValue(GenreProperty, value);
        }
        public static readonly DependencyProperty GenreProperty = DependencyProperty.Register("Genre", typeof(string), typeof(SongContentDialog), new PropertyMetadata(string.Empty));

        public string Writers
        {
            get => (string)GetValue(WritersProperty);
            set => SetValue(WritersProperty, value);
        }
        public static readonly DependencyProperty WritersProperty = DependencyProperty.Register("Writers", typeof(string), typeof(SongContentDialog), new PropertyMetadata(string.Empty));

        public string Composers
        {
            get => (string)GetValue(ComposersProperty);
            set => SetValue(ComposersProperty, value);
        }
        public static readonly DependencyProperty ComposersProperty = DependencyProperty.Register("Composers", typeof(string), typeof(SongContentDialog), new PropertyMetadata(string.Empty));

        public ImageModel Image
        {
            get => (ImageModel)GetValue(ImageProperty);
            set => SetValue(ImageProperty, value);
        }
        public static readonly DependencyProperty ImageProperty = DependencyProperty.Register("Image", typeof(ImageModel), typeof(SongContentDialog), new PropertyMetadata(null));

        public ImageModel LargeImage
        {
            get => (ImageModel)GetValue(LargeImageProperty);
            set => SetValue(LargeImageProperty, value);
        }
        public static readonly DependencyProperty LargeImageProperty = DependencyProperty.Register("LargeImage", typeof(ImageModel), typeof(SongContentDialog), new PropertyMetadata(null));

        public LyricsModel Lyrics
        {
            get => (LyricsModel)GetValue(LyricsProperty);
            set
            {
                SetValue(LyricsProperty, value);
                LyricsPath = value?.Path;
                IsClearLyricsEnabled = value is not null;
            }
        }
        public static readonly DependencyProperty LyricsProperty = DependencyProperty.Register("Lyrics", typeof(LyricsModel), typeof(SongContentDialog), new PropertyMetadata(null));

        public string LyricsPath
        {
            get => (string)GetValue(LyricsPathProperty);
            set => SetValue(LyricsPathProperty, value);
        }
        public static readonly DependencyProperty LyricsPathProperty = DependencyProperty.Register("LyricsPath", typeof(string), typeof(SongContentDialog), new PropertyMetadata(string.Empty));

        public bool IsClearLyricsEnabled
        {
            get => (bool)GetValue(IsClearLyricsEnabledProperty);
            set => SetValue(IsClearLyricsEnabledProperty, value);
        }
        public static readonly DependencyProperty IsClearLyricsEnabledProperty = DependencyProperty.Register("IsClearLyricsEnabled", typeof(bool), typeof(SongContentDialog), new PropertyMetadata(false));

        public int Rating
        {
            get => (int)GetValue(RatingProperty);
            set => SetValue(RatingProperty, value);
        }
        public static readonly DependencyProperty RatingProperty = DependencyProperty.Register("Rating", typeof(int), typeof(SongContentDialog), new PropertyMetadata(0));

        public bool SaveChangesToSongFiles
        {
            get => (bool)GetValue(SaveChangesToSongFilesProperty);
            set => SetValue(SaveChangesToSongFilesProperty, value);
        }
        public static readonly DependencyProperty SaveChangesToSongFilesProperty = DependencyProperty.Register("SaveChangesToSongFiles", typeof(bool), typeof(SongContentDialog), new PropertyMetadata(false));

        private SongUpdateParamater GetSongUpdateParamater()
        {
            return SongUpdateParamater.None | ((Song.Title != SongTitle) ? SongUpdateParamater.Title : SongUpdateParamater.None);
        }

        private void VerifyTitle()
        {
            if (string.IsNullOrWhiteSpace(SongTitle))
            {
                (TitleMessageTextBox.IsOpen, TitleMessageTextBox.IsClosable, TitleMessageTextBox.Message, TitleMessageTextBox.Severity) = (true, false, "Title cannot be empty!", InfoBarSeverity.Error);
                TitleOK = false;
            }
            else
            {
                (TitleMessageTextBox.IsOpen, TitleMessageTextBox.IsClosable, TitleMessageTextBox.Message, TitleMessageTextBox.Severity) = (false, false, string.Empty, InfoBarSeverity.Informational);
                TitleOK = true;
            }
            IsPrimaryButtonEnabled = TitleOK & AlbumOK;
        }

        private void VerifyAlbum()
        {
            if (string.IsNullOrWhiteSpace(Album))
            {
                (AlbumMessageTextBox.IsOpen, AlbumMessageTextBox.IsClosable, AlbumMessageTextBox.Message, AlbumMessageTextBox.Severity) = (true, false, "Album cannot be empty!", InfoBarSeverity.Error);
                AlbumOK = false;
            }
            else
            {
                (AlbumMessageTextBox.IsOpen, AlbumMessageTextBox.IsClosable, AlbumMessageTextBox.Message, AlbumMessageTextBox.Severity) = (false, false, string.Empty, InfoBarSeverity.Informational);
                AlbumOK = true;
            }
            IsPrimaryButtonEnabled = TitleOK & AlbumOK;
        }

        private async void Confirm_ButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            SongUpdateParamater songUpdateParamater = GetSongUpdateParamater();
            ChangeAlbum();
            Song.Title = SongTitle;
            Song.Genre = GenreModel.GetOrCreateGenre(Genre) ?? Song.Genre;
            Song.Image = Image ?? Song.Image;
            Song.LargeImage = LargeImage ?? Song.LargeImage;
            Song.Lyrics = Lyrics;
            Song.Writers = Writers;
            Song.Composers = Composers;
            Song.Rating = (uint)Rating;
            Song.SongUpdated(songUpdateParamater);
            Hide();
            if (SaveChangesToSongFiles) await Song.SaveChangesToFileAsync();
            await DBAccess.SaveChangesAsync();
        }

        private void Cancel_ButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            Hide();
        }

        private async void PickAnImageButton_Click(object sender, RoutedEventArgs e)
        {
            Image = await StorageFileHelpers.PickAndSaveImageAsync();
        }

        private async void PickALargeImageButton_Click(object sender, RoutedEventArgs e)
        {
            LargeImage = await StorageFileHelpers.PickAndSaveImageAsync();
        }

        private async void PickLyricsButton_Click(object sender, RoutedEventArgs e)
        {
            Lyrics = await StorageFileHelpers.PickAndSaveLyricsAsync();
        }

        private void RemoveLyricsButton_Click(object sender, RoutedEventArgs e)
        {
            Lyrics = null;
        }

        private void ChangeAlbum()
        {
            if (Song.Album != Album)
            {
                Song.ChangeAlbum(Album);
                DBAccess.SaveChanges();
            }
        }
    }
}