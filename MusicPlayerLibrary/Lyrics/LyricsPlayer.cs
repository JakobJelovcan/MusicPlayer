using ExtensionsLibrary.Extensions;
using Microsoft.EntityFrameworkCore.Internal;
using MusicPlayerLibrary.Constants;
using MusicPlayerLibrary.Helpers.StorageHelpers;
using MusicPlayerLibrary.MusicPlayer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml;

namespace MusicPlayerLibrary.Lyrics
{
    public class LyricsPlayer : INotifyPropertyChanged
    {
        public LyricsPlayer(MusicPlayerModel musicPlayer)
        {
            MusicPlayer = musicPlayer;
            LyricsList = new List<LyricModel>();
            DisplayLyrics = new ObservableCollection<LyricModel>();
            NextLyricTimer = new DispatcherTimer();
            NextLyricTimer.Tick += NextLyricTimer_Tick;
            HideLyricTimer = new DispatcherTimer();
            HideLyricTimer.Tick += HideLyricTimer_Tick;
        }

        private readonly MusicPlayerModel MusicPlayer;
        public DispatcherTimer NextLyricTimer;
        public DispatcherTimer HideLyricTimer;
        private int CurrentLyricIndex;
        public List<LyricModel> LyricsList;
        public ObservableCollection<LyricModel> DisplayLyrics;

        private LyricsModel Lyrics { get; set; }

        private TimeSpan Position => MusicPlayer?.MediaPlayer?.PlaybackSession?.Position ?? TimeSpan.Zero;

        public Visibility LyricsVisibility => IsEnabled & IsLoaded ? Visibility.Visible : Visibility.Collapsed;

        public bool IsEnabled
        {
            get => isEnabled;
            set
            {
                if (isEnabled != (IsLoaded && value))
                {
                    isEnabled = IsLoaded && value;
                    RaisePropertyChanged(nameof(IsEnabled), nameof(LyricsVisibility));
                    if (IsEnabled) PlayLyrics();
                    else PauseLyrics();
                }
            }
        }
        private bool isEnabled;

        public bool IsLoaded
        {
            get => isLoaded;
            private set
            {
                if (isLoaded != value)
                {
                    isLoaded = value;
                    RaisePropertyChanged(nameof(IsEnabled), nameof(IsLoaded), nameof(LyricsVisibility));
                }
            }
        }
        private bool isLoaded;

        public LyricModel CurrentLyric
        {
            get => currentLyric;
            set
            {
                if (currentLyric != value)
                {
                    if (currentLyric != null) currentLyric.IsHighlighted = false;
                    currentLyric = value;
                }
            }
        }
        private LyricModel currentLyric;

        public void CycleIsEnabled()
        {
            IsEnabled = !IsEnabled;
        }

        private LyricModel GetNextLyric()
        {
            return LyricsList.FirstOrDefault(L => L.Start + L.Duration > Position);
        }

        public void PlayLyrics()
        {
            if (IsLoaded)
            {
                NextLyricTimer.Stop();
                HideLyricTimer.Stop();
                if (GetNextLyric() is LyricModel nextLyric)
                {
                    CurrentLyricIndex = LyricsList.IndexOf(nextLyric) - 1;
                    NextLyricTimer.Interval = Position > nextLyric.Start ? TimeSpan.Zero : nextLyric.Start - Position;
                    UpdateDisplayLyrics(CurrentLyricIndex + 1);
                    if (MusicPlayer?.CurrentPlayingState == PlayingState.Playing) NextLyricTimer.Start();
                }
            }
        }

        private void SetTimers(LyricModel currentLyric)
        {
            LyricModel nextLyric = LyricsList.GetNext(currentLyric);
            TimeSpan nextLyricStartTimer = nextLyric != null ? nextLyric.Start - Position : TimeSpan.Zero;
            TimeSpan currentLyricStartOffset = currentLyric.Start < Position ? Position - currentLyric.Start : TimeSpan.Zero;
            TimeSpan currentLyricDurationTimer = (nextLyricStartTimer != TimeSpan.Zero && nextLyricStartTimer < currentLyric.Duration) ? nextLyricStartTimer : currentLyric.Duration - currentLyricStartOffset;
            NextLyricTimer.Interval = nextLyricStartTimer;
            HideLyricTimer.Interval = currentLyricDurationTimer;
            if (nextLyric != null) NextLyricTimer.Start();
            HideLyricTimer.Start();
        }

        public void PauseLyrics()
        {
            if (IsLoaded)
            {
                NextLyricTimer.Stop();
                HideLyricTimer.Stop();
            }
        }

        public void Clear()
        {
            Lyrics = null;
            IsLoaded = false;
            NextLyricTimer.Stop();
            HideLyricTimer.Stop();
            CurrentLyricIndex = -1;
            LyricsList.Clear();
            DisplayLyrics.Clear();
        }

        private void UpdateDisplayLyrics(int currentLyricIndex)
        {
            DisplayLyrics.UpdateInAscendingOrder(LyricsList.Skip(currentLyricIndex).Take(4).ToList(), L => L.Start);
        }

        public async Task LoadLyrics(LyricsModel lyricsModel)
        {
            Clear();
            Lyrics = lyricsModel;
            if (await StorageFileHelpers.TryGetFileFromPathAsync(Lyrics?.Path) is StorageFile storageFile)
            {
                string[] lyricDataRaw = (await FileIO.ReadTextAsync(storageFile)).Replace("\n", string.Empty).Replace("\r", string.Empty).Split('[', StringSplitOptions.RemoveEmptyEntries);
                if (lyricDataRaw.Any())
                {
                    (TimeSpan Start, TimeSpan Duration, string Text, LyricsSinger Singer)[] lyricData = lyricDataRaw.Select(L => LyricParsers.ParseLRC(L)).Where(L => !string.IsNullOrWhiteSpace(L.Item3)).ToArray();
                    for (int i = 0; i < lyricData.Length; i++)
                    {
                        (TimeSpan, TimeSpan, string, LyricsSinger) tempCurrentLyric = lyricData[i];
                        TimeSpan tempNextLyricStart = (i + 1 < lyricData.Length) ? lyricData[i + 1].Start : default;
                        TimeSpan duration = (tempNextLyricStart > TimeSpan.Zero && tempNextLyricStart < tempCurrentLyric.Item1 + tempCurrentLyric.Item2) ? tempNextLyricStart - tempCurrentLyric.Item1 : tempCurrentLyric.Item2;
                        LyricsList.Add(new LyricModel(tempCurrentLyric.Item1, duration, tempCurrentLyric.Item3, tempCurrentLyric.Item4));
                    }
                    IsLoaded = true;
                }
            }
            else IsLoaded = false;
        }

        private void HideLyricTimer_Tick(object sender, object e)
        {
            HideLyricTimer.Stop();
            CurrentLyric.IsHighlighted = false;
            UpdateDisplayLyrics((CurrentLyricIndex < LyricsList.Count) ? CurrentLyricIndex + 1 : CurrentLyricIndex);
        }

        private void NextLyricTimer_Tick(object sender, object e)
        {
            NextLyricTimer.Stop();
            HideLyricTimer.Stop();
            if (++CurrentLyricIndex < LyricsList.Count && LyricsList[CurrentLyricIndex] is LyricModel nextLyric)
            {
                CurrentLyric = nextLyric;
                CurrentLyric.IsHighlighted = true;
                SetTimers(CurrentLyric);
            }
        }

        protected void RaisePropertyChanged(params string[] values)
        {
            foreach (string value in values) PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(value));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}