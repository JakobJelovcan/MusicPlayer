using ExtensionsLibrary.Extensions;
using Microsoft.EntityFrameworkCore.Internal;
using MusicPlayerLibrary.Constants;
using MusicPlayerLibrary.Data.Settings;
using MusicPlayerLibrary.Helpers.LiveTilesHelpers;
using MusicPlayerLibrary.Helpers.StorageHelpers;
using MusicPlayerLibrary.Info;
using MusicPlayerLibrary.Lyrics;
using MusicPlayerLibrary.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Windows.Media;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.Storage;
using Windows.Storage.Streams;

namespace MusicPlayerLibrary.MusicPlayer
{
    public partial class MusicPlayerModel : IDisposable
    {
        public void CycleLoop()
        {
            Loop = (LoopType)((int)Loop < 2 ? (int)Loop + 1 : 0);
        }

        public void CycleShuffle()
        {
            Shuffle = !Shuffle;
        }

        public void CycleMute()
        {
            Settings.Muted = !Settings.Muted;
        }

        public async Task PlayPrevious()
        {
            if (MediaPlayer.PlaybackSession.Position < TimeSpan.FromSeconds(5))
            {
                (SongModel song, bool autoPlay) = GetPreviousSong();
                if (!await song.ValidateSong())
                {
                    if (Settings.ShowSongMissingError) InfoMessage.ShowMessage("This song could not be played.", InfoTileSeverity.Error, true);
                    for (int i = 0; i < (Shuffle ? CurrentPlayingRandomSongList.Count : CurrentPlayingSongList.Count); i++)
                    {
                        bool tempAutoPlay;
                        (song, tempAutoPlay) = GetPreviousSong(song);
                        autoPlay &= tempAutoPlay;
                        if (await song.ValidateSong()) break;
                    }
                }
                await PlaySong(song, autoPlay);
            }
            else PlayAgain();
        }

        public async Task PlayNext()
        {
            (SongModel song, bool autoPlay) = GetNextSong();
            if (!await song.ValidateSong())
            {
                if (Settings.ShowSongMissingError) InfoMessage.ShowMessage("This song could not be played.", InfoTileSeverity.Error, true);
                for (int i = 0; i < (Shuffle ? CurrentPlayingRandomSongList.Count : CurrentPlayingSongList.Count); i++)
                {
                    bool tempAutoPlay;
                    (song, tempAutoPlay) = GetNextSong(song);
                    autoPlay &= tempAutoPlay;
                    if (await song.ValidateSong()) break;
                }
            }
            await PlaySong(song, autoPlay);
        }

        public void PlayPause()
        {
            switch (CurrentPlayingState)
            {
                case PlayingState.Paused:
                    {
                        MediaPlayer.Play();
                        break;
                    }
                case PlayingState.Playing:
                    {
                        MediaPlayer.Pause();
                        break;
                    }
            }
        }

        private void Stop()
        {
            MediaPlayer.Pause();
            CurrentPlayingState = PlayingState.NotPlaying;
        }

        private void PlayAgain()
        {
            MediaPlayer.PlaybackSession.Position = TimeSpan.Zero;
            MediaPlayer.Play();
        }

        private async Task PlaySong(SongModel song, bool autoPlay = true, bool playFromPosition = false)
        {
            MediaPlayer.Pause();
            if (song != null && await song.ValidateSong())
            {
                MediaPlayer.PlaybackSession.Position = TimeSpan.Zero;
                if (!IsSMTCSet) SetSMTC();
                await SetSMTCDU(song);
                CurrentPlayingSong = song;
                MediaPlayer.Source = MediaSource.CreateFromStorageFile(await song.GetStorageFileAsync());
                MediaPlayer.AutoPlay = autoPlay;
                LiveTilesHelpers.LoadCurrentSongLiveTile(song);
                if (playFromPosition) MediaPlayer.PlaybackSession.Position = song.LastPlaybackPosition;
                await LoadLyrics(song.Lyrics);
            }
            else
            {
                if (Settings.ShowSongMissingError) InfoMessage.ShowMessage("This song could not be played.", InfoTileSeverity.Error, true);
                Stop();
            }
        }

