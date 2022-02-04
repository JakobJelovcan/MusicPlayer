using MusicPlayerLibrary.Constants;
using MusicPlayerLibrary.Interfaces;
using MusicPlayerLibrary.MusicPlayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace MusicPlayerLibrary.Models
{
    public class BaseMusicModel : INotifyPropertyChanged, IEntity
    {
        public BaseMusicModel()
        {
            ID = default;
            IsEnabled = default;
            TimesPlayed = default;
            LastPlayed = default;
            IsSaveEnabled = true;
            Image = default;
            PlayingState = default;
        }

        public BaseMusicModel(bool isEnabled, long lastPlayed, int timesPlayed, ImageModel image, ImageModel largeImage)
        {
            ID = default;
            IsEnabled = isEnabled;
            LastPlayed = lastPlayed;
            TimesPlayed = timesPlayed;
            IsSaveEnabled = true;
            Image = image;
            LargeImage = largeImage;
        }

        public BaseMusicModel(bool isEnabled, bool isSaveEnabled, long lastPlayed, int timesPlayed, ImageModel image, ImageModel largeImage)
        {
            ID = default;
            IsEnabled = isEnabled;
            LastPlayed = lastPlayed;
            TimesPlayed = timesPlayed;
            IsSaveEnabled = isSaveEnabled;
            Image = image;
            LargeImage = largeImage;
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string ID { get; set; }

        public bool IsEnabled
        {
            get => isEnabled;
            set
            {
                if (value != isEnabled)
                {
                    isEnabled = value;
                    RaisePropertyChanged(nameof(IsEnabled));
                }
            }
        }
        private bool isEnabled;

        public bool IsSaveEnabled { get; private set; }

        public int TimesPlayed
        {
            get => timesPlayed;
            set
            {
                if (value != timesPlayed)
                {
                    timesPlayed = value;
                    RaisePropertyChanged(nameof(TimesPlayed));
                }
            }
        }
        private int timesPlayed;

        public long LastPlayed
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
        private long lastPlayed;

        public ImageModel Image
        {
            get => image;
            set
            {
                if (value != image)
                {
                    image = value;
                    RaisePropertyChanged(nameof(Image));
                }
            }
        }
        private ImageModel image;

        public ImageModel LargeImage
        {
            get => largeImage;
            set
            {
                if (largeImage != value)
                {
                    largeImage = value;
                    RaisePropertyChanged(nameof(LargeImage));
                }
            }
        }
        private ImageModel largeImage;

        public PlayingState PlayingState
        {
            get => playingState;
            set
            {
                if (value != playingState)
                {
                    playingState = value;
                    RaisePropertyChanged(nameof(PlayingState));
                }
            }
        }
        private PlayingState playingState;

        public virtual void Remove()
        {
            throw new NotImplementedException();
        }

        public virtual MusicPlayerModel GetMusicPlayer()
        {
            throw new NotImplementedException();
        }

        public virtual IList<SongModel> GetSongs()
        {
            throw new NotImplementedException();
        }

        public virtual PlayingLocation GetPlayingLocation()
        {
            throw new NotImplementedException();
        }

        public virtual void AddToQueue()
        {
            GetMusicPlayer().AddToCurrentPlayingQueue(this);
        }

        public virtual string GetName()
        {
            throw new NotImplementedException();
        }

        protected void RaisePropertyChanged(params string[] values)
        {
            foreach (string value in values) PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(value));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}