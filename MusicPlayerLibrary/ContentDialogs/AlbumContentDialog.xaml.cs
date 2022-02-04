using ExtensionsLibrary.Extensions;
using Microsoft.UI.Xaml.Controls;
using MusicPlayerLibrary.Constants;
using MusicPlayerLibrary.Data.DataBase;
using MusicPlayerLibrary.Helpers.Extensions;
using MusicPlayerLibrary.Helpers.StorageHelpers;
using MusicPlayerLibrary.Models;
using MusicPlayerLibrary.MusicPlayer;
using MusicPlayerLibrary.ViewModels;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace MusicPlayerLibrary.ContentDialogs
{
    public sealed partial class AlbumContentDialog : ContentDialog
    {
        public AlbumContentDialog(AlbumModel album)
        {
            InitializeComponent();
            MusicPlayer = album.GetMusicPlayer();
            Album = album;
            AlbumName = album.Album;
            Image = album.Image;
            Year = album.Year.ToString();
            Songs = album.Songs.Select(S => new SongViewModel(S)).ToObservableCollection();
            ShowMoreButtonVisibility = Songs.Count > 1 ? Visibility.Visible : Visibility.Collapsed;
        }

        public MusicPlayerModel MusicPlayer { get; private set; }
        private bool YearOK;
        private bool AlbumOK;

        public AlbumModel Album
        {
            get => (AlbumModel)GetValue(AlbumProperty);
            set => SetValue(AlbumProperty, value);
        }
        public static readonly DependencyProperty AlbumProperty = DependencyProperty.Register("Album", typeof(AlbumModel), typeof(AlbumContentDialog), new PropertyMetadata(null));

        public string AlbumName
        {
            get => (string)GetValue(AlbumNameProperty);
            set
            {
                SetValue(AlbumNameProperty, value);
                VerifyAlbum();
            }
        }
        public static readonly DependencyProperty AlbumNameProperty = DependencyProperty.Register("AlbumName", typeof(string), typeof(AlbumContentDialog), new PropertyMetadata(string.Empty));

        public ImageModel Image
        {
            get => (ImageModel)GetValue(ImageProperty);
            set => SetValue(ImageProperty, value);
        }
        public static readonly DependencyProperty ImageProperty = DependencyProperty.Register("Image", typeof(ImageModel), typeof(AlbumContentDialog), new PropertyMetadata(null));

        public string Year
        {
            get => (string)GetValue(YearProperty);
            set
            {
                SetValue(YearProperty, value);
                VerifyYear();
            }
        }
        public static readonly DependencyProperty YearProperty = DependencyProperty.Register("Year", typeof(string), typeof(AlbumContentDialog), new PropertyMetadata(string.Empty));

        private ObservableCollection<SongViewModel> Songs
        {
            get => (ObservableCollection<SongViewModel>)GetValue(SongsProperty);
            set
            {
                Songs?.UnregisterPropertyChanged(Songs_CollectionChanged);
                SetValue(SongsProperty, value);
                Songs?.RegisterPropertyChanged(Songs_CollectionChanged);
            }
        }
        private static readonly DependencyProperty SongsProperty = DependencyProperty.Register("Songs", typeof(ObservableCollection<SongModel>), typeof(AlbumContentDialog), new PropertyMetadata(null));

        public bool ShowMore
        {
            get => (bool)GetValue(ShowMoreProperty);
            set => SetValue(ShowMoreProperty, value);
        }
        public static readonly DependencyProperty ShowMoreProperty = DependencyProperty.Register("ShowMore", typeof(bool), typeof(AlbumContentDialog), new PropertyMetadata(false));

        public Visibility ShowMoreButtonVisibility
        {
            get => (Visibility)GetValue(ShowMoreButtonVisibilityProperty);
            set => SetValue(ShowMoreButtonVisibilityProperty, value);
        }
        public static readonly DependencyProperty ShowMoreButtonVisibilityProperty = DependencyProperty.Register("ShowMoreButtonVisibility", typeof(Visibility), typeof(AlbumContentDialog), new PropertyMetadata(Visibility.Collapsed));

        public bool SaveChangesToSongs
        {
            get => (bool)GetValue(SaveChangesToSongsProperty);
            set => SetValue(SaveChangesToSongsProperty, value);
        }
        public static readonly DependencyProperty SaveChangesToSongsProperty = DependencyProperty.Register("SaveChangesToSongs", typeof(bool), typeof(AlbumContentDialog), new PropertyMetadata(false));

        public bool SaveChangesToFiles
        {
            get => (bool)GetValue(SaveChangesToFilesProperty);
            set => SetValue(SaveChangesToFilesProperty, value);
        }
        public static readonly DependencyProperty SaveChangesToFilesProperty = DependencyProperty.Register("SaveChangesToFiles", typeof(bool), typeof(AlbumContentDialog), new PropertyMetadata(false));

        public string Genres => string.Join("; ", Album?.Genres.Select(G => G?.GenreName).Distinct());

        private void VerifyYear()
        {
            if (string.IsNullOrWhiteSpace(Year))
            {
                (YearMessageTextBox.IsOpen, YearMessageTextBox.IsClosable, YearMessageTextBox.Message, YearMessageTextBox.Severity) = (true, false, "Year cannot be empty!", InfoBarSeverity.Error);
                YearOK = false;
            }
            else if (!Year.IsInt32())
            {
                (YearMessageTextBox.IsOpen, YearMessageTextBox.IsClosable, YearMessageTextBox.Message, YearMessageTextBox.Severity) = (true, false, "Year has to be a number!", InfoBarSeverity.Error);
                YearOK = false;
            }
            else
            {
                (YearMessageTextBox.IsOpen, YearMessageTextBox.IsClosable, YearMessageTextBox.Message, YearMessageTextBox.Severity) = (false, false, string.Empty, InfoBarSeverity.Informational);
                YearOK = true;
            }
            IsPrimaryButtonEnabled = YearOK & AlbumOK;
        }

        private void VerifyAlbum()
        {
            if (string.IsNullOrWhiteSpace(AlbumName))
            {
                (AlbumMessageTextBox.IsOpen, AlbumMessageTextBox.IsClosable, AlbumMessageTextBox.Message, AlbumMessageTextBox.Severity) = (true, false, "Album cannot be empty!", InfoBarSeverity.Error);
                AlbumOK = false;
            }
            else
            {
                (AlbumMessageTextBox.IsOpen, AlbumMessageTextBox.IsClosable, AlbumMessageTextBox.Message, AlbumMessageTextBox.Severity) = (false, false, string.Empty, InfoBarSeverity.Informational);
                AlbumOK = true;
            }
            IsPrimaryButtonEnabled = AlbumOK & YearOK;
        }

        private void Songs_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add) for (int i = 0; i < Songs.Count; i++) Songs[i].Track = (uint)i + 1;
        }

        private void Confirm_ButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            Album.Image = Image ?? Album.Image;
            Album.Year = int.Parse(Year);
            ChangeAlbum();
            Songs.ForEach(S => S.SaveChanges());
            Album?.Songs?.ToArray()?.ForEach(S => { S.Year = Album.Year; S.SongUpdated(SongUpdateParamater.Track); });
            if (SaveChangesToSongs) Album?.Songs.ForEach(S => S.Image = Album.Image);
            if (SaveChangesToFiles) Album?.Songs.ForEach(async S => await S.SaveChangesToFileAsync());
            DBAccess.SaveChanges();
            Hide();
        }

        private void Cancel_ButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            Hide();
        }

        private async void PickAnImageButton_Click(object sender, RoutedEventArgs e)
        {
            Image = await StorageFileHelpers.PickAndSaveImageAsync();
        }

        private void ChangeAlbum()
        {
            if (Album.Album != AlbumName)
            {
                Album.Songs.ToArray().ChangeAlbum(AlbumName);
                DBAccess.SaveChanges();
            }
        }

        private void ContentDisplayButton_ContentDisplayChanged(ContentDisplay newValue)
        {
            ShowMore = newValue == ContentDisplay.More;
        }
    }
}