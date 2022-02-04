using ExtensionsLibrary.Helpers;
using MusicMetaDataLibrary.ID3v2.Constants;
using MusicMetaDataLibrary.ID3v2.Containers;
using MusicMetaDataLibrary.ID3v2.Extensions;
using MusicMetaDataLibrary.ID3v2.Headers;
using MusicMetaDataLibrary.ID3v2.Helpers;
using System;
using System.Linq;
using System.Text;

namespace MusicMetaDataLibrary.ID3v2.Frames
{
    public class GeneralEncapsulatedFrame : BaseFrame
    {
        public GeneralEncapsulatedFrame(ID3Tag tag, string contentDescriptor)
        {
            Header = new FrameHeader(tag, HeaderID.GeneralEncapsulatedObject, this);
            Encoding = Encoding.Unicode;
            MIMETypeContainer = new StringContainer(Encoding.ASCII, true);
            FileNameContainer = new StringContainer(Encoding.Unicode, true);
            ContentDescriptorContainer = new StringContainer(contentDescriptor, Encoding.Unicode, true);
            EncapsulatedObject = new ArraySegment<byte>();
        }

        public GeneralEncapsulatedFrame(FrameHeader header, ArrayReader arrayReader) : base(header, arrayReader)
        {
            if (!header.Encryption && !header.Compression)
            {
                Encoding = EncodingHelpers.GetEncoding(arrayReader.ReadByte());
                MIMETypeContainer = new StringContainer(arrayReader.GetArraySegment(Encoding.ASCII), Encoding.ASCII, true);
                FileNameContainer = new StringContainer(arrayReader.GetArraySegment(Encoding), Encoding, true);
                ContentDescriptorContainer = new StringContainer(arrayReader.GetArraySegment(Encoding), Encoding, true);
                EncapsulatedObject = arrayReader.GetArraySegment();
            }
        }

        public Encoding Encoding { get; private set; }

        public string MIMEType
        {
            get => MIMETypeContainer.Content;
            set => MIMETypeContainer.Content = value;
        }
        private readonly StringContainer MIMETypeContainer;

        public string FileName
        {
            get => FileNameContainer.Content;
            set => FileNameContainer.Content = value;
        }
        private readonly StringContainer FileNameContainer;

        public string ContentDescriptor
        {
            get => ContentDescriptorContainer.Content;
            set => ContentDescriptorContainer.Content = value;
        }
        private readonly StringContainer ContentDescriptorContainer;

        public ArraySegment<byte> EncapsulatedObject { get; private set; }

        public override int GetByteCount()
        {
            return MIMETypeContainer.GetByteCount() + FileNameContainer.GetByteCount() + ContentDescriptorContainer.GetByteCount() + EncapsulatedObject.Count + 1;
        }

        public override ArrayWriter WriteBytes(ArrayWriter arrayWriter)
        {
            arrayWriter.WriteByte(Encoding.ToByte());
            arrayWriter.WriteBytes(MIMETypeContainer.GetBytes());
            arrayWriter.WriteBytes(FileNameContainer.GetBytes());
            arrayWriter.WriteBytes(ContentDescriptorContainer.GetBytes());
            arrayWriter.WriteArraySegment(EncapsulatedObject);
            return arrayWriter;
        }

        public static GeneralEncapsulatedFrame CreateGeneralEncapsulatedFrame(ID3Tag tag, string contentDescriptor)
        {
            return (GeneralEncapsulatedFrame)tag.GetFrames(HeaderID.GeneralEncapsulatedObject).FirstOrDefault(F => F is GeneralEncapsulatedFrame generalEncapsulatedFrame && generalEncapsulatedFrame?.ContentDescriptor == contentDescriptor) ?? (GeneralEncapsulatedFrame)tag.AddFrame(new GeneralEncapsulatedFrame(tag, contentDescriptor));
        }

        public override bool IsEmpty()
        {
            return false;
        }
    }
}