using ExtensionsLibrary.Extensions;
using MusicPlayerLibrary.Constants;
using MusicPlayerLibrary.Lyrics;
using MusicPlayerLibrary.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.Media;
using Windows.Media.Playback;
using Windows.UI.Xaml;

namespace MusicPlayerLibrary.MusicPlayer
{
    public partial class MusicPlayerModel
    {
        public readonly MediaPlayer MediaPlayer;
        public readonly MusicPlayerSearch Search;
        public readonly LyricsPlayer LyricsPlayer;
        private SystemMediaTransportControls SMTC;
        private bool IsSMTCSet;

        public bool IsEnabled => CurrentPlayingSong is SongModel;

        public ElementTheme ApplicationTheme
        {
            get => applicationTheme;
            private set
            {
                if (applicationTheme != value)
                {
                    applicationTheme = value;
                    RaisePropertyChanged(nameof(ApplicationTheme));
                }
            }
        }
        private ElementTheme applicationTheme;

        public SongModel CurrentPlayingSong
        {
            get => currentPlayingSong;
            set
            {
                currentPlayingSong?.UnregisterPropertyChanged(CurrentPlayingSong_PropertyChanged);
                if (currentPlayingSong is SongModel) (currentPlayingSong.PlayingState, currentPlayingSong.LastPlaybackPosition) = (PlayingState.NotPlaying, MediaPlayer?.PlaybackSession?.Position ?? TimeSpan.Zero);
                currentPlayingSong = value;
                if (currentPlayingSong is SongModel) (currentPlayingSong.LastPlayed, currentPlayingSong.TimesPlayed) = (DateTime.Now.Ticks, currentPlayingSong.TimesPlayed + 1);
                if (currentPlayingSong?.Genre is GenreModel genre) genre.TimesPlayed++;
                currentPlayingSong.RegisterPropertyChanged(CurrentPlayingSong_PropertyChanged);
                RaisePropertyChanged(nameof(CurrentPlayingSong), nameof(IsEnabled));
            }
        }
        private SongModel currentPlayingSong;

        public BaseMusicModel CurrentPlayingContent
        {
            get => currentPlayingContent;
            set
            {
                if (value != currentPlayingContent)
                {
                    if (currentPlayingContent is BaseMusicModel) currentPlayingContent.PlayingState = PlayingState.NotPlaying;
                    currentPlayingContent = value;
                    if (currentPlayingContent is BaseMusicModel)
                    {
                        currentPlayingContent.LastPlayed = DateTime.Now.Ticks;
                        currentPlayingContent.TimesPlayed++;
                        AddToLastPlayed(currentPlayingContent);
                        AddToMostPlayed(currentPlayingContent);
                    }
                    SongBeforeQueue = null;
                    CurrentPlayingQueue.Clear();
                    RaisePropertyChanged(nameof(CurrentPlayingContent));
                }
            }
        }
        private BaseMusicModel currentPlayingContent;

        public PlayingState CurrentPlayingState
        {
            get => currentPlayingState;
            set
            {
                currentPlayingState = value;
                if (CurrentPlayingSong is SongModel) CurrentPlayingSong.PlayingState = value;
                if (CurrentPlayingContent is BaseMusicModel) CurrentPlayingContent.PlayingState = value;
                RaisePropertyChanged(nameof(CurrentPlayingState));
            }
        }
        private PlayingState currentPlayingState;

        public PlayingLocation CurrentPlayingLocation
        {
            get => currentPlayingLocation;
            set
            {
                if (value != currentPlayingLocation)
                {
                    currentPlayingLocation = value;
                    RaisePropertyChanged(nameof(CurrentPlayingLocation));
                }
            }
        }
        private PlayingLocation currentPlayingLocation;

        public IList<SongModel> CurrentPlayingSongList
        {
            get => currentPlayingSongList;
            set
            {
                if (currentPlayingSongList != value)
                {
                    currentPlayingSongList = value;
                    CurrentPlayingRandomSongList = value.Randomize();
                }
            }
        }
        private IList<SongModel> currentPlayingSongList;

