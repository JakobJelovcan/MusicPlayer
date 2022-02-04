using Windows.Storage.Streams;

namespace ExtensionsLibrary.Extensions
{
    public static class DataReaderExtensions
    {
        public static byte[] ReadBytes(this DataReader dataReader, int size)
        {
            byte[] array = new byte[size];
            dataReader.ReadBytes(array);
            return array;
        }
    }
}