        public (SongModel, bool) GetNextSong(SongModel song = null)
        {
            song ??= SongBeforeQueue ?? CurrentPlayingSong;
            if (CurrentPlayingQueue.Count != 0)
            {
                SongBeforeQueue ??= CurrentPlayingSong;
                return (CurrentPlayingQueue.Dequeue(), true);
            }
            IList<SongModel> songList = Shuffle ? CurrentPlayingRandomSongList : CurrentPlayingSongList;
            int currentSongIndex = GetCurrentSongIndex(song);
            SongBeforeQueue = null;
            return (currentSongIndex < songList.Count - 1) ? (songList[currentSongIndex + 1], true) : (songList[0], Loop.Equals(LoopType.RepeatAll));
        }

        public (SongModel, bool) GetPreviousSong(SongModel song = null)
        {
            song ??= SongBeforeQueue ?? CurrentPlayingSong;
            if (SongBeforeQueue != null)
            {
                SongBeforeQueue = null;
                return (song, true);
            }
            IList<SongModel> songList = Shuffle ? CurrentPlayingRandomSongList : CurrentPlayingSongList;
            int currentSongIndex = GetCurrentSongIndex(song);
            return ((currentSongIndex > 0) ? songList[currentSongIndex - 1] : songList[songList.Count - 1], true);
        }

        private int GetCurrentSongIndex(SongModel song = null)
        {
            song ??= CurrentPlayingSong;
            return Shuffle ? CurrentPlayingRandomSongList?.IndexOf(song) ?? -1 : CurrentPlayingSongList?.IndexOf(song) ?? -1;
        }

        public async Task PlayMusicModel(BaseMusicModel musicModel, PlayingLocation playingLocation, IList<SongModel> songs = null, bool autoPlay = true, bool playFromPosition = false)
        {
            if (musicModel == CurrentPlayingContent || musicModel == CurrentPlayingSong) PlayPause();
            else
            {
                songs ??= musicModel.GetSongs();
                CurrentPlayingLocation = playingLocation;
                CurrentPlayingSongList = songs;
                if (musicModel is SongModel song) CurrentPlayingContent = null;
                else
                {
                    song = songs.FirstOrDefault();
                    CurrentPlayingContent = musicModel;
                }
                await PlaySong(song, autoPlay, playFromPosition);
            }
        }

        public async Task PlayMusicModel(PlayingLocation playingLocation, IList<SongModel> songs)
        {
            if (songs.Any())
            {
                SongModel song = songs.FirstOrDefault();
                if (song == CurrentPlayingContent || song == CurrentPlayingSong) PlayPause();
                else
                {
                    songs ??= song.GetSongs();
                    CurrentPlayingLocation = playingLocation;
                    CurrentPlayingSongList = songs;
                    CurrentPlayingContent = null;
                    await PlaySong(song);
                }
            }
        }

        public async Task PlayMusicModel(SongModel song, BaseMusicModel musicModel, PlayingLocation playingLocation, bool autoPlay = true, bool playFromPosition = false)
        {
            if (song == currentPlayingSong) PlayPause();
            else
            {
                CurrentPlayingLocation = playingLocation;
                CurrentPlayingSongList = musicModel.GetSongs();
                CurrentPlayingContent = musicModel;
                await PlaySong(song, autoPlay, playFromPosition);
            }
        }

        public async Task LoadLyrics(LyricsModel lyricsModel)
        {
            LyricsPlayer.Clear();
            await LyricsPlayer.LoadLyrics(lyricsModel);
        }

        private async Task SetSMTCDU(SongModel song = null)
        {
            song ??= CurrentPlayingSong;
            if (await song?.GetStorageFileAsync() is StorageFile storageFile)
            {
                SystemMediaTransportControlsDisplayUpdater SMTCDU = SMTC.DisplayUpdater;
                await SMTCDU.CopyFromFileAsync(MediaPlaybackType.Music, storageFile);
                if (await StorageFileHelpers.TryGetFileFromPathAsync(song?.Image?.Path) is StorageFile imageFile) SMTCDU.Thumbnail = RandomAccessStreamReference.CreateFromFile(imageFile);
                SMTCDU.Type = MediaPlaybackType.Music;
                SMTCDU.Update();
            }
        }

