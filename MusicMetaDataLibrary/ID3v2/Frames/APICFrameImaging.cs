using MusicMetaDataLibrary.ID3v2.Exceptions;
using MusicMetaDataLibrary.ID3v2.Helpers;
using System;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace MusicMetaDataLibrary.ID3v2.Frames
{
    public partial class APICFrame
    {
        public async Task<BitmapImage> GetBitmapAsync()
        {
            if (Header.Encryption) throw new EncryptionException();
            using (InMemoryRandomAccessStream stream = new InMemoryRandomAccessStream())
            {
                using (DataWriter dataWriter = new DataWriter(stream.GetOutputStreamAt(0)))
                {
                    dataWriter.WriteBytes(ImageBuffer.ToArray());
                    await dataWriter.StoreAsync();
                }
                BitmapImage bitmapImage = new BitmapImage();
                await bitmapImage.SetSourceAsync(stream);
                return bitmapImage;
            }
        }

        public async Task<ImageBrush> GetImageBrushAsync()
        {
            ImageBrush imageBrush = new ImageBrush { ImageSource = await GetBitmapAsync() };
            return imageBrush;
        }

        public async Task<StorageFile> SaveToFileAsync(StorageFile storageFile)
        {
            using (InMemoryRandomAccessStream imageStream = new InMemoryRandomAccessStream())
            {
                using (DataWriter dataWriter = new DataWriter(imageStream.GetOutputStreamAt(0)))
                {
                    dataWriter.WriteBytes(ImageBuffer.ToArray());
                    await dataWriter.StoreAsync();
                }
                BitmapDecoder bitmapDecoder = await BitmapDecoder.CreateAsync(imageStream);
                SoftwareBitmap softwareBitmap = await bitmapDecoder.GetSoftwareBitmapAsync();
                using (IRandomAccessStream fileStream = await storageFile.OpenAsync(FileAccessMode.ReadWrite))
                {
                    BitmapEncoder bitmapEncoder = await BitmapEncoder.CreateAsync(BitmapEncoder.PngEncoderId, fileStream);
                    bitmapEncoder.SetSoftwareBitmap(softwareBitmap);
                    await bitmapEncoder.FlushAsync();
                }
            }
            return storageFile;
        }

        public async Task<StorageFile> SaveToFolderAsync(StorageFolder storageFolder, string fileName)
        {
            StorageFile file = await storageFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
            return await SaveToFileAsync(file);
        }

        public async Task SetImageAsync(StorageFile storageFile)
        {
            using (IRandomAccessStream fileStream = await storageFile.OpenAsync(FileAccessMode.Read)) await SetImageAsync(fileStream, 1000, BitmapEncoder.JpegEncoderId);
        }

        public async Task SetImageAsync(StorageFile storageFile, int size)
        {
            using (IRandomAccessStream fileStream = await storageFile.OpenAsync(FileAccessMode.Read)) await SetImageAsync(fileStream, size, BitmapEncoder.JpegEncoderId);
        }

        public async Task SetImageAsync(StorageFile storageFile, Guid encoderID)
        {
            using (IRandomAccessStream fileStream = await storageFile.OpenAsync(FileAccessMode.Read)) await SetImageAsync(fileStream, 1000, encoderID);
        }

        public async Task SetImageAsync(StorageFile storageFile, int size, Guid encoderID)
        {
            using (IRandomAccessStream fileStream = await storageFile.OpenAsync(FileAccessMode.Read)) await SetImageAsync(fileStream, size, encoderID);
        }

        public async Task SetImageAsync(IRandomAccessStream stream)
        {
            await SetImageAsync(stream, 1000, BitmapEncoder.JpegEncoderId);
        }

        public async Task SetImageAsync(IRandomAccessStream stream, Guid encoderID)
        {
            await SetImageAsync(stream, 1000, encoderID);
        }

        public async Task SetImageAsync(IRandomAccessStream fileStream, int size, Guid encoderID)
        {
            if (Header.Encryption) throw new EncryptionException();
            InMemoryRandomAccessStream stream = new InMemoryRandomAccessStream();
            BitmapEncoder bitmapEncoder = await BitmapEncoder.CreateAsync(encoderID, stream);
            bitmapEncoder.BitmapTransform.ScaledHeight = bitmapEncoder.BitmapTransform.ScaledWidth = (uint)size;
            bitmapEncoder.BitmapTransform.InterpolationMode = BitmapInterpolationMode.Fant;
            bitmapEncoder.SetSoftwareBitmap(await (await BitmapDecoder.CreateAsync(fileStream)).GetSoftwareBitmapAsync());
            await bitmapEncoder.FlushAsync();
            using (DataReader dataReader = new DataReader(stream))
            {
                await dataReader.LoadAsync((uint)stream.Size);
                byte[] imageBuffer = new byte[stream.Size];
                dataReader.ReadBytes(imageBuffer);
                ImageBuffer = new ArraySegment<byte>(imageBuffer);
            }
            MIMEType = ImageEncodingHelpers.GetMIMETypeFromEncoderID(encoderID);
        }
    }
}