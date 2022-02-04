using ExtensionsLibrary.Extensions;
using ExtensionsLibrary.Helpers;
using MusicMetaDataLibrary.ID3v2.Constants;
using MusicMetaDataLibrary.ID3v2.Exceptions;
using MusicMetaDataLibrary.ID3v2.Frames;
using MusicMetaDataLibrary.ID3v2.Headers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;

namespace MusicMetaDataLibrary.ID3v2
{
    public partial class ID3Tag
    {
        public ID3Tag()
        {
            FrameHeaders = new List<FrameHeader>();
        }

        private TagHeader Header { get; set; }

        private ExtendedTagHeader ExtendedHeader { get; set; }

        private List<FrameHeader> FrameHeaders { get; set; }

        private StorageFile SongFile { get; set; }

        public FrameHeader GetFrameHeader(HeaderID headerID)
        {
            return FrameHeaders.FirstOrDefault(H => H.HeaderID == headerID);
        }

        public IEnumerable<FrameHeader> GetFrameHeaders(HeaderID headerID)
        {
            return FrameHeaders.Where(H => H.HeaderID == headerID);
        }

        public BaseFrame GetFrame(HeaderID headerID)
        {
            return GetFrameHeader(headerID)?.Frame;
        }

        public IEnumerable<BaseFrame> GetFrames(HeaderID headerID)
        {
            return GetFrameHeaders(headerID)?.Select(H => H?.Frame);
        }

        internal BaseFrame AddFrame(BaseFrame frame)
        {
            FrameHeaders.Add(frame.Header);
            return frame;
        }

        internal int GetCompleteTagSize()
        {
            return GetTagSize() + GetPaddingSize();
        }

        private int GetTagSize()
        {
            return FrameHeaders.Where(H => !H.Frame.IsEmpty()).Sum(H => H.GetByteCount());
        }

        internal int GetPaddingSize()
        {
            int sizeDifference = Header.TagSize - GetTagSize();
            return Math.Min(sizeDifference < 0 ? 1024 : sizeDifference, 1024);
        }

        private byte[] GetTagBytes()
        {
            ArrayWriter arrayWriter = new ArrayWriter(GetCompleteTagSize() + Header.HeaderSize);
            Header.WriteBytes(arrayWriter);
            if (Header.ExtendedHeader) ExtendedHeader.WriteBytes(arrayWriter);
            FrameHeaders.Where(H => !H.Frame.IsEmpty()).ForEach(H => H.WriteBytes(arrayWriter));
            return arrayWriter.Array;
        }

        public async Task LoadTagAsync(StorageFile storageFile)
        {
            SongFile = storageFile;
            using (IRandomAccessStream fileStream = await storageFile.OpenAsync(FileAccessMode.Read))
            {
                using (DataReader dataReader = new DataReader(fileStream.GetInputStreamAt(0)))
                {
                    dataReader.ByteOrder = ByteOrder.BigEndian;
                    dataReader.UnicodeEncoding = UnicodeEncoding.Utf8;
                    await dataReader.LoadAsync(10);
                    Header = new TagHeader(this, dataReader);
                    if (Header.Unsynchronization) throw new UnsynchronizationNotSupportedException();
                    await dataReader.LoadAsync((uint)Header.TagSize);
                    ArrayReader arrayReader = new ArrayReader(dataReader.ReadBytes(Header.TagSize));
                    ExtendedHeader = Header.ExtendedHeader ? new ExtendedTagHeader(this, arrayReader) : null;
                    while (arrayReader.Position < Header.TagSize)
                    {
                        if (!arrayReader.IsZero(4)) FrameHeaders.Add(new FrameHeader(this, arrayReader));
                        else break;
                    }
                    ExtendedHeader ??= new ExtendedTagHeader(this, Header.TagSize - arrayReader.Position);
                    dataReader.Dispose();
                }
                fileStream.Dispose();
            }
        }

        public async Task SaveTagAsync()
        {
            if (GetCompleteTagSize() != Header.TagSize) await RewriteTagAsync();
            else await OverwriteTagAsync();
        }

        private async Task OverwriteTagAsync()
        {
            using (IRandomAccessStream stream = await SongFile.OpenAsync(FileAccessMode.ReadWrite))
            {
                using (IOutputStream outputStream = stream.GetOutputStreamAt(0))
                {
                    using (DataWriter dataWriter = new DataWriter(outputStream))
                    {
                        dataWriter.WriteBytes(GetTagBytes());
                        await dataWriter.StoreAsync();
                        await outputStream.FlushAsync();
                        outputStream.Dispose();
                        dataWriter.Dispose();
                    }
                }
                stream.Dispose();
            }
        }

        private async Task RewriteTagAsync()
        {
            byte[] tagBytes = GetTagBytes();
            byte[] audioBytes = await GetAudioBytesAsync();
            ArrayWriter arrayWriter = new ArrayWriter(tagBytes.Length + audioBytes.Length);
            arrayWriter.WriteBytes(tagBytes);
            arrayWriter.WriteBytes(audioBytes);
            await FileIO.WriteBytesAsync(SongFile, arrayWriter.Array);
        }

        private async Task<byte[]> GetAudioBytesAsync()
        {
            using (IRandomAccessStream fileStream = await SongFile.OpenAsync(FileAccessMode.Read))
            {
                using (DataReader dataReader = new DataReader(fileStream.GetInputStreamAt((uint)Header.CompleteTagSize)))
                {
                    int count = (int)fileStream.Size - Header.CompleteTagSize;
                    await dataReader.LoadAsync((uint)count);
                    byte[] bytes = dataReader.ReadBytes(count);
                    dataReader.Dispose();
                    fileStream.Dispose();
                    return bytes;
                }
            }
        }

        public void RemoveFrame(FrameHeader header)
        {
            FrameHeaders.Remove(header);
        }

        public void Clear()
        {
            foreach (FieldInfo field in GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly)) if (Nullable.GetUnderlyingType(field.FieldType) is not null) field.SetValue(this, null);
            FrameHeaders = new List<FrameHeader>();
        }
    }
}