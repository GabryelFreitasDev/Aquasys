using SkiaSharp;

namespace Aquasys.Core.Utils
{
    public static class DCImageUtils
    {
        public const int QUANTIDADE_MAXIMA_IMAGENS = 7;

        public static readonly List<string> ImageExtensions = new List<string> { "JPG", "JPEG", "JPE", "BMP", "GIF", "PNG" };

        private static readonly int TAMANHO_MAXIMO_EM_KILOBYTES_PADRAO = 100;


        public static async Task<byte[]> ResizeImageAsync(byte[] image, bool fromCamera)
        {
            //int? maxSizePixel = await GetTamanhoMaximoEmPixelAsync();
            //int imageQuality = await GetTamanhoQualidadeAsync(); // Método tenta utilizar 100% de qualidade, caso não consiga vai reduzindo até 50%.
            int maxSizeBytes = await GetTamanhoMaximoEmBytesAsync();

            return ResizeImage(image, 1024,  1024, maxSizeBytes, fromCamera);
        }

        private static SKEncodedOrigin GetImageOrientation(byte[] imageBytes)
        {
            using var stream = new MemoryStream(imageBytes);
            using var codec = SKCodec.Create(stream);
            if (codec.Info != null)
                return codec.EncodedOrigin;
            return SKEncodedOrigin.Default;
        }

        public static byte[] ResizeImage(byte[] imageBytes, int maxWidth, int maxHeight, int maxSizeInBytes, bool fromCamera)
        {
            SKBitmap bitmap = SKBitmap.Decode(imageBytes);

            var exifOrientation = GetImageOrientation(imageBytes);

            if (exifOrientation == SKEncodedOrigin.Default && DeviceInfo.Platform == DevicePlatform.iOS && fromCamera)
            {
                exifOrientation = SKEncodedOrigin.RightTop;
            }

            switch (exifOrientation)
            {

                case SKEncodedOrigin.BottomRight:
                    using (var surface = new SKCanvas(bitmap))
                    {
                        surface.RotateDegrees(180, bitmap.Width / 2, bitmap.Height / 2);
                        surface.DrawBitmap(bitmap.Copy(), 0, 0);
                    }
                    break;

                case SKEncodedOrigin.RightTop:
                    var rotated = new SKBitmap(bitmap.Height, bitmap.Width);

                    using (var surface = new SKCanvas(rotated))
                    {
                        surface.Translate(rotated.Width, 0);
                        surface.RotateDegrees(90);
                        surface.DrawBitmap(bitmap, 0, 0);
                    }
                    bitmap = rotated;
                    break;

                case SKEncodedOrigin.LeftBottom:
                    var rotated2 = new SKBitmap(bitmap.Height, bitmap.Width);

                    using (var surface = new SKCanvas(rotated2))
                    {
                        surface.Translate(0, rotated2.Height);
                        surface.RotateDegrees(270);
                        surface.DrawBitmap(bitmap, 0, 0);
                    }
                    bitmap = rotated2;
                    break;
                case SKEncodedOrigin.TopLeft:
                case SKEncodedOrigin.TopRight:
                case SKEncodedOrigin.RightBottom:
                case SKEncodedOrigin.BottomLeft:
                case SKEncodedOrigin.LeftTop:
                    break;
            }

            int newWidth, newHeight;
            if (bitmap.Width > bitmap.Height)
            {
                newWidth = maxWidth;
                newHeight = (int)(((float)bitmap.Height / bitmap.Width) * maxWidth);
            }
            else
            {
                newWidth = (int)(((float)bitmap.Width / bitmap.Height) * maxHeight);
                newHeight = maxHeight;
            }

            using (SKBitmap resizedBitmap = bitmap.Resize(new SKImageInfo(newWidth, newHeight), SKFilterQuality.High))
            {
                bitmap.Dispose();
                using (SKData compressedData = resizedBitmap.Encode(SKEncodedImageFormat.Jpeg, 100))
                {
                    if (compressedData.Size <= maxSizeInBytes)
                        return compressedData.ToArray();

                    if (compressedData.Size > maxSizeInBytes)
                    {
                        for (int quality = 99; quality >= 50; quality -= 2)
                        {
                            using (SKData tempData = resizedBitmap.Encode(SKEncodedImageFormat.Jpeg, quality))
                            {
                                if (tempData.Size <= maxSizeInBytes)
                                {
                                    compressedData.Dispose();
                                    return tempData.ToArray();
                                }
                            }
                        }
                    }

                    return null;
                }
            }

        }

        private async static Task<int> GetTamanhoMaximoEmBytesAsync()
        {
            //try
            //{
            //    return await GetTamanhoMaximoEmKbytesAsync() * 1024;
            //}
            //catch
            //{
                return TAMANHO_MAXIMO_EM_KILOBYTES_PADRAO * 1024;
            //}
        }
    }
}
