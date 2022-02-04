using ExtensionsLibrary.Extensions;
using MusicPlayerLibrary.Constants;
using MusicPlayerLibrary.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;

namespace MusicPlayerLibrary.MusicPlayer
{
    public class MusicPlayerSearch : INotifyPropertyChanged
    {
        public MusicPlayerSearch(MusicPlayerModel musicPlayer)
        {
            MusicPlayer = musicPlayer;
            Artists = new ObservableCollection<ArtistModel>();
            Albums = new ObservableCollection<AlbumModel>();
            Playlists = new ObservableCollection<PlaylistModel>();
            DisplayedSongs = new ObservableCollection<SongModel>();
            Songs = new List<SongModel>();
        }
        private readonly MusicPlayerModel MusicPlayer;

        public Visibility ArtistListViewVisibility => (Artists?.Any() ?? false) ? Visibility.Visible : Visibility.Collapsed;

        public ObservableCollection<ArtistModel> Artists
        {
            get => artists;
            set
            {
                if (artists != value)
                {
                    artists = value;
                    RaisePropertyChanged(nameof(Artists), nameof(ArtistListViewVisibility));
                }
            }
        }
        private ObservableCollection<ArtistModel> artists;

        public Visibility AlbumListViewVisibility => (Albums?.Any() ?? false) ? Visibility.Visible : Visibility.Collapsed;

        public ObservableCollection<AlbumModel> Albums
        {
            get => albums;
            set
            {
                if (albums != value)
                {
                    albums = value;
                    RaisePropertyChanged(nameof(Albums), nameof(AlbumListViewVisibility));
                }
            }
        }
        private ObservableCollection<AlbumModel> albums;

        public Visibility PlaylistListViewVisibility => (Playlists?.Any() ?? false) ? Visibility.Visible : Visibility.Collapsed;

        public ObservableCollection<PlaylistModel> Playlists
        {
            get => playlists;
            set
            {
                if (playlists != value)
                {
                    playlists = value;
                    RaisePropertyChanged(nameof(Playlists), nameof(PlaylistListViewVisibility));
                }
            }
        }
        private ObservableCollection<PlaylistModel> playlists;

        public Visibility SongListViewVisibility => (DisplayedSongs?.Any() ?? false) ? Visibility.Visible : Visibility.Collapsed;

        public ObservableCollection<SongModel> DisplayedSongs
        {
            get => displayedSongs;
            set
            {
                if (displayedSongs != value)
                {
                    displayedSongs = value;
                    RaisePropertyChanged(nameof(DisplayedSongs), nameof(SongListViewVisibility));
                }
            }
        }
        private ObservableCollection<SongModel> displayedSongs;

        public List<SongModel> Songs
        {
            get => songs;
            set
            {
                if (songs != value)
                {
                    songs = value;
                    UpdateDisplaySongs();
                }
            }
        }
        private List<SongModel> songs;

        public int NumOfVisibleSongs
        {
            get => numOfVisibleSongs;
            set
            {
                if (numOfVisibleSongs != value)
                {
                    numOfVisibleSongs = value;
                    UpdateDisplaySongs();
                }
            }
        }
        private int numOfVisibleSongs;

        public ContentDisplay ContentDisplay
        {
            get => contentDisplay;
            set
            {
                if (contentDisplay != value)
                {
                    contentDisplay = value;
                    UpdateDisplaySongs();
                    RaisePropertyChanged(nameof(ContentDisplay));
                }
            }
        }
        private ContentDisplay contentDisplay;

        public Visibility ShowAllButtonVisibility
        {
            get => showAllButtonVisibility;
            set
            {
                if (showAllButtonVisibility != value)
                {
                    showAllButtonVisibility = value;
                    RaisePropertyChanged(nameof(ShowAllButtonVisibility));
                }
            }
        }
        private Visibility showAllButtonVisibility;

        private (string, string) GetQueryTypeAndValue(string query)
        {
            string queryType;
            string queryValue;
            if (query.Contains(':'))
            {
                string[] searchData = query.Split(':');
                queryType = searchData[0];
                queryValue = searchData[1].Replace("&Col;", ":");
            }
            else
            {
                queryType = string.Empty;
                queryValue = query.Replace("&Col;", ":");
            }
            queryType = queryType.Trim().ToLower();
            queryValue = queryValue.Trim().ToLower();
            return (queryType, queryValue);
        }

