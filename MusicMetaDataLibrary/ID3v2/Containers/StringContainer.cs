using MusicMetaDataLibrary.ID3v2.Extensions;
using System;
using System.Text;

namespace MusicMetaDataLibrary.ID3v2.Containers
{
    public class StringContainer
    {
        public StringContainer(Encoding encoding, bool terminated) : this(string.Empty, encoding, terminated)
        {

        }

        public StringContainer(string content, Encoding encoding, bool terminated)
        {
            NewEncoding = OriginalEncoding = encoding;
            Terminated = terminated;
            Array = null;
            Content = content;
        }

        public StringContainer(ArraySegment<byte> array, bool terminated) : this(array, Encoding.UTF8, terminated)
        {

        }

        public StringContainer(ArraySegment<byte> array, Encoding encoding, bool terminated)
        {
            Array = array;
            CacheOK = true;
            OriginalEncoding = encoding;
            Terminated = terminated;
        }

        public string Content
        {
            get => content ??= (Array != null && CacheOK) ? OriginalEncoding.GetString(Array.Array, Array.Offset, Array.Count).Trim(new[] { '\0', '\uFFFE', '\uFEFF', '\u200B' }) : string.Empty;
            set => (content, CacheOK) = (value, false);
        }
        private string content;

        private readonly ArraySegment<byte> Array;

        public Encoding OriginalEncoding
        {
            get => originalEncoding;
            set => originalEncoding = NewEncoding = value;
        }
        private Encoding originalEncoding;

        public Encoding NewEncoding { get; set; }

        private bool Terminated { get; set; }

        private bool CacheOK { get; set; }

        public int GetByteCount()
        {
            if (CacheOK && OriginalEncoding == NewEncoding) return Array.Count;
            if (Terminated) return NewEncoding.GetByteCount(Content.AddPremableAndTermination(OriginalEncoding));
            return NewEncoding.GetByteCount(Content.AddPremable(OriginalEncoding));
        }

        public byte[] GetBytes()
        {
            if (CacheOK && OriginalEncoding == NewEncoding) return Array.ToArray();
            if (Terminated) return NewEncoding.GetBytes(Content.AddPremableAndTermination(OriginalEncoding));
            return NewEncoding.GetBytes(Content.AddPremable(OriginalEncoding));
        }
    }
}