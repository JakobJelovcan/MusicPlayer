using System;
using Windows.Graphics.Imaging;

namespace MusicMetaDataLibrary.ID3v2.Helpers
{
    public static class ImageEncodingHelpers
    {
        public static string GetMIMETypeFromEncoderID(Guid encoderID)
        {
            if (encoderID == BitmapEncoder.JpegEncoderId) return "image/jpeg";
            if (encoderID == BitmapEncoder.PngEncoderId) return "image/png";
            if (encoderID == BitmapEncoder.BmpEncoderId) return "image/bmp";
            return string.Empty;
        }
    }
}
