using ExtensionsLibrary.Helpers;
using MusicMetaDataLibrary.ID3v2.Headers;

namespace MusicMetaDataLibrary.ID3v2.Frames
{
    internal class DefaultFrame : BaseFrame
    {
        public DefaultFrame(FrameHeader header)
        {
            Header = header;
        }

        public DefaultFrame(FrameHeader header, ArrayReader arrayReader) : base(header, arrayReader)
        {
            arrayReader.SkipSection();
        }

        public override int GetByteCount()
        {
            return RawData.Count;
        }

        public override ArrayWriter WriteBytes(ArrayWriter arrayWriter)
        {
            arrayWriter.WriteArraySegment(RawData);
            return arrayWriter;
        }


        public override bool IsEmpty()
        {
            return false;
        }
    }
}