using ExtensionsLibrary.Extensions;
using MusicPlayerLibrary.Constants;
using MusicPlayerLibrary.ContentDialogs;
using MusicPlayerLibrary.Models;
using System;
using System.Linq;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace MusicPlayerLibrary.Resources
{
    public partial class MenusResourceDictionary
    {
        public MenusResourceDictionary()
        {
            InitializeComponent();
        }

        private void SongFlyout_Opening(object sender, object args)
        {
            MenuFlyout menuFlyout = (MenuFlyout)sender;
            if (menuFlyout.Items.Last() is MenuFlyoutSubItem subItem) menuFlyout.Items.Remove(subItem);
            menuFlyout.Items.Add(CreatePlaylistsMenuFlyoutSubItem((BaseMusicModel)menuFlyout.Target.DataContext));
        }

        private async void SongFlyoutItemEdit_Click(object sender, object args)
        {
            await new SongContentDialog((sender as MenuFlyoutItem).DataContext as SongModel).ShowAsync();
        }

        private async void SongFlyoutItemRemove_Click(object sender, object args)
        {
            ContentDialog contentDialog = new ContentDialog
            {
                Title = ResourceLoader.GetForViewIndependentUse("MusicPlayerLibrary/DialogResources").GetString("Warning/Text"),
                Content = ResourceLoader.GetForViewIndependentUse("MusicPlayerLibrary/DialogResources").GetString("Remove_Song_Message/Text"),
                DefaultButton = ContentDialogButton.Primary,
                PrimaryButtonText = ResourceLoader.GetForViewIndependentUse("MusicPlayerLibrary/DialogResources").GetString("Confirm/Text"),
                CloseButtonText = ResourceLoader.GetForViewIndependentUse("MusicPlayerLibrary/DialogResources").GetString("Cancel/Text")
            };
            SongModel song = (sender as MenuFlyoutItem).DataContext as SongModel;
            if (await contentDialog.ShowAsync() == ContentDialogResult.Primary) song.Remove();
        }

        private void SongFlyoutItemAddToQueue_Click(object sender, object args)
        {
            ((sender as MenuFlyoutItem).DataContext as BaseMusicModel).AddToQueue();
        }

        private void AlbumFlyout_Opening(object sender, object args)
        {
            MenuFlyout menuFlyout = (MenuFlyout)sender;
            if (menuFlyout.Items.Last() is MenuFlyoutSubItem subItem) menuFlyout.Items.Remove(subItem);
            menuFlyout.Items.Add(CreatePlaylistsMenuFlyoutSubItem((BaseMusicModel)menuFlyout.Target.DataContext));
        }

        private async void AlbumFlyoutItemEdit_Click(object sender, object args)
        {
            await new AlbumContentDialog((sender as MenuFlyoutItem).DataContext as AlbumModel).ShowAsync();
        }

        private async void AlbumFlyoutItemRemove_Click(object sender, object args)
        {
            ContentDialog contentDialog = new ContentDialog
            {
                Title = ResourceLoader.GetForViewIndependentUse("MusicPlayerLibrary/DialogResources").GetString("Warning/Text"),
                Content = ResourceLoader.GetForViewIndependentUse("MusicPlayerLibrary/DialogResources").GetString("Remove_Album_Message/Text"),
                DefaultButton = ContentDialogButton.Primary,
                PrimaryButtonText = ResourceLoader.GetForViewIndependentUse("MusicPlayerLibrary/DialogResources").GetString("Confirm/Text"),
                CloseButtonText = ResourceLoader.GetForViewIndependentUse("MusicPlayerLibrary/DialogResources").GetString("Cancel/Text")
            };
            AlbumModel album = (sender as MenuFlyoutItem).DataContext as AlbumModel;
            if (await contentDialog.ShowAsync() == ContentDialogResult.Primary) album.Remove();
        }

        private void AlbumFlyoutItemAddToQueue_Click(object sender, object args)
        {
            ((sender as MenuFlyoutItem).DataContext as BaseMusicModel).AddToQueue();
        }

        private void ArtistFlyout_Opening(object sender, object args)
        {
            MenuFlyout menuFlyout = (MenuFlyout)sender;
            if (menuFlyout.Items.Last() is MenuFlyoutSubItem subItem) menuFlyout.Items.Remove(subItem);
            menuFlyout.Items.Add(CreatePlaylistsMenuFlyoutSubItem((BaseMusicModel)menuFlyout.Target.DataContext));
        }

        private async void ArtistFlyoutItemEdit_Click(object sender, object args)
        {
            if ((sender as MenuFlyoutItem).DataContext is ArtistModel artist) await new ArtistContentDialog(artist).ShowAsync();
        }

        private async void ArtistFlyoutItemRemove_Click(object sender, object args)
        {
            ContentDialog contentDialog = new ContentDialog
            {
                Title = ResourceLoader.GetForViewIndependentUse("MusicPlayerLibrary/DialogResources").GetString("Warning/Text"),
                Content = ResourceLoader.GetForViewIndependentUse("MusicPlayerLibrary/DialogResources").GetString("Remove_Artist_Message/Text"),
                DefaultButton = ContentDialogButton.Primary,
                PrimaryButtonText = ResourceLoader.GetForViewIndependentUse("MusicPlayerLibrary/DialogResources").GetString("Confirm/Text"),
                CloseButtonText = ResourceLoader.GetForViewIndependentUse("MusicPlayerLibrary/DialogResources").GetString("Cancel/Text")
            };
            ArtistModel artist = (sender as MenuFlyoutItem).DataContext as ArtistModel;
            if (await contentDialog.ShowAsync() == ContentDialogResult.Primary) artist.Remove();
        }

        private void ArtistFlyoutItemAddToQueue_Click(object sender, object args)
        {
            ((sender as MenuFlyoutItem).DataContext as BaseMusicModel).AddToQueue();
        }

        private async void PlaylistFlyoutItemEdit_Click(object sender, object args)
        {
            await new PlaylistContentDialog((sender as MenuFlyoutItem).DataContext as PlaylistModel, PlaylistDialogTask.Edit).ShowAsync();
        }

        private async void PlaylistFlyoutItemDelete_Click(object sender, object args)
        {
            ContentDialog contentDialog = new ContentDialog
            {
                Title = ResourceLoader.GetForViewIndependentUse("MusicPlayerLibrary/DialogResources").GetString("Warning/Text"),
                Content = ResourceLoader.GetForViewIndependentUse("MusicPlayerLibrary/DialogResources").GetString("Delete_Playlist_Message/Text"),
                DefaultButton = ContentDialogButton.Primary,
                PrimaryButtonText = ResourceLoader.GetForViewIndependentUse("MusicPlayerLibrary/DialogResources").GetString("Confirm/Text"),
                CloseButtonText = ResourceLoader.GetForViewIndependentUse("MusicPlayerLibrary/DialogResources").GetString("Cancel/Text")
            };
            PlaylistModel playlist = (sender as MenuFlyoutItem).DataContext as PlaylistModel;
            if (await contentDialog.ShowAsync() == ContentDialogResult.Primary) playlist.Remove();
        }

        private void PlaylistFlyoutItemRemoveDuplicates_Click(object sender, object args)
        {
            ((sender as MenuFlyoutItem).DataContext as PlaylistModel).RemoveDulpicates();
        }

        private void PlaylistFlyoutItemAddToQueue_Click(object sender, object args)
        {
            ((sender as MenuFlyoutItem).DataContext as BaseMusicModel).AddToQueue();
        }

        private MenuFlyoutSubItem CreatePlaylistsMenuFlyoutSubItem(BaseMusicModel musicModel)
        {
            MenuFlyoutSubItem menuFlyoutSubItem = new MenuFlyoutSubItem { Text = ResourceLoader.GetForViewIndependentUse("MusicPlayerLibrary/FlyoutResources").GetString("Add_To_Playlist/Text"), DataContext = musicModel };
            musicModel.GetMusicPlayer().Playlists.ForEach(P => menuFlyoutSubItem.Items.Add(CreateNewPlaylistMenuFlyoutItem(musicModel, P)));
            menuFlyoutSubItem.Items.Add(new MenuFlyoutSeparator());
            MenuFlyoutItem newPlaylistFlyoutItem = new MenuFlyoutItem { Icon = new FontIcon { Glyph = "\xE710" }, Text = ResourceLoader.GetForViewIndependentUse("MusicPlayerLibrary/FlyoutResources").GetString("New_Playlist/Text"), DataContext = musicModel };
            newPlaylistFlyoutItem.Click += CreateNewPlaylistMenuFlyout_Click;
            menuFlyoutSubItem.Items.Add(newPlaylistFlyoutItem);
            return menuFlyoutSubItem;
        }

        private async void CreateNewPlaylistMenuFlyout_Click(object sender, RoutedEventArgs e)
        {
            BaseMusicModel musicModel = (sender as MenuFlyoutItem).DataContext as BaseMusicModel;
            PlaylistModel playlistModel = new PlaylistModel(musicModel.GetMusicPlayer(), musicModel.GetName(), true, true, musicModel.Image, musicModel.LargeImage);
            playlistModel.AddMusicModel(musicModel);
            if ((await new PlaylistContentDialog(playlistModel, PlaylistDialogTask.Create).ShowAsync()) == ContentDialogResult.Primary) playlistModel.Add();
        }

        private MenuFlyoutItem CreateNewPlaylistMenuFlyoutItem(BaseMusicModel musicModel, PlaylistModel playlist)
        {
            MenuFlyoutItem menuFlyoutItem = new MenuFlyoutItem() { Text = playlist.Playlist, DataContext = new { MusicModel = musicModel, Playlist = playlist } };
            menuFlyoutItem.Click += PlaylistMenuFlyoutItem_Click;
            return menuFlyoutItem;
        }

        private void PlaylistMenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {
            dynamic data = (sender as MenuFlyoutItem).DataContext;
            BaseMusicModel musicModel = data?.MusicModel as BaseMusicModel;
            PlaylistModel playlist = data?.Playlist as PlaylistModel;
            playlist.AddMusicModel(musicModel);
        }
    }
}