        private void SetSMTC()
        {
            SMTC = MediaPlayer.SystemMediaTransportControls;
            MediaPlayer.CommandManager.IsEnabled = false;
            SMTC.ButtonPressed += SMTC_ButtonPressed;
            SMTC.IsEnabled = Settings.EnableOverlay;
            SMTC.IsPreviousEnabled = true;
            SMTC.IsNextEnabled = true;
            SMTC.IsPlayEnabled = true;
            SMTC.IsPauseEnabled = true;
            SMTC.ShuffleEnabled = true;
            IsSMTCSet = true;
        }

        public void AddToCurrentPlayingQueue(BaseMusicModel baseMusicModel)
        {
            switch (baseMusicModel)
            {
                case SongModel song: CurrentPlayingQueue.Enqueue(song); break;
                case AlbumModel album: CurrentPlayingQueue.EnqueueRange(album.GetSongs()); break;
                case ArtistModel artist: CurrentPlayingQueue.EnqueueRange(artist.GetSongs()); break;
                case PlaylistModel playlist: CurrentPlayingQueue.EnqueueRange(playlist.GetSongs()); break;
            }
        }

        private void ReorderSongs()
        {
            switch (SongOrder)
            {
                case Order.Ascending: Songs.Sort(GetSongOrderSelector(), S => S.Album); break;
                case Order.Descending: Songs.SortDescending(GetSongOrderSelector(), S => S.Album); break;
            }
            if (CurrentPlayingLocation == PlayingLocation.Songs) CurrentPlayingSongList = Songs;
        }

        private Func<SongModel, IComparable> GetSongOrderSelector()
        {
            switch (SongOrderType)
            {
                case SongOrderType.Title: return new Func<SongModel, IComparable>(S => S.Title);
                case SongOrderType.Album: return new Func<SongModel, IComparable>(S => S.Album);
                case SongOrderType.Artist: return new Func<SongModel, IComparable>(S => S.Artist);
                default: throw new DataException();
            }
        }

        public void UpdateSongOrder(SongOrderType orderType, Order order)
        {
            (SongOrderType, SongOrder) = (orderType, order);
            ReorderSongs();
        }

        public void Dispose()
        {
            MediaPlayer.CurrentStateChanged -= MediaPlayer_CurrentStateChanged;
            if (IsSMTCSet) SMTC.ButtonPressed -= SMTC_ButtonPressed;
            Settings.ThemeChanged -= Settings_ThemeChanged;
            Settings.VolumeChanged -= Settings_VolumeChanged;
            Settings.MuteChanged -= Settings_MuteChanged;
            Settings.NumOfGenresForYouChanged -= Settings_NumOfGenresForYouChanged;
            Settings.NumOfAlbumsForYouChanged -= Settings_NumOfAlbumsForYouChanged;
            Settings.NumOfSongsForYouChanged -= Settings_NumOfSongsForYouChanged;
            if (CurrentPlayingSong != null) CurrentPlayingSong.LastPlaybackPosition = MediaPlayer.PlaybackSession.Position;
            MediaPlayer?.Dispose();
        }

        public MusicPlayerModel()
        {
            MediaPlayer = new MediaPlayer();
            MediaPlayer.CurrentStateChanged += MediaPlayer_CurrentStateChanged;
            MediaPlayer.MediaEnded += MediaPlayer_MediaEnded;
            Search = new MusicPlayerSearch(this);
            LyricsPlayer = new LyricsPlayer(this);
            Settings.ThemeChanged += Settings_ThemeChanged;
            Settings.VolumeChanged += Settings_VolumeChanged;
            Settings.MuteChanged += Settings_MuteChanged;
            Settings.NumOfGenresForYouChanged += Settings_NumOfGenresForYouChanged;
            Settings.NumOfAlbumsForYouChanged += Settings_NumOfAlbumsForYouChanged;
            Settings.NumOfSongsForYouChanged += Settings_NumOfSongsForYouChanged;
            Settings.EnableOverlayChanged += Settings_EnableOverlayChanged;
        }

        ~MusicPlayerModel()
        {

        }
    }
}