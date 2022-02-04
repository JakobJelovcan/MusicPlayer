using MusicMetaDataLibrary.ID3v2.Constants;
using MusicMetaDataLibrary.ID3v2.Extensions;
using MusicMetaDataLibrary.ID3v2.Frames;
using MusicMetaDataLibrary.ID3v2.Helpers;
using System;
using System.Collections.Generic;

namespace MusicMetaDataLibrary.ID3v2
{
    public partial class ID3Tag
    {
        public string Subtitle
        {
            get => SubtitleFrame?.Content;
            set
            {
                if (SubtitleFrame is not TextFrame) SubtitleFrame = TextFrame.CreateTextFrame(this, HeaderID.Subtitle);
                SubtitleFrame.Content = value;
            }
        }

        private TextFrame SubtitleFrame
        {
            get => subtitleFrame ??= (TextFrame)GetFrame(HeaderID.Subtitle);
            set => subtitleFrame = value;
        }
        private TextFrame subtitleFrame;

        public string Title
        {
            get => TitleFrame?.Content;
            set
            {
                if (TitleFrame is not TextFrame) TitleFrame = TextFrame.CreateTextFrame(this, HeaderID.Title);
                TitleFrame.Content = value;
            }
        }

        private TextFrame TitleFrame
        {
            get => titleFrame ??= (TextFrame)GetFrame(HeaderID.Title);
            set => titleFrame = value;
        }
        private TextFrame titleFrame;

        public string Album
        {
            get => AlbumFrame?.Content;
            set
            {
                if (AlbumFrame is not TextFrame) AlbumFrame = TextFrame.CreateTextFrame(this, HeaderID.Album);
                AlbumFrame.Content = value;
            }
        }

        private TextFrame AlbumFrame
        {
            get => albumFrame ??= (TextFrame)GetFrame(HeaderID.Album);
            set => albumFrame = value;
        }
        private TextFrame albumFrame;

        public string Artist
        {
            get => ArtistFrame?.Content;
            set
            {
                if (ArtistFrame is not TextFrame) ArtistFrame = TextFrame.CreateTextFrame(this, HeaderID.Artist);
                ArtistFrame.Content = value;
            }
        }

        private TextFrame ArtistFrame
        {
            get => artistFrame ??= (TextFrame)GetFrame(HeaderID.Artist);
            set => artistFrame = value;
        }
        private TextFrame artistFrame;

        public uint Track
        {
            get => TrackFrame?.Content?.SplitToUInt32().Current ?? 0;
            set
            {
                if (TrackFrame is not TextFrame) TrackFrame = TextFrame.CreateTextFrame(this, HeaderID.Track);
                TrackFrame.Content = $"{value}/{NumberOfTracks}";
            }
        }

        public uint NumberOfTracks
        {
            get => TrackFrame?.Content?.SplitToUInt32().Total ?? 0;
            set
            {
                if (TrackFrame is not TextFrame) TrackFrame = TextFrame.CreateTextFrame(this, HeaderID.Track);
                TrackFrame.Content = $"{Track}/{value}";
            }
        }

        private TextFrame TrackFrame
        {
            get => trackFrame ??= (TextFrame)GetFrame(HeaderID.Track);
            set => trackFrame = value;
        }
        private TextFrame trackFrame;

        public int Year
        {
            get
            {
                bool success = int.TryParse(YearFrame?.Content, out int value);
                return success ? value : -1;
            }
            set
            {
                if (YearFrame is TextFrame) YearFrame.Content = value.ToString();
            }
        }

        private TextFrame YearFrame
        {
            get => yearFrame ??= (TextFrame)GetFrame(HeaderID.Year);
            set => yearFrame = value;
        }
        private TextFrame yearFrame;

        public int Disk
        {
            get => DiskFrame?.Content?.SplitToInt32().Current ?? -1;
            set
            {
                if (DiskFrame is not TextFrame) DiskFrame = TextFrame.CreateTextFrame(this, HeaderID.PartOfASet);
                DiskFrame.Content = $"{value}/{NumberOfDisks}";
            }
        }

        public int NumberOfDisks
        {
            get => DiskFrame?.Content?.SplitToInt32().Total ?? -1;
            set
            {
                if (DiskFrame is not TextFrame) DiskFrame = TextFrame.CreateTextFrame(this, HeaderID.PartOfASet);
                DiskFrame.Content = $"{Disk}/{value}";
            }
        }

        private TextFrame DiskFrame
        {
            get => diskFrame ??= (TextFrame)GetFrame(HeaderID.PartOfASet);
            set => diskFrame = value;
        }
        private TextFrame diskFrame;

        public APICFrame Picture => pictureFrame ??= (APICFrame)GetFrame(HeaderID.AttachedPicture);
        private APICFrame pictureFrame;

        public string Copyright
        {
            get => CopyrightFrame?.Content;
            set
            {
                if (CopyrightFrame is not TextFrame) CopyrightFrame = TextFrame.CreateTextFrame(this, HeaderID.CopyrightMessage);
                CopyrightFrame.Content = value;
            }
        }

        private TextFrame CopyrightFrame
        {
            get => copyrightFrame ??= (TextFrame)GetFrame(HeaderID.CopyrightMessage);
            set => copyrightFrame = value;
        }
        private TextFrame copyrightFrame;

