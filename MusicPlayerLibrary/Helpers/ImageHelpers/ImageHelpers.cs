using ExtensionsLibrary.Extensions;
using MusicPlayerLibrary.Constants;
using MusicPlayerLibrary.Helpers.ClusteringHelpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.UI;

namespace MusicPlayerLibrary.Helpers.ImageHelpers
{
    public static class ImageHelpers
    {
        public static async Task<IEnumerable<Vector3>> GetColorClustersAsync(StorageFile imageFile, ColorType colorType)
        {
            List<Vector3> vectors = await GetImagePixelsAsync(imageFile).ToListAsync();
            return KMeansClustering.ClusterData(GetBaseVectors(colorType, vectors), vectors);
        }

        public static async Task<IEnumerable<Vector3>> GetColorClustersAsync(StorageItemThumbnail thumbnail, ColorType colorType)
        {
            List<Vector3> vectors = await GetThumbnailPixelsAsync(thumbnail).ToListAsync();
            return KMeansClustering.ClusterData(GetBaseVectors(colorType, vectors), vectors);
        }

        private static async IAsyncEnumerable<Vector3> GetImagePixelsAsync(StorageFile imageFile, uint size = 80)
        {
            BitmapTransform bitmapTransform = new BitmapTransform { ScaledHeight = size, ScaledWidth = size, InterpolationMode = BitmapInterpolationMode.Cubic };
            byte[] pixelData = (await (await BitmapDecoder.CreateAsync((await imageFile.OpenStreamForReadAsync()).AsRandomAccessStream())).GetPixelDataAsync(BitmapPixelFormat.Bgra8, BitmapAlphaMode.Premultiplied, bitmapTransform, ExifOrientationMode.RespectExifOrientation, ColorManagementMode.ColorManageToSRgb)).DetachPixelData();
            for (int i = 0; i < pixelData.Length - 4; i += 4) yield return new Vector3(pixelData[i + 2], pixelData[i + 1], pixelData[i]);
        }

        private static async IAsyncEnumerable<Vector3> GetThumbnailPixelsAsync(StorageItemThumbnail thumbnail, uint size = 80)
        {
            BitmapTransform bitmapTransform = new BitmapTransform { ScaledHeight = size, ScaledWidth = size, InterpolationMode = BitmapInterpolationMode.Cubic };
            byte[] pixelData = (await (await BitmapDecoder.CreateAsync(thumbnail)).GetPixelDataAsync(BitmapPixelFormat.Bgra8, BitmapAlphaMode.Premultiplied, bitmapTransform, ExifOrientationMode.RespectExifOrientation, ColorManagementMode.ColorManageToSRgb)).DetachPixelData();
            for (int i = 0; i < pixelData.Length - 4; i += 4) yield return new Vector3(pixelData[i + 2], pixelData[i + 1], pixelData[i]);
        }

        private static List<Vector3> GetNeutralBaseVectors()
        {
            return new List<Vector3>
            {
                Colors.White.ToVector3(),
                Colors.Gray.ToVector3(),
                Colors.Black.ToVector3(),
                Colors.Red.ToVector3(),
                Colors.Lime.ToVector3(),
                Colors.Blue.ToVector3()
            };
        }

        private static List<Vector3> GetRandomBaseVectors(List<Vector3> vectors)
        {
            return vectors.Randomize().Take(6).ToList();
        }

        private static List<Vector3> GetGrayScaleBaseVectors()
        {
            return new List<Vector3>
            {
                new Vector3(0x00, 0x00, 0x00),
                new Vector3(0x33, 0x33, 0x33),
                new Vector3(0x66, 0x66, 0x66),
                new Vector3(0x99, 0x99, 0x99),
                new Vector3(0xCC, 0xCC, 0xCC),
                new Vector3(0xFF, 0xFF, 0xFF)
            };
        }

        private static List<Vector3> GetBaseVectors(ColorType colorType, List<Vector3> vectors)
        {
            switch (colorType)
            {
                case ColorType.Random: return GetRandomBaseVectors(vectors);
                case ColorType.Neutral: return GetNeutralBaseVectors();
                case ColorType.Grayscale: return GetGrayScaleBaseVectors();
                default: throw new DataException();
            }
        }
    }
}