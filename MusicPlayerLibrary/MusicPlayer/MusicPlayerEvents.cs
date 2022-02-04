using MusicPlayerLibrary.Constants;
using MusicPlayerLibrary.Models;
using System;
using System.ComponentModel;
using Windows.ApplicationModel.Core;
using Windows.Media;
using Windows.Media.Playback;
using Windows.UI.Core;
using Windows.UI.Xaml;

namespace MusicPlayerLibrary.MusicPlayer
{
    public partial class MusicPlayerModel : INotifyPropertyChanged
    {
        private async void SMTC_ButtonPressed(SystemMediaTransportControls sender, SystemMediaTransportControlsButtonPressedEventArgs args)
        {
            switch (args.Button)
            {
                case SystemMediaTransportControlsButton.Next:
                    {
                        await CoreApplication.MainView.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () => await PlayNext());
                        break;
                    }
                case SystemMediaTransportControlsButton.Previous:
                    {
                        await CoreApplication.MainView.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () => await PlayPrevious());
                        break;
                    }
                case SystemMediaTransportControlsButton.Play:
                    {
                        await CoreApplication.MainView.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => PlayPause());
                        break;
                    }
                case SystemMediaTransportControlsButton.Pause:
                    {
                        await CoreApplication.MainView.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => PlayPause());
                        break;
                    }
            }
        }

        private async void MediaPlayer_MediaEnded(MediaPlayer sender, object args)
        {
            if (Loop.Equals(LoopType.RepeatOne)) PlayAgain();
            else await CoreApplication.MainView.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () => await PlayNext());
        }

        private async void MediaPlayer_CurrentStateChanged(MediaPlayer sender, object args)
        {
            switch (MediaPlayer.PlaybackSession.PlaybackState)
            {
                case MediaPlaybackState.Paused:
                    {
                        SMTC.PlaybackStatus = MediaPlaybackStatus.Paused;
                        await CoreApplication.MainView.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => CurrentPlayingState = PlayingState.Paused);
                        await CoreApplication.MainView.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => LyricsPlayer.PauseLyrics());
                        break;
                    }
                case MediaPlaybackState.Playing:
                    {
                        SMTC.PlaybackStatus = MediaPlaybackStatus.Playing;
                        await CoreApplication.MainView.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => CurrentPlayingState = PlayingState.Playing);
                        await CoreApplication.MainView.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => LyricsPlayer.PlayLyrics());
                        break;
                    }
            }
        }

        private async void CurrentPlayingSong_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(SongModel.Lyrics): await LyricsPlayer.LoadLyrics(currentPlayingSong?.Lyrics); break;
            }
        }

        private void Settings_NumOfSongsForYouChanged(uint newValue)
        {
            LoadSongsForYou();
        }

        private void Settings_NumOfAlbumsForYouChanged(uint newValue)
        {
            LoadAlbumsForYou();
        }

        private void Settings_NumOfGenresForYouChanged(uint newValue)
        {
            LoadSongsForYou();
            LoadAlbumsForYou();
        }

        private void Settings_MuteChanged(bool newValue)
        {
            MediaPlayer.IsMuted = newValue;
        }

        private void Settings_VolumeChanged(double newValue)
        {
            MediaPlayer.Volume = newValue;
        }

        private void Settings_ThemeChanged(ElementTheme newTheme)
        {
            ApplicationTheme = newTheme;
        }

        private void Settings_EnableOverlayChanged(bool newValue)
        {
            if (SMTC != null) SMTC.IsEnabled = newValue;
        }

        protected void RaisePropertyChanged(params string[] values)
        {
            foreach (string value in values) PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(value));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}