        public List<string> GetHints(string query)
        {
            (string queryType, string queryValue) = GetQueryTypeAndValue(query);
            ResourceLoader resources = ResourceLoader.GetForViewIndependentUse("MusicPlayerLibrary/SearchResources");
            if (queryType == resources.GetString("Title/Text")) return (string.IsNullOrWhiteSpace(queryValue) ? MusicPlayer.Songs.Select(S => S.Title).ToList() : MusicPlayer.Songs.Where(S => S.Title.ToLower().StartsWith(queryValue) && S.Title.ToLower() != queryValue).Select(S => S.Title)).Distinct().ToList();
            else if (queryType == resources.GetString("Album/Text")) return (string.IsNullOrWhiteSpace(queryValue) ? MusicPlayer.Albums.Select(A => A.Album).ToList() : MusicPlayer.Albums.Where(A => A.Album.ToLower().StartsWith(queryValue) && A.Album.ToLower() != queryValue).Select(S => S.Album)).Distinct().ToList();
            else if (queryType == resources.GetString("Artist/Text")) return (string.IsNullOrWhiteSpace(queryValue) ? MusicPlayer.Artists.Select(A => A.Artist).ToList() : MusicPlayer.Artists.Where(A => A.Artist.ToLower().StartsWith(queryValue) && A.Artist.ToLower() != queryValue).Select(S => S.Artist)).Distinct().ToList();
            else if (queryType == resources.GetString("Playlist/Text")) return (string.IsNullOrWhiteSpace(queryValue) ? MusicPlayer.Playlists.Select(P => P.Playlist).ToList() : MusicPlayer.Playlists.Where(P => P.Playlist.ToLower().StartsWith(queryValue) && P.Playlist.ToLower() != queryValue).Select(S => S.Playlist)).Distinct().ToList();
            else if (queryType == resources.GetString("Genre/Text")) return (string.IsNullOrWhiteSpace(queryValue) ? MusicPlayer.Genres.Select(G => G.GenreName).ToList() : MusicPlayer.Genres.Where(G => G.GenreName.ToLower().StartsWith(queryValue) && G.GenreName.ToLower() != queryValue).Select(S => S.GenreName)).Distinct().ToList();
            else if (queryType == resources.GetString("Year/Text")) return new List<string> { "No results" };
            else if (queryType == resources.GetString("Composer/Text")) return new List<string> { "No results" };
            else if (queryType == resources.GetString("Writer/Text")) return new List<string> { "No results" };
            else return (string.IsNullOrWhiteSpace(queryValue) ? MusicPlayer.Songs.Select(S => S.Title).ToList() : MusicPlayer.Songs.Where(S => S.Title.ToLower().StartsWith(queryValue) && S.Title.ToLower() != queryValue).Select(S => S.Title)).Distinct().ToList();
        }

        private void UpdateDisplaySongs()
        {
            DisplayedSongs.Update(ContentDisplay == ContentDisplay.More ? Songs : Songs.Take(NumOfVisibleSongs).ToList());
            ShowAllButtonVisibility = NumOfVisibleSongs >= Songs.Count ? Visibility.Collapsed : Visibility.Visible;
            RaisePropertyChanged(nameof(SongListViewVisibility));
        }

        private void UpdateSongs(List<SongModel> songs)
        {
            if (songs is not null) Songs = songs;
            else Songs = new List<SongModel>();
        }

        private void UpdateAlbums(List<AlbumModel> albums)
        {
            if (albums is not null) Albums.Update(albums);
            else Albums = new ObservableCollection<AlbumModel>();
            RaisePropertyChanged(nameof(AlbumListViewVisibility));
        }

        private void UpdateArtists(List<ArtistModel> artists)
        {
            if (artists is not null) Artists.Update(artists);
            else Artists = new ObservableCollection<ArtistModel>();
            RaisePropertyChanged(nameof(ArtistListViewVisibility));
        }

        private void UpdatePlaylists(List<PlaylistModel> playlists)
        {
            if (playlists is not null) Playlists.Update(playlists);
            else Playlists = new ObservableCollection<PlaylistModel>();
            RaisePropertyChanged(nameof(PlaylistListViewVisibility));
        }

