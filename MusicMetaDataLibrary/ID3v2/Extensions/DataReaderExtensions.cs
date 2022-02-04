using Windows.Storage.Streams;

namespace MusicMetaDataLibrary.ID3v2.Extensions
{
    public static class DataReaderExtensions
    {
        public static int ReadInt28(this DataReader dataReader)
        {
            byte[] array = new byte[4];
            dataReader.ReadBytes(array);
            return array[0] << 21 | array[1] << 14 | array[2] << 7 | array[3];
        }
    }
}
