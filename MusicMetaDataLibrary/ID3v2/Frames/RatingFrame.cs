using ExtensionsLibrary.Helpers;
using MusicMetaDataLibrary.ID3v2.Constants;
using MusicMetaDataLibrary.ID3v2.Containers;
using MusicMetaDataLibrary.ID3v2.Headers;
using System;
using System.Linq;
using System.Text;

namespace MusicMetaDataLibrary.ID3v2.Frames
{
    public class RatingFrame : BaseFrame
    {
        public RatingFrame(ID3Tag tag, string email)
        {
            Header = new FrameHeader(tag, HeaderID.Rating, this);
            EmailContainer = new StringContainer(email, Encoding.ASCII, true);
            Rating = 0;
            Counter = new byte[4];
        }

        public RatingFrame(FrameHeader header, ArrayReader arrayReader) : base(header, arrayReader)
        {
            if (!header.Encryption && !header.Compression)
            {
                EmailContainer = new StringContainer(arrayReader.GetArraySegment(Encoding.ASCII), Encoding.ASCII, true);
                Rating = arrayReader.ReadByte();
                Counter = arrayReader.ReadBytes();
            }
        }

        public string Email
        {
            get => EmailContainer.Content;
            set => EmailContainer.Content = value;
        }
        private readonly StringContainer EmailContainer;

        public uint Rating
        {
            get => rating;
            set => rating = (byte)Math.Min(255, Math.Max(0, value));
        }
        private byte rating;

        public byte[] Counter { get; private set; }

        public override int GetByteCount()
        {
            return EmailContainer.GetByteCount() + 1 + Counter.Length;
        }

        public override ArrayWriter WriteBytes(ArrayWriter arrayWriter)
        {
            arrayWriter.WriteBytes(EmailContainer.GetBytes());
            arrayWriter.WriteByte(rating);
            arrayWriter.WriteBytes(Counter);
            return arrayWriter;
        }

        public static RatingFrame CreateRatingFrame(ID3Tag tag, string email)
        {
            return (RatingFrame)tag.GetFrames(HeaderID.Rating).FirstOrDefault(F => F is RatingFrame ratingFrame && ratingFrame?.Email == email) ?? (RatingFrame)tag.AddFrame(new RatingFrame(tag, email));
        }

        public override bool IsEmpty()
        {
            return false;
        }
    }
}