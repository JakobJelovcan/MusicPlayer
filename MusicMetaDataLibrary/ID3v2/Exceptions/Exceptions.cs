using System;

namespace MusicMetaDataLibrary.ID3v2.Exceptions
{
    public class ReadOnlyException : Exception
    {
        public ReadOnlyException() : base()
        {

        }

        public ReadOnlyException(string message) : base(message)
        {

        }
    }

    public class DuplicateFrameHeaderExeption : Exception
    {
        public DuplicateFrameHeaderExeption() : base()
        {

        }

        public DuplicateFrameHeaderExeption(string message) : base(message)
        {

        }
    }

    public class InvalidTagIDException : Exception
    {
        public InvalidTagIDException() : base()
        {

        }

        public InvalidTagIDException(string message) : base(message)
        {

        }
    }

    public class UnsynchronizationNotSupportedException : Exception
    {
        public UnsynchronizationNotSupportedException() : base()
        {

        }

        public UnsynchronizationNotSupportedException(string message) : base(message)
        {

        }
    }

    public class EncryptionException : Exception
    {
        public EncryptionException() : base()
        {

        }

        public EncryptionException(string message) : base(message)
        {

        }
    }

    public class InvalidTagVersionException : Exception
    {
        public InvalidTagVersionException() : base()
        {

        }

        public InvalidTagVersionException(string message) : base(message)
        {

        }
    }
}