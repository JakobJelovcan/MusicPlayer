using ExtensionsLibrary.Exceptions;
using ExtensionsLibrary.Extensions;
using ExtensionsLibrary.Helpers;
using MusicMetaDataLibrary.ID3v2.Exceptions;
using MusicMetaDataLibrary.ID3v2.Extensions;
using Windows.Storage.Streams;

namespace MusicMetaDataLibrary.ID3v2.Headers
{
    internal class TagHeader
    {
        public TagHeader(ID3Tag tag, DataReader dataReader)
        {
            if (dataReader.UnconsumedBufferLength < HeaderSize) throw new ArraySizeException();
            Tag = tag;
            FileID = dataReader.ReadString(3);
            FileVersion = dataReader.ReadBytes(2);
            Flags = dataReader.ReadByte();
            TagSize = dataReader.ReadInt28();
            if (FileID != "ID3") throw new InvalidTagIDException();
            if (FileVersion[0] != 3 && FileVersion[1] != 0) throw new InvalidTagVersionException();
        }

        private ID3Tag Tag { get; set; }

        public string FileID { get; private set; }

        public byte[] FileVersion { get; private set; }

        public int HeaderSize => 10;

        public int CompleteTagSize => HeaderSize + TagSize;

        public int TagSize { get; private set; }

        private byte Flags { get; set; }

        public bool Unsynchronization
        {
            get => (Flags & 0x80) != 0;
            set
            {
                switch (value)
                {
                    case true: Flags |= 0x80; break;
                    case false: Flags &= 0x7F; break;
                }
            }
        }

        public bool ExtendedHeader
        {
            get => (Flags & 0x40) != 0;
            set
            {
                switch (value)
                {
                    case true: Flags |= 0x40; break;
                    case false: Flags &= 0xBF; break;
                }
            }
        }

        public bool ExperimentalIndicator
        {
            get => (Flags & 0x20) != 0;
            set
            {
                switch (value)
                {
                    case true: Flags |= 0x20; break;
                    case false: Flags &= 0xDF; break;
                }
            }
        }

        public ArrayWriter WriteBytes(ArrayWriter arrayWriter)
        {
            arrayWriter.WriteString(FileID);
            arrayWriter.WriteBytes(FileVersion);
            arrayWriter.WriteByte(Flags);
            arrayWriter.WriteInt28(Tag.GetCompleteTagSize());
            return arrayWriter;
        }
    }
}