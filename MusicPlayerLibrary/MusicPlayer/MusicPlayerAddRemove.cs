using ExtensionsLibrary.Extensions;
using Microsoft.EntityFrameworkCore;
using MusicPlayerLibrary.Constants;
using MusicPlayerLibrary.Data.DataBase;
using MusicPlayerLibrary.Helpers.Extensions;
using MusicPlayerLibrary.Lyrics;
using MusicPlayerLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace MusicPlayerLibrary.MusicPlayer
{
    public partial class MusicPlayerModel
    {
        public void AddArtist(ArtistModel artist)
        {
            Artists.AddIfDoesntContainInAscendingOrder(artist, A => A.Artist);
        }

        public void RemoveArtist(ArtistModel artist)
        {
            Artists.Remove(artist);
            LastPlayed.Remove(artist);
            MostPlayed.Remove(artist);
            Search?.Artists?.Remove(artist);
            DBAccess.Artists.Remove(artist);
        }

        public void AddAlbum(AlbumModel album)
        {
            Albums.AddIfDoesntContainInAscendingOrder(album, A => A.Album);
        }

        public void RemoveAlbum(AlbumModel album)
        {
            Albums.Remove(album);
            LastPlayed.Remove(album);
            MostPlayed.Remove(album);
            AlbumsForYou.Remove(album);
            Search?.Albums?.Remove(album);
            DBAccess.Albums.Remove(album);
        }

        public void AddSong(SongModel song)
        {
            Songs.AddIfDoesntContainInAscendingOrder(song, S => S.Title);
        }

        public void RemoveSong(SongModel song)
        {
            Songs?.Remove(song);
            SongsForYou?.Remove(song);
            Search?.Songs?.Remove(song);
            Search?.DisplayedSongs?.Remove(song);
            DBAccess.Songs.Remove(song);
        }

        public void AddPlaylist(PlaylistModel playlist)
        {
            Playlists.AddIfDoesntContainInAscendingOrder(playlist, P => P.Playlist);
            DBAccess.Playlists.AddOrUpdate(playlist);
        }

        public void RemovePlaylist(PlaylistModel playlist)
        {
            Playlists.Remove(playlist);
            LastPlayed.Remove(playlist);
            MostPlayed.Remove(playlist);
            Search?.Playlists?.Remove(playlist);
            DBAccess.Playlists.Remove(playlist);
        }

        public void SongUpdated(SongModel song, SongUpdateParamater updateParamater)
        {
            if (CurrentPlayingContent is BaseMusicModel baseMusicModel) CurrentPlayingSongList = CurrentPlayingContent.GetSongs();
            if (updateParamater == SongUpdateParamater.Album && SongOrderType == SongOrderType.Album) Songs = Songs.OrderBy(GetSongOrderSelector()).ToObservableCollection();
            Playlists.Where(P => P.SongLinks.Any(SL => SL.Song == song)).ForEach(P => P.SongUpdated(song, updateParamater));
        }

        public void SongsUpdated(IEnumerable<SongModel> songs, SongUpdateParamater updateParamater)
        {
            if (CurrentPlayingContent is BaseMusicModel baseMusicModel) CurrentPlayingSongList = CurrentPlayingContent.GetSongs();
            if (updateParamater == SongUpdateParamater.Album && SongOrderType == SongOrderType.Album) Songs = Songs.OrderBy(GetSongOrderSelector()).ToObservableCollection();
            songs.ForEach(S => Playlists.Where(P => P.SongLinks.Any(SL => SL.Song == S)).ForEach(P => P.SongUpdated(S, updateParamater)));
        }

        public async Task ClearImageCache()
        {
            ImageModel[] removeImages = await DBAccess.Images.Include(I => I.SmallImageReferences).Include(I => I.LargeImageReferences).Where(I => !I.SmallImageReferences.Any() && !I.LargeImageReferences.Any()).ToArrayAsync();
            if (removeImages.Any() && !VisualTreeHelper.GetOpenPopups(Window.Current).Any())
            {
                ContentDialog contentDialog = new ContentDialog
                {
                    Title = "Warning",
                    Content = $"Do you want to delete {removeImages.Count()} images?",
                    PrimaryButtonText = "Yes",
                    CloseButtonText = "No",
                    DefaultButton = ContentDialogButton.Primary,
                };
                if (await contentDialog.ShowAsync() == ContentDialogResult.Primary) removeImages.ForEach(async I => await I.DeleteImageAsync());
            }
            else if (!VisualTreeHelper.GetOpenPopups(Window.Current).Any()) await new ContentDialog { Title = "Information", Content = "There are no unused images.", CloseButtonText = "OK", }.ShowAsync();
        }

        public async Task ClearLyricsCache()
        {
            LyricsModel[] removeLyrics = await DBAccess.Lyrics.Include(L => L.LyricsReferences).Where(L => !L.LyricsReferences.Any()).ToArrayAsync();
            if (removeLyrics.Any() && !VisualTreeHelper.GetOpenPopups(Window.Current).Any())
            {
                ContentDialog contentDialog = new ContentDialog
                {
                    Title = "Warning",
                    Content = $"Do you want to delete {removeLyrics.Count()} lyrics?",
                    PrimaryButtonText = "Yes",
                    CloseButtonText = "No",
                    DefaultButton = ContentDialogButton.Primary,
                };
                if (await contentDialog.ShowAsync() == ContentDialogResult.Primary) removeLyrics.ForEach(async L => await L.DeleteLyricsAsync());
            }
            else if (!VisualTreeHelper.GetOpenPopups(Window.Current).Any()) await new ContentDialog { Title = "Information", Content = "There are no unused lyrics.", CloseButtonText = "OK", }.ShowAsync();
        }
    }
}