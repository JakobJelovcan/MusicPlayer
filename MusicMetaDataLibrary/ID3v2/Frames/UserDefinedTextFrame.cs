using ExtensionsLibrary.Helpers;
using MusicMetaDataLibrary.ID3v2.Constants;
using MusicMetaDataLibrary.ID3v2.Containers;
using MusicMetaDataLibrary.ID3v2.Extensions;
using MusicMetaDataLibrary.ID3v2.Headers;
using MusicMetaDataLibrary.ID3v2.Helpers;
using System.Linq;
using System.Text;

namespace MusicMetaDataLibrary.ID3v2.Frames
{
    public class UserDefinedTextFrame : BaseFrame
    {
        public UserDefinedTextFrame(ID3Tag tag, string description)
        {
            Header = new FrameHeader(tag, HeaderID.UserDefinedFrame, this);
            Encoding = Encoding.Unicode;
            DescriptionContainer = new StringContainer(description, Encoding.Unicode, true);
            ContentContainer = new StringContainer(Encoding.Unicode, false);
        }

        public UserDefinedTextFrame(FrameHeader header, ArrayReader arrayReader) : base(header, arrayReader)
        {
            if (!header.Encryption && !header.Compression)
            {
                Encoding = EncodingHelpers.GetEncoding(arrayReader.ReadByte());
                DescriptionContainer = new StringContainer(arrayReader.GetArraySegment(Encoding), Encoding, true);
                ContentContainer = new StringContainer(arrayReader.GetArraySegment(), Encoding, false);
            }
        }

        public Encoding Encoding { get; private set; }

        public string Description
        {
            get => DescriptionContainer.Content;
            set => DescriptionContainer.Content = value;
        }
        private readonly StringContainer DescriptionContainer;

        public string Content
        {
            get => ContentContainer.Content;
            set => ContentContainer.Content = value;
        }
        private readonly StringContainer ContentContainer;

        public override int GetByteCount()
        {
            return 1 + DescriptionContainer.GetByteCount() + ContentContainer.GetByteCount();
        }

        public override ArrayWriter WriteBytes(ArrayWriter arrayWriter)
        {
            arrayWriter.WriteByte(Encoding.ToByte());
            arrayWriter.WriteBytes(DescriptionContainer.GetBytes());
            arrayWriter.WriteBytes(ContentContainer.GetBytes());
            return arrayWriter;
        }

        public static UserDefinedTextFrame CreateUserDefinedTextFrame(ID3Tag tag, string description)
        {
            return (UserDefinedTextFrame)tag.GetFrames(HeaderID.UserDefinedFrame).FirstOrDefault(F => F is UserDefinedTextFrame userDefinedTextFrame && userDefinedTextFrame.Description == description) ?? (UserDefinedTextFrame)tag.AddFrame(new UserDefinedTextFrame(tag, description));
        }

        public override bool IsEmpty()
        {
            return string.IsNullOrEmpty(Content);
        }
    }
}