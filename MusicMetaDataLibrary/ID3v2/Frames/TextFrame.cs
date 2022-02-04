using ExtensionsLibrary.Helpers;
using MusicMetaDataLibrary.ID3v2.Constants;
using MusicMetaDataLibrary.ID3v2.Containers;
using MusicMetaDataLibrary.ID3v2.Extensions;
using MusicMetaDataLibrary.ID3v2.Headers;
using MusicMetaDataLibrary.ID3v2.Helpers;
using System.Text;

namespace MusicMetaDataLibrary.ID3v2.Frames
{
    public class TextFrame : BaseFrame
    {
        public TextFrame(ID3Tag tag, HeaderID headerID)
        {
            Header = new FrameHeader(tag, headerID, this);
            Encoding = Encoding.Unicode;
            Container = new StringContainer(Encoding.Unicode, false);
        }

        public TextFrame(FrameHeader header, ArrayReader arrayReader) : base(header, arrayReader)
        {
            if (!header.Encryption && !header.Compression)
            {
                Encoding = EncodingHelpers.GetEncoding(arrayReader.ReadByte());
                Container = new StringContainer(arrayReader.GetArraySegment(), Encoding, false);
            }
        }

        public Encoding Encoding { get; private set; }

        public string Content
        {
            get => Container.Content;
            set => Container.Content = value;
        }
        private readonly StringContainer Container;

        public override int GetByteCount()
        {
            return Container.GetByteCount() + 1;
        }

        public override ArrayWriter WriteBytes(ArrayWriter arrayWriter)
        {
            arrayWriter.WriteByte(Encoding.ToByte());
            arrayWriter.WriteBytes(Container.GetBytes());
            return arrayWriter;
        }

        public static TextFrame CreateTextFrame(ID3Tag tag, HeaderID headerID)
        {
            return (TextFrame)tag.GetFrame(headerID) ?? (TextFrame)tag.AddFrame(new TextFrame(tag, headerID));
        }

        public override bool IsEmpty()
        {
            return string.IsNullOrEmpty(Content);
        }
    }
}