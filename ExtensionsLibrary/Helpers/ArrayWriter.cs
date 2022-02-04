using ExtensionsLibrary.Exceptions;
using System;
using System.Text;

namespace ExtensionsLibrary.Helpers
{
    public class ArrayWriter
    {
        public ArrayWriter(byte[] array)
        {
            Array = array;
        }

        public ArrayWriter(int count)
        {
            Array = new byte[count];
        }

        public byte[] Array { get; private set; }

        public int Position { get; private set; }

        public void WriteArraySegment(ArraySegment<byte> value)
        {
            if (Position + value.Count > Array.Length) throw new ArraySizeException();
            System.Array.Copy(value.Array, value.Offset, Array, Position, value.Count);
            Position += value.Count;
        }

        public void WriteString(string value, Encoding encoding)
        {
            WriteBytes(encoding.GetBytes(value));
        }

        public void WriteString(string value)
        {
            WriteBytes(Encoding.ASCII.GetBytes(value));
        }

        public void WriteBytes(byte[] value)
        {
            if (Position + value.Length > Array.Length) throw new ArraySizeException();
            System.Array.Copy(value, 0, Array, Position, value.Length);
            Position += value.Length;
        }

        public void WriteByte(byte value)
        {
            if (Position + 1 > Array.Length) throw new ArraySizeException();
            Array[Position++] = value;
        }

        public void WriteInt32(int value)
        {
            if (Position + 4 > Array.Length) throw new ArraySizeException();
            Array[Position++] = (byte)(value >> 24);
            Array[Position++] = (byte)(value >> 16);
            Array[Position++] = (byte)(value >> 8);
            Array[Position++] = (byte)value;
        }

        public void WriteInt28(int value)
        {
            if (Position + 4 > Array.Length) throw new ArraySizeException();
            Array[Position++] = (byte)(0x7F & (value >> 21));
            Array[Position++] = (byte)(0x7F & (value >> 14));
            Array[Position++] = (byte)(0x7F & (value >> 7));
            Array[Position++] = (byte)(0x7F & value);
        }

        public void WriteInt16(short value)
        {
            if (Position + 2 > Array.Length) throw new ArraySizeException();
            Array[Position++] = (byte)(value >> 8);
            Array[Position++] = (byte)value;
        }

        public void Seek(int position)
        {
            if (position > 0 && position < Array.Length) Position = position;
            else throw new ArraySizeException();
        }
    }
}