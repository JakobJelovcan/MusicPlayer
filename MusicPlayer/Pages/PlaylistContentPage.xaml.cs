using ExtensionsLibrary.Extensions;
using MusicPlayerLibrary.Constants;
using MusicPlayerLibrary.ContentDialogs;
using MusicPlayerLibrary.Interfaces;
using MusicPlayerLibrary.Models;
using MusicPlayerLibrary.MusicPlayer;
using System;
using System.Diagnostics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace MusicPlayer.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PlaylistContentPage : Page, IMusicPlayerPage, IScrollablePage
    {
        public PlaylistContentPage()
        {
            InitializeComponent();
#if DEBUG
            Debug.WriteLine($"PlaylistsContentPage {GetHashCode()} Constructed");
#endif
        }

        ~PlaylistContentPage()
        {
#if DEBUG
            Debug.WriteLine($"PlaylistContentPage {GetHashCode()} Finalized");
#endif
        }

        public MusicPlayerModel MusicPlayer
        {
            get => (MusicPlayerModel)GetValue(MusicPlayerProperty);
            set => SetValue(MusicPlayerProperty, value);
        }
        public static readonly DependencyProperty MusicPlayerProperty = DependencyProperty.Register("MusicPlayer", typeof(MusicPlayerModel), typeof(PlaylistContentPage), new PropertyMetadata(null));

        public PlaylistModel Playlist
        {
            get => (PlaylistModel)GetValue(PlaylistProperty);
            set => SetValue(PlaylistProperty, value);
        }
        public static readonly DependencyProperty PlaylistProperty = DependencyProperty.Register("Playlist", typeof(PlaylistModel), typeof(PlaylistContentPage), new PropertyMetadata(null));

        public PageActions PageAction { get; set; }

        public object PageActionTarget { get; set; }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter is PageParameters parameters)
            {
                MusicPlayer = parameters.MusicPlayer;
                PageAction = parameters.PageAction;
                PageActionTarget = parameters.PageActionTarget;
                Playlist = parameters.PageParameter as PlaylistModel;
            }
        }

        private void PlaylistContentPage_Loaded(object sender, RoutedEventArgs e)
        {
            switch (PageAction)
            {
                case PageActions.ScrollInToView:
                    {
                        ScrollInToView(PageActionTarget);
                        break;
                    }
            }
        }

        public bool ScrollInToView(object obj)
        {
            if (IsLoaded) PlaylistContentListView.ScrollIntoView(obj ?? MusicPlayer.CurrentPlayingSong);
            return IsLoaded;
        }

        private void PlaylistHeader_PlayPause(object sender, BaseMusicModel e)
        {
            MusicPlayer.PlayMusicModel(e, PlayingLocation.Playlist).FireAndForget();
        }

        private void SongTileMinimalisticNoNum_PlayPause(object sender, BaseMusicModel e)
        {
            MusicPlayer.PlayMusicModel(e as SongModel, Playlist, PlayingLocation.Playlist).FireAndForget();
        }
        //
        //SongFlyout
        //
        private async void SongFlyoutItemEdit_Click(object sender, object args)
        {
            await new SongContentDialog((sender as MenuFlyoutItem).DataContext as SongModel).ShowAsync();
        }

        private async void SongFlyoutItemRemove_Click(object sender, object args)
        {
            ContentDialog contentDialog = new ContentDialog
            {
                Title = "Warning",
                Content = "Do you really want to remove this song?",
                DefaultButton = ContentDialogButton.Primary,
                PrimaryButtonText = "Yes",
                CloseButtonText = "No"
            };
            SongModel song = (sender as MenuFlyoutItem).DataContext as SongModel;
            if (await contentDialog.ShowAsync() == ContentDialogResult.Primary) song.Remove();
        }

        private async void SongFlyoutItemRemoveFromPlaylist_Click(object sender, object args)
        {
            ContentDialog contentDialog = new ContentDialog
            {
                Title = "Warning",
                Content = "Do you really want to remove this song from the playlist?",
                DefaultButton = ContentDialogButton.Primary,
                PrimaryButtonText = "Yes",
                CloseButtonText = "No"
            };
            SongModel song = (sender as MenuFlyoutItem).DataContext as SongModel;
            if (await contentDialog.ShowAsync() == ContentDialogResult.Primary) Playlist?.RemoveSong(song);
        }

        private void SongFlyoutItemAddToQueue_Click(object sender, object args)
        {
            ((sender as MenuFlyoutItem).DataContext as BaseMusicModel).AddToQueue();
        }
    }
}