        public IList<SongModel> CurrentPlayingRandomSongList;

        private SongModel SongBeforeQueue;
        private Queue<SongModel> CurrentPlayingQueue;

        public ObservableCollection<SongModel> Songs
        {
            get => songs;
            set
            {
                if (value != songs)
                {
                    songs = value;
                    RaisePropertyChanged(nameof(Songs));
                }
            }
        }
        private ObservableCollection<SongModel> songs;

        public ObservableCollection<AlbumModel> Albums
        {
            get => albums;
            set
            {
                if (value != albums)
                {
                    albums = value;
                    RaisePropertyChanged(nameof(Albums));
                }
            }
        }
        private ObservableCollection<AlbumModel> albums;

        public ObservableCollection<ArtistModel> Artists
        {
            get => artists;
            set
            {
                artists = value;
                RaisePropertyChanged(nameof(Artists));
            }
        }
        private ObservableCollection<ArtistModel> artists;

        public ObservableCollection<PlaylistModel> Playlists
        {
            get => playlists;
            set
            {
                if (value != playlists)
                {
                    playlists = value;
                    RaisePropertyChanged(nameof(Playlists));
                }
            }
        }
        private ObservableCollection<PlaylistModel> playlists;

        public ObservableCollection<GenreModel> Genres
        {
            get => genres;
            set
            {
                if (genres != value)
                {
                    genres = value;
                    RaisePropertyChanged(nameof(Genres));
                }
            }
        }
        private ObservableCollection<GenreModel> genres;

        public ObservableCollection<AlbumModel> AlbumsForYou
        {
            get => albumsForYou;
            set
            {
                if (albumsForYou != value)
                {
                    albumsForYou = value;
                    RaisePropertyChanged(nameof(AlbumsForYou));
                }
            }
        }
        private ObservableCollection<AlbumModel> albumsForYou;

        public ObservableCollection<SongModel> SongsForYou
        {
            get => songsForYou;
            set
            {
                if (songsForYou != value)
                {
                    songsForYou = value;
                    RaisePropertyChanged(nameof(SongsForYou));
                }
            }
        }
        private ObservableCollection<SongModel> songsForYou;

        public ObservableCollection<BaseMusicModel> LastPlayed
        {
            get => lastPlayed;
            set
            {
                if (lastPlayed != value)
                {
                    lastPlayed = value;
                    RaisePropertyChanged(nameof(LastPlayed));
                }
            }
        }
        private ObservableCollection<BaseMusicModel> lastPlayed;

        public ObservableCollection<BaseMusicModel> MostPlayed
        {
            get => mostPlayed;
            set
            {
                if (mostPlayed != value)
                {
                    mostPlayed = value;
                    RaisePropertyChanged(nameof(MostPlayed));
                }
            }
        }
        private ObservableCollection<BaseMusicModel> mostPlayed;

        public Order SongOrder
        {
            get => songOrder;
            set
            {
                if (songOrder != value)
                {
                    songOrder = value;
                    RaisePropertyChanged(nameof(SongOrder));
                }
            }
        }
        private Order songOrder;

        public SongOrderType SongOrderType
        {
            get => songOrderType;
            set
            {
                if (songOrderType != value)
                {
                    songOrderType = value;
                    RaisePropertyChanged(nameof(SongOrderType));
                }
            }
        }
        private SongOrderType songOrderType;

        public bool Shuffle
        {
            get => shuffle;
            set
            {
                if (shuffle != value)
                {
                    shuffle = value;
                    RaisePropertyChanged(nameof(Shuffle));
                }
            }
        }
        private bool shuffle;

        public LoopType Loop
        {
            get => loop;
            set
            {
                if (loop != value)
                {
                    loop = value;
                    RaisePropertyChanged(nameof(Loop));
                }
            }
        }
        private LoopType loop;
    }
}