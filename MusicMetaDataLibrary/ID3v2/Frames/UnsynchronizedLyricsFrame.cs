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
    public class UnsynchronizedLyricsFrame : BaseFrame
    {
        public UnsynchronizedLyricsFrame(ID3Tag tag, byte[] language, string contentDescriptor)
        {
            Header = new FrameHeader(tag, HeaderID.UnsynchronizedLyrics, this);
            Encoding = Encoding.Unicode;
            Language = language;
            DescriptorContainer = new StringContainer(contentDescriptor, Encoding, true);
            LyricsContainer = new StringListContainer(Encoding);
        }

        public UnsynchronizedLyricsFrame(FrameHeader header, ArrayReader arrayReader) : base(header, arrayReader)
        {
            if (!header.Encryption && !header.Compression)
            {
                Encoding = EncodingHelpers.GetEncoding(arrayReader.ReadByte());
                Language = arrayReader.ReadBytes(3);
                DescriptorContainer = new StringContainer(arrayReader.GetArraySegment(Encoding), Encoding, true);
                LyricsContainer = new StringListContainer(arrayReader.GetArraySegment(), Encoding);
            }
        }

        public Encoding Encoding { get; private set; }

        public byte[] Language { get; private set; }

        public string Descriptor
        {
            get => DescriptorContainer.Content;
            set => DescriptorContainer.Content = value;
        }
        private readonly StringContainer DescriptorContainer;

        public IEnumerable<string> Lyrics
        {
            get => LyricsContainer.Collection;
            set => LyricsContainer.Collection = value;
        }
        private readonly StringListContainer LyricsContainer;

        public override int GetByteCount()
        {
            return DescriptorContainer.GetByteCount() + LyricsContainer.GetByteCount() + 4;
        }

        public override ArrayWriter WriteBytes(ArrayWriter arrayWriter)
        {
            arrayWriter.WriteByte(Encoding.ToByte());
            arrayWriter.WriteBytes(Language);
            arrayWriter.WriteBytes(DescriptorContainer.GetBytes());
            LyricsContainer.WriteBytes(arrayWriter);
            return arrayWriter;
        }

        public static UnsynchronizedLyricsFrame CreateUnsynchronizedLyricsFrame(ID3Tag tag, byte[] language, string contentDescriptor)
        {
            return (UnsynchronizedLyricsFrame)tag.GetFrames(HeaderID.UnsynchronizedLyrics).FirstOrDefault(F => F is UnsynchronizedLyricsFrame unsynchronizedLyricsFrame && unsynchronizedLyricsFrame.Language == language && unsynchronizedLyricsFrame.Descriptor == contentDescriptor) ?? (UnsynchronizedLyricsFrame)tag.AddFrame(new UnsynchronizedLyricsFrame(tag, language, contentDescriptor));
        }

        public override bool IsEmpty()
        {
            return false;
        }
    }
}