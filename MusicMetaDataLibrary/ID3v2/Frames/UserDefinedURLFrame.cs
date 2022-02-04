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
    public class UserDefinedURLFrame : BaseFrame
    {
        public UserDefinedURLFrame(ID3Tag tag, string description)
        {
            Header = new FrameHeader(tag, HeaderID.UserDefinedLinkFrame, this);
            Encoding = Encoding.Unicode;
            DescriptionContainer = new StringContainer(description, Encoding, true);
            URLContainer = new StringContainer(Encoding.ASCII, false);
        }

        public UserDefinedURLFrame(FrameHeader header, ArrayReader arrayReader) : base(header, arrayReader)
        {
            if (!header.Encryption && !header.Compression)
            {
                Encoding = EncodingHelpers.GetEncoding(arrayReader.ReadByte());
                DescriptionContainer = new StringContainer(arrayReader.GetArraySegment(Encoding), Encoding, true);
                URLContainer = new StringContainer(arrayReader.GetArraySegment(), Encoding, false);
            }
        }

        public Encoding Encoding { get; private set; }

        public string Description
        {
            get => DescriptionContainer.Content;
            set => DescriptionContainer.Content = value;
        }
        private readonly StringContainer DescriptionContainer;

        public string URL
        {
            get => URLContainer.Content;
            set => URLContainer.Content = value;
        }
        private readonly StringContainer URLContainer;

        public override int GetByteCount()
        {
            return 1 + DescriptionContainer.GetByteCount() + URLContainer.GetByteCount();
        }

        public override ArrayWriter WriteBytes(ArrayWriter arrayWriter)
        {
            arrayWriter.WriteByte(Encoding.ToByte());
            arrayWriter.WriteBytes(DescriptionContainer.GetBytes());
            arrayWriter.WriteBytes(URLContainer.GetBytes());
            return arrayWriter;
        }

        public static UserDefinedURLFrame CreateUserDefinedURLFrame(ID3Tag tag, string description)
        {
            return (UserDefinedURLFrame)tag.GetFrames(HeaderID.UserDefinedLinkFrame).FirstOrDefault(F => F is UserDefinedURLFrame userDefinedURLFrame && userDefinedURLFrame.Description == description) ?? (UserDefinedURLFrame)tag.AddFrame(new UserDefinedURLFrame(tag, description));
        }

        public override bool IsEmpty()
        {
            return string.IsNullOrEmpty(URL);
        }
    }
}