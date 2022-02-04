namespace MusicMetaDataLibrary.ID3v2.Extensions
{
    public static class Int32Extensions
    {
        public static byte[] To32ByteArray(this int value)
        {
            byte[] buffer = new byte[4];
            buffer[0] = (byte)(value >> 24);
            buffer[1] = (byte)(value >> 16);
            buffer[2] = (byte)(value >> 8);
            buffer[3] = (byte)value;
            return buffer;
        }

        public static byte[] To28ByteArray(this int value)
        {
            byte[] buffer = new byte[4];
            buffer[0] = (byte)(0x7F & (value >> 21));
            buffer[1] = (byte)(0x7F & (value >> 14));
            buffer[2] = (byte)(0x7F & (value >> 7));
            buffer[3] = (byte)(0x7F & value);
            return buffer;
        }

        public static byte[] To16ByteArray(this int value)
        {
            byte[] buffer = new byte[2];
            buffer[0] = (byte)(value >> 8);
            buffer[1] = (byte)value;
            return buffer;
        }
    }
}