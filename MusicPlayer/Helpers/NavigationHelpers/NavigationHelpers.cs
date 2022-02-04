using MusicPlayer.Pages;
using MusicPlayerLibrary.Constants;
using MusicPlayerLibrary.Models;
using System;
using System.Data;
using Windows.UI.Xaml.Controls;

namespace MusicPlayer.Helpers.NavigationHelpers
{
    public static class NavigationHelpers
    {
        public static Type GetPageTypeFromName(string pageName)
        {
            switch (pageName)
            {
                case "HomeNavigationItem": return typeof(HomePage);
                case "ForYouNavigationItem": return typeof(ForYouPage);
                case "SongsNavigationItem": return typeof(SongsPage);
                case "AlbumsNavigationItem": return typeof(AlbumsPage);
                case "ArtistsNavigationItem": return typeof(ArtistsPage);
                case "PlaylistsNavigationItem": return typeof(PlaylistsPage);
                case "SettingsNavigationItem": return typeof(SettingsPage);
                case "DebugNavigationItem": return typeof(DebugPage);
                default: return typeof(HomePage);
            }
        }

        public static Type GetPageTypeFromContent(BaseMusicModel musicModel)
        {
            switch (musicModel)
            {
                case AlbumModel: return typeof(AlbumContentPage);
                case ArtistModel: return typeof(ArtistContentPage);
                case PlaylistModel: return typeof(PlaylistContentPage);
                default: return null;
            }
        }

        public static Type GetPageTypeFromPlayingLocation(PlayingLocation playingLocation)
        {
            switch (playingLocation)
            {
                case PlayingLocation.Album: return typeof(AlbumContentPage);
                case PlayingLocation.Artist: return typeof(ArtistContentPage);
                case PlayingLocation.Playlist: return typeof(PlaylistContentPage);
                case PlayingLocation.SearchSongs: return typeof(SearchPage);
                case PlayingLocation.Songs: return typeof(SongsPage);
                case PlayingLocation.SongsForYou: return typeof(ForYouPage);
                default: throw new DataException();
            }
        }

        public static string GetPageNameFromType(Page page)
        {
            switch (page)
            {
                case HomePage: return "HomeNavigationItem";
                case ForYouPage: return "ForYouNavigationItem";
                case SongsPage: return "SongsNavigationItem";
                case AlbumsPage: return "AlbumsNavigationItem";
                case ArtistsPage: return "ArtistsNavigationItem";
                case PlaylistsPage: return "PlaylistsNavigationItem";
                case SettingsPage: return "SettingsNavigationItem";
                case DebugPage: return "DebugNavigationItem";
                default: return string.Empty;
            }
        }

        public static object GetPageContent(Page page)
        {
            switch (page)
            {
                case AlbumContentPage albumContentPage: return albumContentPage.Album;
                case ArtistContentPage artistContentPage: return artistContentPage.Artist;
                case PlaylistContentPage playlistContentPage: return playlistContentPage.Playlist;
                case AlbumsPage albumsPage: return albumsPage.MusicPlayer;
                case ArtistsPage artistsPage: return artistsPage.MusicPlayer;
                case PlaylistsPage playlistsPage: return playlistsPage.MusicPlayer;
                case SettingsPage settingsPage: return settingsPage.MusicPlayer;
                case HomePage homePage: return homePage.MusicPlayer;
                case ForYouPage forYouPage: return forYouPage.MusicPlayer;
                case SearchPage searchPage: return searchPage.MusicPlayer;
                case SongsPage songsPage: return songsPage.MusicPlayer;
                case DebugPage: return null;
                default: throw new DataException();
            }
        }
    }
}