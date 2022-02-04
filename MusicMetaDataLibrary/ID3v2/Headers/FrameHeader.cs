using ExtensionsLibrary.Helpers;
using MusicMetaDataLibrary.ID3v2.Constants;
using MusicMetaDataLibrary.ID3v2.Frames;

namespace MusicMetaDataLibrary.ID3v2.Headers
{
    public class FrameHeader
    {
        public FrameHeader(ID3Tag tag, ArrayReader arrayReader)
        {
            Tag = tag;
            HeaderID = (HeaderID)arrayReader.ReadInt32();
            FrameSize = arrayReader.ReadInt32();
            Flags = arrayReader.ReadBytes(2);
            Frame = BaseFrame.Frame(this, arrayReader);
        }

        public FrameHeader(ID3Tag tag, HeaderID headerID, BaseFrame frame)
        {
            Tag = tag;
            HeaderID = headerID;
            Flags = new byte[2];
            Frame = frame;
        }

        public ID3Tag Tag { get; private set; }

        public HeaderID HeaderID { get; private set; }

        public int FrameSize { get; private set; }

        private byte[] Flags { get; set; }

        public bool TagAlterPreservation
        {
            get => (Flags[0] & 0x80) != 0;
            set
            {
                switch (value)
                {
                    case true: Flags[0] |= 0x80; break;
                    case false: Flags[0] &= 0x7F; break;
                }
            }
        }

        public bool FileAlterPreservation
        {
            get => (Flags[0] & 0x40) != 0;
            set
            {
                switch (value)
                {
                    case true: Flags[0] |= 0x40; break;
                    case false: Flags[0] &= 0xBF; break;
                }
            }
        }

        public bool ReadOnly
        {
            get => (Flags[0] & 0x20) != 0;
            set
            {
                switch (value)
                {
                    case true: Flags[0] |= 0x20; break;
                    case false: Flags[0] &= 0xDF; break;
                }
            }
        }

        public bool Compression
        {
            get => (Flags[1] & 0x80) != 0;
            set
            {
                switch (value)
                {
                    case true: Flags[1] |= 0x80; break;
                    case false: Flags[1] &= 0x7F; break;
                }
            }
        }

        public bool Encryption
        {
            get => (Flags[1] & 0x40) != 0;
            set
            {
                switch (value)
                {
                    case true: Flags[1] |= 0x40; break;
                    case false: Flags[1] &= 0xBF; break;
                }
            }
        }

        public bool GroupingIdentity
        {
            get => (Flags[1] & 0x40) != 0; set
            {
                switch (value)
                {
                    case true: Flags[1] |= 0x20; break;
                    case false: Flags[1] &= 0xDF; break;
                }
            }
        }

        public BaseFrame Frame { get; internal set; }

        public int GetByteCount()
        {
            return 10 + Frame.GetByteCount();
        }

        public ArrayWriter WriteBytes(ArrayWriter arrayWriter)
        {
            arrayWriter.WriteInt32((int)HeaderID);
            arrayWriter.WriteInt32(Frame.GetByteCount());
            arrayWriter.WriteBytes(Flags);
            return Frame.WriteBytes(arrayWriter);
        }
    }
}