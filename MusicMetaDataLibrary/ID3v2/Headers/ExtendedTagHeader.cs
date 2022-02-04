using ExtensionsLibrary.Helpers;

namespace MusicMetaDataLibrary.ID3v2.Headers
{
    public class ExtendedTagHeader
    {
        public ExtendedTagHeader(ID3Tag tag, ArrayReader arrayReader)
        {
            Tag = tag;
            ExtendedHeaderSize = arrayReader.ReadInt32();
            Flags = arrayReader.ReadBytes(2);
            PaddingSize = arrayReader.ReadInt32();
        }

        public ExtendedTagHeader(ID3Tag tag, int paddingSize)
        {
            Tag = tag;
            ExtendedHeaderSize = 10;
            Flags = new byte[2];
            PaddingSize = paddingSize;
        }

        public ID3Tag Tag { get; private set; }

        public int ExtendedHeaderSize { get; private set; }

        private byte[] Flags { get; set; }

        public int PaddingSize { get; private set; }

        public bool IsCRCPresent
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

        public ArrayWriter WriteBytes(ArrayWriter arrayWriter)
        {
            arrayWriter.WriteInt32(ExtendedHeaderSize);
            arrayWriter.WriteBytes(Flags);
            arrayWriter.WriteInt32(PaddingSize);
            return arrayWriter;
        }
    }
}