using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.UI.Xaml.Controls;
using MusicPlayerLibrary.Constants;
using MusicPlayerLibrary.Data.DataBase;
using MusicPlayerLibrary.Helpers.StorageHelpers;
using MusicPlayerLibrary.Models;
using MusicPlayerLibrary.MusicPlayer;
using System.Linq;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace MusicPlayerLibrary.ContentDialogs
{
    public sealed partial class PlaylistContentDialog : ContentDialog
    {
        public PlaylistContentDialog(PlaylistModel playlist, PlaylistDialogTask playlistDialogTask)
        {
            InitializeComponent();
            MusicPlayer = playlist?.Parent;
            Playlist = playlist;
            PlaylistName = playlist?.Playlist ?? string.Empty;
            Image = playlist?.Image;
            DialogTitle = playlistDialogTask == PlaylistDialogTask.Create ? ResourceLoader.GetForViewIndependentUse("MusicPlayerLibrary/PlaylistDialogResources").GetString("Create_Title/Text") : ResourceLoader.GetForCurrentView("MusicPlayerLibrary/PlaylistDialogResources").GetString("Edit_Title/Text");
        }

        public MusicPlayerModel MusicPlayer;

        public PlaylistModel Playlist
        {
            get => (PlaylistModel)GetValue(PlaylistProperty);
            set => SetValue(PlaylistProperty, value);
        }
        public static readonly DependencyProperty PlaylistProperty = DependencyProperty.Register("Playlist", typeof(PlaylistModel), typeof(PlaylistContentDialog), new PropertyMetadata(null));

        public string DialogTitle
        {
            get => (string)GetValue(DialogTitleProperty);
            set => SetValue(DialogTitleProperty, value);
        }
        public static readonly DependencyProperty DialogTitleProperty = DependencyProperty.Register("DialogTitle", typeof(string), typeof(PlaylistContentDialog), new PropertyMetadata(string.Empty));

        public string PlaylistName
        {
            get => (string)GetValue(PlaylistNameProperty);
            set
            {
                SetValue(PlaylistNameProperty, value);
                ValidateName();
            }
        }
        public static readonly DependencyProperty PlaylistNameProperty = DependencyProperty.Register("PlaylistName", typeof(string), typeof(PlaylistContentDialog), new PropertyMetadata(string.Empty));

        public ImageModel Image
        {
            get => (ImageModel)GetValue(ImageProperty);
            set => SetValue(ImageProperty, value);
        }
        public static readonly DependencyProperty ImageProperty = DependencyProperty.Register("Image", typeof(ImageModel), typeof(PlaylistContentDialog), new PropertyMetadata(null));

        private void ValidateName()
        {
            if (string.IsNullOrWhiteSpace(PlaylistName))
            {
                (PlaylistNameMessageTextBox.IsOpen, PlaylistNameMessageTextBox.IsClosable, PlaylistNameMessageTextBox.Message, PlaylistNameMessageTextBox.Severity) = (true, false, "The name can't be empty!", InfoBarSeverity.Error);
                IsPrimaryButtonEnabled = false;
            }
            else if (MusicPlayer?.Playlists?.Any(P => P.Playlist == PlaylistName && P != Playlist) ?? false)
            {
                (PlaylistNameMessageTextBox.IsOpen, PlaylistNameMessageTextBox.IsClosable, PlaylistNameMessageTextBox.Message, PlaylistNameMessageTextBox.Severity) = (true, false, "A playlist with this name already exists!", InfoBarSeverity.Error);
                IsPrimaryButtonEnabled = false;
            }
            else
            {
                (PlaylistNameMessageTextBox.IsOpen, PlaylistNameMessageTextBox.IsClosable, PlaylistNameMessageTextBox.Message, PlaylistNameMessageTextBox.Severity) = (false, false, string.Empty, InfoBarSeverity.Informational);
                IsPrimaryButtonEnabled = true;
            }
        }

        private void Confirm_ButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            Playlist.Playlist = PlaylistName ?? Playlist.Playlist;
            Playlist.Image = Image ?? Playlist.Image;
            DBAccess.SaveChanges();
        }

        private void Close_ButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            Hide();
        }

        private async void PickAnImageButton_Click(object sender, RoutedEventArgs e)
        {
            Image = await StorageFileHelpers.PickAndSaveImageAsync() ?? Image;
        }
    }
}