using ExtensionsLibrary.Exceptions;
using ExtensionsLibrary.Extensions;
using System;
using System.Text;

namespace ExtensionsLibrary.Helpers
{
    public class ArrayReader
    {
        public ArrayReader(byte[] array)
        {
            Array = array;
        }

        public int Position { get; private set; }

        public int SectionStart { get; private set; }

        public int SectionCount { get; private set; }

        public byte[] Array { get; private set; }

        public bool IsZero(int count)
        {
            if (Position + count >= Array.Length) throw new ArraySizeException();
            for (int i = 0; i < count; i++) if (Array[i + Position] != 0x00) return false;
            return true;
        }

        public byte ReadByte()
        {
            return Array[Position++];
        }

        public string ReadString(int count)
        {
            return ReadString(Encoding.ASCII, count);
        }

        public string ReadString(Encoding encoding, int count)
        {
            if (Position + count >= Array.Length) throw new ArraySizeException();
            string str = encoding.GetString(Array, Position, count);
            Position += count;
            return str;
        }

        public string ReadString(Encoding sourceEncoding, Encoding targetEncoding, int count)
        {
            if (Position + count >= Array.Length) throw new ArraySizeException();
            string str = targetEncoding.GetString(Encoding.Convert(sourceEncoding, targetEncoding, Array, Position, count));
            Position += count;
            return str;
        }

        public string ReadString(Encoding encoding)
        {
            byte[] terminator = encoding.GetTerminator();
            int endIndex = Array.IndexOf(terminator, Position);
            if (endIndex < 0 || endIndex == Position)
            {
                Position += terminator.Length;
                return encoding.GetString(terminator);
            }
            int count = endIndex - Position + terminator.Length;
            string str = encoding.GetString(Array, Position, count);
            Position += count;
            return str;
        }

        public string ReadString(Encoding sourceEncoding, Encoding targetEncoding)
        {
            byte[] terminator = sourceEncoding.GetTerminator();
            int endIndex = Array.IndexOf(terminator, Position);
            if (endIndex < 0 || endIndex == Position)
            {
                Position += terminator.Length;
                return targetEncoding.GetString(terminator);
            }
            int count = endIndex - Position + terminator.Length;
            string str = targetEncoding.GetString(Encoding.Convert(sourceEncoding, targetEncoding, Array, Position, count));
            Position += count;
            return str;
        }

        public int ReadInt32()
        {
            if (Array.Length <= Position + 4) throw new ArraySizeException();
            int value = Array[Position] << 24 | Array[Position + 1] << 16 | Array[Position + 2] << 8 | Array[Position + 3];
            Position += 4;
            return value;
        }

        public int ReadInt28()
        {
            if (Array.Length <= Position + 4) throw new ArraySizeException();
            int value = Array[Position] << 21 | Array[Position + 1] << 14 | Array[Position + 2] << 7 | Array[Position + 3];
            Position += 4;
            return value;
        }

        public int ReadInt16()
        {
            if (Array.Length <= Position + 4) throw new ArraySizeException();
            int value = Array[Position] << 8 | Array[Position + 1];
            Position += 2;
            return value;
        }

        public void ReadBytes(byte[] bytes)
        {
            if (Position + bytes.Length >= Array.Length) throw new ArraySizeException();
            System.Array.Copy(Array, Position, bytes, 0, bytes.Length);
            Position += bytes.Length;
        }

        public byte[] ReadBytes(int count)
        {
            byte[] bytes = new byte[count];
            ReadBytes(bytes);
            return bytes;
        }

        public byte[] ReadBytes()
        {
            if (SectionStart + SectionCount < Position || SectionStart + SectionCount >= Array.Length) throw new ArraySizeException();
            byte[] bytes = new byte[SectionStart + SectionCount - Position];
            ReadBytes(bytes);
            return bytes;
        }

        public ArraySegment<byte> GetArraySegment(int count)
        {
            ArraySegment<byte> segment = new ArraySegment<byte>(Array, Position, count);
            Position += count;
            return segment;
        }

        public ArraySegment<byte> GetArraySegment()
        {
            if (SectionStart + SectionCount < Position || SectionStart + SectionCount >= Array.Length) throw new ArraySizeException();
            return GetArraySegment(SectionStart + SectionCount - Position);
        }

        public ArraySegment<byte> GetArraySegment(Encoding encoding)
        {
            byte[] terminator = encoding.GetTerminator();
            int endIndex = Array.IndexOf(terminator, Position);
            int count = endIndex - Position + terminator.Length;
            ArraySegment<byte> temp = new ArraySegment<byte>(Array, Position, count);
            Position += count;
            return temp;
        }

        public ArraySegment<byte> GetSection()
        {
            return new ArraySegment<byte>(Array, SectionStart, SectionCount);
        }

        public void Seek(int position)
        {
            if (position < Array.Length && position >= 0) Position = position;
        }

        public ArrayReader Section(int count)
        {
            SectionStart = Position;
            SectionCount = count;
            return this;
        }

        public void SkipSection()
        {
            Position = SectionStart + SectionCount;
        }
    }
}