using ExtensionsLibrary.Helpers;
using MusicMetaDataLibrary.ID3v2.Constants;
using MusicMetaDataLibrary.ID3v2.Headers;

namespace MusicMetaDataLibrary.ID3v2.Frames
{
    public class PlayCounterFrame : BaseFrame
    {
        public PlayCounterFrame(ID3Tag tag)
        {
            Header = new FrameHeader(tag, HeaderID.PlayCounter, this);
            Counter = new byte[4];
        }

        public PlayCounterFrame(FrameHeader header, ArrayReader arrayReader) : base(header, arrayReader)
        {
            if (!header.Encryption && !header.Compression) Counter = arrayReader.ReadBytes();
        }

        public byte[] Counter { get; private set; }

        public override int GetByteCount()
        {
            return Counter.Length;
        }

        public override ArrayWriter WriteBytes(ArrayWriter arrayWriter)
        {
            arrayWriter.WriteBytes(Counter);
            return arrayWriter;
        }

        public static PlayCounterFrame CreatePlayCounterFrame(ID3Tag tag)
        {
            return (PlayCounterFrame)tag.GetFrame(HeaderID.PlayCounter) ?? (PlayCounterFrame)tag.AddFrame(new PlayCounterFrame(tag));
        }

        public override bool IsEmpty()
        {
            return Counter.Length == 0;
        }
    }
}