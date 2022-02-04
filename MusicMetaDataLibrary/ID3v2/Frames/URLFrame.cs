using ExtensionsLibrary.Helpers;
using MusicMetaDataLibrary.ID3v2.Constants;
using MusicMetaDataLibrary.ID3v2.Containers;
using MusicMetaDataLibrary.ID3v2.Headers;
using System.Text;

namespace MusicMetaDataLibrary.ID3v2.Frames
{
    public class URLFrame : BaseFrame
    {
        public URLFrame(ID3Tag tag, HeaderID headerID)
        {
            Header = new FrameHeader(tag, headerID, this);
            Container = new StringContainer(Encoding.ASCII, false);
        }

        public URLFrame(FrameHeader header, ArrayReader arrayReader) : base(header, arrayReader)
        {
            if (!header.Encryption && !header.Compression) Container = new StringContainer(arrayReader.GetArraySegment(), Encoding.ASCII, false);
        }

        public string URL
        {
            get => Container.Content;
            set => Container.Content = value;
        }
        private readonly StringContainer Container;

        public override int GetByteCount()
        {
            return Container.GetByteCount();
        }

        public override ArrayWriter WriteBytes(ArrayWriter arrayWriter)
        {
            arrayWriter.WriteBytes(Container.GetBytes());
            return arrayWriter;
        }

        public static URLFrame CreateURLFrame(ID3Tag tag, HeaderID headerID)
        {
            return (URLFrame)tag.GetFrame(headerID) ?? (URLFrame)tag.AddFrame(new URLFrame(tag, headerID));
        }

        public override bool IsEmpty()
        {
            return string.IsNullOrEmpty(URL);
        }
    }
}