        public void StartQuery(string query)
        {
            ContentDisplay = ContentDisplay.Less;
            (string queryType, string queryValue) = GetQueryTypeAndValue(query);
            (Func<SongModel, string, bool> songFunc, Func<AlbumModel, string, bool> albumFunc, Func<ArtistModel, string, bool> artistFunc, Func<PlaylistModel, string, bool> playlistFunc) = GetSearchFunctions(queryType);
            UpdateSongs(MusicPlayer?.Songs?.Where(S => songFunc(S, queryValue)).ToList() ?? null);
            UpdateAlbums(MusicPlayer?.Albums?.Where(A => albumFunc(A, queryValue)).ToList() ?? null);
            UpdateArtists(MusicPlayer?.Artists?.Where(A => artistFunc(A, queryValue)).ToList() ?? null);
            UpdatePlaylists(MusicPlayer?.Playlists?.Where(P => playlistFunc(P, queryValue)).ToList() ?? null);
        }

        private (Func<SongModel, string, bool>, Func<AlbumModel, string, bool>, Func<ArtistModel, string, bool>, Func<PlaylistModel, string, bool>) GetSearchFunctions(string queryType)
        {
            ResourceLoader resources = ResourceLoader.GetForViewIndependentUse("MusicPlayerLibrary/SearchResources");
            if (queryType == resources.GetString("Title/Text")) return ((S, Q) => S?.Title?.ToLower()?.StartsWith(Q) ?? false, (A, Q) => false, (A, Q) => false, (P, Q) => false);
            else if (queryType == resources.GetString("Album/Text")) return ((S, Q) => S?.Album?.ToLower()?.StartsWith(Q) ?? false, (A, Q) => A?.Album?.ToLower()?.StartsWith(Q) ?? false, (A, Q) => false, (P, Q) => false);
            else if (queryType == resources.GetString("Artist/Text")) return ((S, Q) => S?.Artist?.ToLower()?.StartsWith(Q) ?? false, (A, Q) => A?.Artist?.ToLower()?.StartsWith(Q) ?? false, (A, Q) => A?.Artist?.ToLower()?.StartsWith(Q) ?? false, (P, Q) => false);
            else if (queryType == resources.GetString("Playlist/Text")) return ((S, Q) => false, (A, Q) => false, (A, Q) => false, (P, Q) => P?.Playlist?.ToLower()?.StartsWith(Q) ?? false);
            else if (queryType == resources.GetString("Genre/Text")) return ((S, Q) => S?.Genre?.GenreName?.ToLower()?.StartsWith(Q) ?? false, (A, Q) => A?.Genres?.Any(G => G?.GenreName?.ToLower()?.StartsWith(Q) ?? false) ?? false, (A, Q) => false, (P, Q) => false);
            else if (queryType == resources.GetString("Writer/Text")) return ((S, Q) => S?.Writers?.Split(';')?.Any(W => W?.Trim()?.ToLower()?.StartsWith(Q) ?? false) ?? false, (A, Q) => false, (A, Q) => false, (P, Q) => false);
            else if (queryType == resources.GetString("Composer/Text")) return ((S, Q) => S?.Composers?.Split(';')?.Any(W => W?.Trim()?.ToLower()?.StartsWith(Q) ?? false) ?? false, (A, Q) => false, (A, Q) => false, (P, Q) => false);
            else if (queryType == resources.GetString("Year/Text")) return ((S, Q) => S?.Year.ToString()?.StartsWith(Q) ?? false, (A, Q) => A?.Year.ToString()?.StartsWith(Q) ?? false, (A, Q) => false, (P, Q) => false);
            else return ((S, Q) => S?.Title?.ToLower()?.StartsWith(Q) ?? false, (A, Q) => A?.Album?.ToLower()?.StartsWith(Q) ?? false, (A, Q) => A?.Artist?.ToLower()?.StartsWith(Q) ?? false, (P, Q) => P?.Playlist?.ToLower()?.StartsWith(Q) ?? false);
        }

        private void RaisePropertyChanged(params string[] values)
        {
            foreach (string value in values) PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(value));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}