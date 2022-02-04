using ExtensionsLibrary.Extensions;
using ExtensionsLibrary.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace MusicMetaDataLibrary.ID3v2.Containers
{
    public class StringListContainer
    {
        public StringListContainer(Encoding encoding) : this(new ArraySegment<byte>(), encoding)
        {

        }

        public StringListContainer(ArraySegment<byte> array, Encoding encoding)
        {
            Array = array;
            OriginalEncoding = encoding;
        }

        private ArraySegment<byte> Array { get; set; }

        public IEnumerable<string> Collection
        {
            get => collection ??= (Array != null && CacheOK) ? Array.ToTerminatedStringCollection(OriginalEncoding) : new List<string>();
            set
            {
                if (value != collection)
                {
                    collection = value;
                    CacheOK = false;
                }
            }
        }
        private IEnumerable<string> collection;

        public Encoding OriginalEncoding
        {
            get => originalEncoding;
            set => originalEncoding = NewEncoding = value;
        }
        private Encoding originalEncoding;

        public Encoding NewEncoding { get; set; }

        public bool CacheOK { get; private set; }

        public int GetByteCount()
        {
            return (CacheOK && OriginalEncoding == NewEncoding) ? Array.Count : NewEncoding.GetByteCount(string.Join("\0", Collection));
        }

        public ArrayWriter WriteBytes(ArrayWriter arrayWriter)
        {
            if (CacheOK && OriginalEncoding == NewEncoding) arrayWriter.WriteArraySegment(Array);
            else arrayWriter.WriteString(string.Join("\0", Collection), NewEncoding);
            return arrayWriter;
        }
    }
}