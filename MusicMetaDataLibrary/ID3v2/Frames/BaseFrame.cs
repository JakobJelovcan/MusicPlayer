using ExtensionsLibrary.Helpers;
using MusicMetaDataLibrary.ID3v2.Constants;
using MusicMetaDataLibrary.ID3v2.Headers;
using System;

namespace MusicMetaDataLibrary.ID3v2.Frames
{
    public class BaseFrame
    {
        protected BaseFrame()
        {
            RawData = new ArraySegment<byte>();
        }

        protected BaseFrame(FrameHeader header, ArrayReader arrayReader)
        {
            Header = header;
            RawData = arrayReader.Section(header.FrameSize).GetSection();
        }

        public FrameHeader Header { get; protected set; }

        public ArraySegment<byte> RawData { get; private set; }

        public virtual int GetByteCount()
        {
            throw new NotImplementedException();
        }

        public virtual ArrayWriter WriteBytes(ArrayWriter arrayWriter)
        {
            throw new NotImplementedException();
        }

        public virtual bool IsEmpty()
        {
            throw new NotImplementedException();
        }

        public static BaseFrame Frame(FrameHeader header, ArrayReader arrayReader)
        {
            switch (header.HeaderID)
            {
                case HeaderID.AttachedPicture: return new APICFrame(header, arrayReader);
                //TextFrames
                case HeaderID.Album:
                case HeaderID.BPM:
                case HeaderID.Composer:
                case HeaderID.Genre:
                case HeaderID.CopyrightMessage:
                case HeaderID.Date:
                case HeaderID.PlaylistDelay:
                case HeaderID.EncodedBy:
                case HeaderID.Lyricist:
                case HeaderID.FileType:
                case HeaderID.Time:
                case HeaderID.ContentDescription:
                case HeaderID.Title:
                case HeaderID.Subtitle:
                case HeaderID.InitialKey:
                case HeaderID.Language:
                case HeaderID.Length:
                case HeaderID.MediaType:
                case HeaderID.OriginalAlbum:
                case HeaderID.OriginalFileName:
                case HeaderID.OriginalLyricist:
                case HeaderID.OriginalArtist:
                case HeaderID.OriginalReleaseYear:
                case HeaderID.FileOwner:
                case HeaderID.Artist:
                case HeaderID.Band:
                case HeaderID.Conductor:
                case HeaderID.RemixedBy:
                case HeaderID.PartOfASet:
                case HeaderID.Publisher:
                case HeaderID.Track:
                case HeaderID.RecordingDates:
                case HeaderID.RadioStationName:
                case HeaderID.RadioStationOwner:
                case HeaderID.Size:
                case HeaderID.RecordingCode:
                case HeaderID.SettingsForEncoding:
                case HeaderID.Year:
                    return new TextFrame(header, arrayReader);
                case HeaderID.UserDefinedFrame: return new UserDefinedTextFrame(header, arrayReader);
                //URLFrames
                case HeaderID.CommertialInformation:
                case HeaderID.CopyrightInformation:
                case HeaderID.FileWebpage:
                case HeaderID.ArtistWebpage:
                case HeaderID.SourceWebpage:
                case HeaderID.RadioStationHomepage:
                case HeaderID.Payment:
                case HeaderID.PublisherWebpage:
                    return new URLFrame(header, arrayReader);
                case HeaderID.UserDefinedLinkFrame: return new UserDefinedURLFrame(header, arrayReader);
                case HeaderID.SynchronizedLyrics: return new SynchronizedLyricsFrame(header, arrayReader);
                case HeaderID.UnsynchronizedLyrics: return new UnsynchronizedLyricsFrame(header, arrayReader);
                case HeaderID.Rating: return new RatingFrame(header, arrayReader);
                default: return new DefaultFrame(header, arrayReader);
            }
        }
    }
}