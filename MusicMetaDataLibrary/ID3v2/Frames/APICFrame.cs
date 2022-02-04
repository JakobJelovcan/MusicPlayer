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
    public partial class APICFrame : BaseFrame
    {
        public APICFrame(ID3Tag tag, byte pictureType)
        {
            Header = new FrameHeader(tag, HeaderID.AttachedPicture, this);
            PictureType = pictureType;
            Encoding = Encoding.Unicode;
            MIMETypeContainer = new StringContainer(Encoding.ASCII, true);
            DescriptionContainer = new StringContainer(Encoding.Unicode, true);
            ImageBuffer = new ArraySegment<byte>();
        }

        public APICFrame(FrameHeader header, ArrayReader arrayReader) : base(header, arrayReader)
        {
            if (!header.Encryption && !header.Compression)
            {
                Encoding = EncodingHelpers.GetEncoding(arrayReader.ReadByte());
                MIMETypeContainer = new StringContainer(arrayReader.GetArraySegment(Encoding.UTF8), true);
                PictureType = arrayReader.ReadByte();
                DescriptionContainer = new StringContainer(arrayReader.GetArraySegment(Encoding), Encoding, true);
                ImageBuffer = arrayReader.GetArraySegment();
            }
        }

        public Encoding Encoding { get; private set; }

        public string MIMEType
        {
            get => MIMETypeContainer.Content;
            set => MIMETypeContainer.Content = value;
        }
        private readonly StringContainer MIMETypeContainer;

        public byte PictureType { get; private set; }

        public string Description
        {
            get => DescriptionContainer.Content;
            set => DescriptionContainer.Content = value;
        }
        private readonly StringContainer DescriptionContainer;

        private ArraySegment<byte> ImageBuffer { get; set; }

        public override int GetByteCount()
        {
            return 2 + MIMETypeContainer.GetByteCount() + DescriptionContainer.GetByteCount() + ImageBuffer.Count;
        }

        public override ArrayWriter WriteBytes(ArrayWriter arrayWriter)
        {
            arrayWriter.WriteByte(Encoding.ToByte());
            arrayWriter.WriteBytes(MIMETypeContainer.GetBytes());
            arrayWriter.WriteByte(PictureType);
            arrayWriter.WriteBytes(DescriptionContainer.GetBytes());
            arrayWriter.WriteArraySegment(ImageBuffer);
            return arrayWriter;
        }

        public static APICFrame CreateAPICFrame(ID3Tag tag, byte pictureType)
        {
            return (APICFrame)tag.GetFrames(HeaderID.AttachedPicture).FirstOrDefault(F => F is APICFrame apicFrame && apicFrame?.PictureType == pictureType) ?? (APICFrame)tag.AddFrame(new APICFrame(tag, pictureType));
        }

        public override bool IsEmpty()
        {
            return false;
        }
    }
}