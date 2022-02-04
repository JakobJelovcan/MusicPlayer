using System;

namespace ExtensionsLibrary.Exceptions
{
    public class ArraySizeException : Exception
    {
        public ArraySizeException() : base()
        {
        }

        public ArraySizeException(string message) : base(message)
        {
        }
    }

    public class StreamSizeException : Exception
    {
        public StreamSizeException() : base()
        {

        }

        public StreamSizeException(string message) : base(message)
        {

        }
    }
}