        public string Genre
        {
            get
            {
                bool success = int.TryParse(GenreFrame?.Content?.Trim(new[] { '(', ')' }), out int value);
                return GenreHelpers.ToGenre(success ? value : 0);
            }
            set
            {
                if (GenreFrame is not TextFrame) GenreFrame = TextFrame.CreateTextFrame(this, HeaderID.Genre);
                GenreFrame.Content = $"({GenreHelpers.ToGenre(value)})";
            }
        }

        private TextFrame GenreFrame
        {
            get => genreFrame ??= (TextFrame)GetFrame(HeaderID.Genre);
            set => genreFrame = value;
        }
        private TextFrame genreFrame;

        public uint Rating
        {
            get => RatingFrame?.Rating / 51 ?? 0;
            set
            {
                if (RatingFrame is not Frames.RatingFrame) RatingFrame = RatingFrame.CreateRatingFrame(this, "MusicPlayer");
                RatingFrame.Rating = (uint)Math.Min(255, Math.Abs(value) * 51);
            }
        }

        private RatingFrame RatingFrame
        {
            get => ratingFrame ??= (RatingFrame)GetFrame(HeaderID.Rating);
            set => ratingFrame = value;
        }
        private RatingFrame ratingFrame;

        public IEnumerable<string> UnsynchronizedLyrics
        {
            get => UnsynchronizedLyricsFrame?.Lyrics;
            set
            {
                if (UnsynchronizedLyricsFrame is not Frames.UnsynchronizedLyricsFrame) UnsynchronizedLyricsFrame = UnsynchronizedLyricsFrame.CreateUnsynchronizedLyricsFrame(this, new byte[] { 0x65, 0x6e, 0x67 }, string.Empty);
                UnsynchronizedLyricsFrame.Lyrics = value;
            }
        }

        private UnsynchronizedLyricsFrame UnsynchronizedLyricsFrame
        {
            get => unsynchronizedLyricsFrame ??= (UnsynchronizedLyricsFrame)GetFrame(HeaderID.UnsynchronizedLyrics);
            set => unsynchronizedLyricsFrame = value;
        }
        private UnsynchronizedLyricsFrame unsynchronizedLyricsFrame;

        public IEnumerable<string> SynchronizedLyrics
        {
            get => SynchronizedLyricsFrame?.Content;
            set
            {
                if (SynchronizedLyricsFrame is not Frames.SynchronizedLyricsFrame) SynchronizedLyricsFrame = SynchronizedLyricsFrame.CreateSynchronizedLyricsFrame(this, new byte[] { 0x65, 0x6e, 0x67 }, string.Empty);
                SynchronizedLyricsFrame.Content = value;
            }
        }

        private SynchronizedLyricsFrame SynchronizedLyricsFrame
        {
            get => synchronizedLyricsFrame ??= (SynchronizedLyricsFrame)GetFrame(HeaderID.SynchronizedLyrics);
            set => synchronizedLyricsFrame = value;
        }
        private SynchronizedLyricsFrame synchronizedLyricsFrame;

        public TimeSpan Duration
        {
            get
            {
                bool result = int.TryParse(DurationFrame?.Content, out int value);
                return result ? TimeSpan.FromMilliseconds(value) : TimeSpan.Zero;
            }
            set
            {
                if (DurationFrame is not TextFrame) durationFrame = TextFrame.CreateTextFrame(this, HeaderID.Length);
                durationFrame.Content = value.TotalMilliseconds.ToString();
            }
        }

        private TextFrame DurationFrame
        {
            get => durationFrame ??= (TextFrame)GetFrame(HeaderID.Length);
            set => durationFrame = value;
        }
        private TextFrame durationFrame;

        public IEnumerable<string> Composers
        {
            get => ComposerFrame?.Content?.Split('/') ?? new string[0];
            set
            {
                if (value is IEnumerable<string>)
                {
                    if (ComposerFrame is not TextFrame) ComposerFrame = TextFrame.CreateTextFrame(this, HeaderID.Composer);
                    ComposerFrame.Content = string.Join("/", value);
                }
            }
        }

        private TextFrame ComposerFrame
        {
            get => composerFrame ??= (TextFrame)GetFrame(HeaderID.Composer);
            set => composerFrame = value;
        }
        private TextFrame composerFrame;

        public IEnumerable<string> Writers
        {
            get => WriterFrame?.Content?.Split('/') ?? new string[0];
            set
            {
                if (value is IEnumerable<string>)
                {
                    if (WriterFrame is not TextFrame) WriterFrame = TextFrame.CreateTextFrame(this, HeaderID.Lyricist);
                    WriterFrame.Content = string.Join("/", value);
                }
            }
        }

        private TextFrame WriterFrame
        {
            get => writerFrame ??= (TextFrame)GetFrame(HeaderID.Lyricist);
            set => writerFrame = value;
        }
        private TextFrame writerFrame;

        public string Publisher
        {
            get => PublisherFrame?.Content;
            set
            {
                if (PublisherFrame is not TextFrame) PublisherFrame = TextFrame.CreateTextFrame(this, HeaderID.Publisher);
                PublisherFrame.Content = value;
            }
        }

        private TextFrame PublisherFrame
        {
            get => publisherFrame ??= (TextFrame)GetFrame(HeaderID.Publisher);
            set => publisherFrame = value;
        }
        private TextFrame publisherFrame;
    }
}