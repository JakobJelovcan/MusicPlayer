using ExtensionsLibrary.Helpers;
using MusicMetaDataLibrary.ID3v2.Constants;
using MusicMetaDataLibrary.ID3v2.Containers;
using MusicMetaDataLibrary.ID3v2.Extensions;
using MusicMetaDataLibrary.ID3v2.Headers;
using MusicMetaDataLibrary.ID3v2.Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MusicMetaDataLibrary.ID3v2.Frames
{
    public class SynchronizedLyricsFrame : BaseFrame
    {
        public SynchronizedLyricsFrame(ID3Tag tag, byte[] language, string contentDescriptor)
        {
            Header = new FrameHeader(tag, HeaderID.SynchronizedLyrics, this);
            Encoding = Encoding.Unicode;
            Language = language;
            TimeStampFormat = 0x00;
            ContentType = 0x00;
            DescriptorContainer = new StringContainer(contentDescriptor, Encoding.ASCII, true);
            LyricsContainer = new StringListContainer(Encoding.ASCII);
        }

        public SynchronizedLyricsFrame(FrameHeader header, ArrayReader arrayReader) : base(header, arrayReader)
        {
            if (!header.Encryption && !header.Compression)
            {
                Encoding = EncodingHelpers.GetEncoding(arrayReader.ReadByte());
                Language = arrayReader.ReadBytes(3);
                TimeStampFormat = arrayReader.ReadByte();
                ContentType = arrayReader.ReadByte();
                DescriptorContainer = new StringContainer(arrayReader.GetArraySegment(Encoding), Encoding, true);
                LyricsContainer = new StringListContainer(arrayReader.GetArraySegment(), Encoding);
            }
        }

        public Encoding Encoding { get; private set; }

        public byte[] Language { get; private set; }

        public byte TimeStampFormat { get; private set; }

        public byte ContentType { get; private set; }

        public string Descriptor
        {
            get => DescriptorContainer.Content;
            set => DescriptorContainer.Content = value;
        }
        private readonly StringContainer DescriptorContainer;

        public IEnumerable<string> Content
        {
            get => LyricsContainer.Collection;
            set => LyricsContainer.Collection = value;
        }
        private readonly StringListContainer LyricsContainer;

        public override int GetByteCount()
        {
            return DescriptorContainer.GetByteCount() + LyricsContainer.GetByteCount() + 6;
        }

        public override ArrayWriter WriteBytes(ArrayWriter arrayWriter)
        {
            arrayWriter.WriteByte(Encoding.ToByte());
            arrayWriter.WriteBytes(Language);
            arrayWriter.WriteByte(TimeStampFormat);
            arrayWriter.WriteByte(ContentType);
            arrayWriter.WriteBytes(DescriptorContainer.GetBytes());
            LyricsContainer.WriteBytes(arrayWriter);
            return arrayWriter;
        }

        public static SynchronizedLyricsFrame CreateSynchronizedLyricsFrame(ID3Tag tag, byte[] language, string contentDescriptor)
        {
            return (SynchronizedLyricsFrame)tag.GetFrames(HeaderID.SynchronizedLyrics).FirstOrDefault(F => F is SynchronizedLyricsFrame synchronizedLyricsFrame && synchronizedLyricsFrame.Language == language && synchronizedLyricsFrame.Descriptor == contentDescriptor) ?? (SynchronizedLyricsFrame)tag.AddFrame(new SynchronizedLyricsFrame(tag, language, contentDescriptor));
        }

        public override bool IsEmpty()
        {
            return false;
        }
    }
}