using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.Versioning;

namespace ColorThiefDotNet
{
    public static class ColorThief
    {
        private const int DefaultColorCount = 5;
        private const int DefaultQuality = 10;
        private const bool DefaultIgnoreWhite = true;
        private const int ColorDepth = 3;

        /// <summary>
        ///     Use the median cut algorithm to cluster similar colors and return the base color from the largest cluster.
        /// </summary>
        /// <param name="sourceImage">The source image.</param>
        /// <param name="quality">
        ///     1 is the highest quality settings. 10 is the default. There is
        ///     a trade-off between quality and speed. The bigger the number,
        ///     the faster a color will be returned but the greater the
        ///     likelihood that it will not be the visually most dominant color.
        /// </param>
        /// <param name="ignoreWhite">if set to <c>true</c> [ignore white].</param>
        /// <returns>The quantized color</returns>
#if NETCOREAPP
        [SupportedOSPlatform("windows")]
#endif
        public static QuantizedColor GetColor(Bitmap sourceImage, int quality = DefaultQuality, bool ignoreWhite = DefaultIgnoreWhite)
        {
            IEnumerable<QuantizedColor> palette = GetPaletteEnumeration(sourceImage, 256, quality, ignoreWhite);
            return new QuantizedColor(
                Color.FromArgb(
                    255,
                    (byte)palette.Average(a => a.Color.R),
                    (byte)palette.Average(a => a.Color.G),
                    (byte)palette.Average(a => a.Color.B)
                ), (int)palette.Average(a => a.Population));
        }

        /// <summary>
        ///     Use the median cut algorithm to cluster similar colors and return the base color from the largest cluster.
        /// </summary>
        /// <param name="bitmapBuffer">The buffer from a bitmap image.</param>
        /// <param name="bitmapChannel">The channel count of a bitmap buffer (usually 3 for RGB and 4 for RGBA sequentially)</param>
        /// <param name="bitmapWidth">The width of the bitmap buffer</param>
        /// <param name="bitmapHeight">The height of the bitmap buffer</param>
        /// <param name="quality">
        ///     1 is the highest quality settings. 10 is the default. There is
        ///     a trade-off between quality and speed. The bigger the number,
        ///     the faster a color will be returned but the greater the
        ///     likelihood that it will not be the visually most dominant color.
        /// </param>
        /// <param name="ignoreWhite">if set to <c>true</c> [ignore white].</param>
        /// <returns>The quantized color</returns>
        public static QuantizedColor GetColor(IntPtr bitmapBuffer, int bitmapChannel, int bitmapWidth, int bitmapHeight,
            int quality = DefaultQuality, bool ignoreWhite = DefaultIgnoreWhite)
        {
            IEnumerable<QuantizedColor> palette = GetPaletteEnumeration(bitmapBuffer, bitmapChannel, bitmapWidth, bitmapHeight, 256, quality, ignoreWhite);
            return new QuantizedColor(
                Color.FromArgb(
                    255,
                    (byte)palette.Average(a => a.Color.R),
                    (byte)palette.Average(a => a.Color.G),
                    (byte)palette.Average(a => a.Color.B)
                ), (int)palette.Average(a => a.Population));
        }

        /// <summary>
        ///     Use the median cut algorithm to enumerate a cluster of similar colors.
        /// </summary>
        /// <param name="sourceImage">The source image.</param>
        /// <param name="colorCount">The color count.</param>
        /// <param name="quality">
        ///     1 is the highest quality settings. 10 is the default. There is
        ///     a trade-off between quality and speed. The bigger the number,
        ///     the faster a color will be returned but the greater the
        ///     likelihood that it will not be the visually most dominant color.
        /// </param>
        /// <param name="ignoreWhite">if set to <c>true</c> [ignore white].</param>
        /// <returns>The enumerator of clustered quantized color</returns>
#if NETCOREAPP
        [SupportedOSPlatform("windows")]
#endif
        public static IEnumerable<QuantizedColor> GetPaletteEnumeration(Bitmap sourceImage, int colorCount = DefaultColorCount, int quality = DefaultQuality, bool ignoreWhite = DefaultIgnoreWhite)
        {
            CMap cmap = GetCMap(sourceImage, colorCount, quality, ignoreWhite);
            return cmap.GeneratePalette();
        }

        /// <summary>
        ///     Use the median cut algorithm to enumerate a cluster of similar colors.
        /// </summary>
        /// <param name="bitmapBuffer">The buffer from a bitmap image.</param>
        /// <param name="bitmapChannel">The channel count of a bitmap buffer (usually 3 for RGB and 4 for RGBA sequentially)</param>
        /// <param name="bitmapWidth">The width of the bitmap buffer</param>
        /// <param name="bitmapHeight">The height of the bitmap buffer</param>
        /// <param name="colorCount">The color count.</param>
        /// <param name="quality">
        ///     1 is the highest quality settings. 10 is the default. There is
        ///     a trade-off between quality and speed. The bigger the number,
        ///     the faster a color will be returned but the greater the
        ///     likelihood that it will not be the visually most dominant color.
        /// </param>
        /// <param name="ignoreWhite">if set to <c>true</c> [ignore white].</param>
        /// <returns>The enumerator of clustered quantized color</returns>
        public static IEnumerable<QuantizedColor> GetPaletteEnumeration(IntPtr bitmapBuffer, int bitmapChannel, int bitmapWidth, int bitmapHeight,
            int colorCount = DefaultColorCount, int quality = DefaultQuality, bool ignoreWhite = DefaultIgnoreWhite)
        {
            int bitmapStride = bitmapWidth * bitmapChannel;
            CMap cmap = GetCMap(bitmapBuffer, bitmapChannel, bitmapStride, bitmapWidth, bitmapHeight, colorCount, quality, ignoreWhite);
            return cmap.GeneratePalette();
        }

        /// <summary>
        ///     Use the median cut algorithm to cluster similar colors.
        /// </summary>
        /// <param name="sourceImage">The source image.</param>
        /// <param name="colorCount">The color count.</param>
        /// <param name="quality">
        ///     1 is the highest quality settings. 10 is the default. There is
        ///     a trade-off between quality and speed. The bigger the number,
        ///     the faster a color will be returned but the greater the
        ///     likelihood that it will not be the visually most dominant color.
        /// </param>
        /// <param name="ignoreWhite">if set to <c>true</c> [ignore white].</param>
        /// <returns>The list of quantized color</returns>
#if NETCOREAPP
        [SupportedOSPlatform("windows")]
#endif
        public static List<QuantizedColor> GetPaletteList(Bitmap sourceImage, int colorCount = DefaultColorCount, int quality = DefaultQuality, bool ignoreWhite = DefaultIgnoreWhite)
        {
            CMap cmap = GetCMap(sourceImage, colorCount, quality, ignoreWhite);
            return cmap.GeneratePaletteList();
        }


        /// <summary>
        ///     Use the median cut algorithm to cluster similar colors from a bitmap buffer.
        /// </summary>
        /// <param name="bitmapBuffer">The buffer from a bitmap image.</param>
        /// <param name="bitmapChannel">The channel count of a bitmap buffer (usually 3 for RGB and 4 for RGBA sequentially)</param>
        /// <param name="bitmapWidth">The width of the bitmap buffer</param>
        /// <param name="bitmapHeight">The height of the bitmap buffer</param>
        /// <param name="colorCount">The color count.</param>
        /// <param name="quality">
        ///     1 is the highest quality settings. 10 is the default. There is
        ///     a trade-off between quality and speed. The bigger the number,
        ///     the faster a color will be returned but the greater the
        ///     likelihood that it will not be the visually most dominant color.
        /// </param>
        /// <param name="ignoreWhite">if set to <c>true</c> [ignore white].</param>
        /// <returns>The list of quantized color</returns>
        public static List<QuantizedColor> GetPaletteList(IntPtr bitmapBuffer, int bitmapChannel, int bitmapWidth, int bitmapHeight, int colorCount = DefaultColorCount, int quality = DefaultQuality, bool ignoreWhite = DefaultIgnoreWhite)
        {
            int bitmapStride = bitmapWidth * bitmapChannel;
            CMap cmap = GetCMap(bitmapBuffer, bitmapChannel, bitmapStride, bitmapWidth, bitmapHeight, colorCount, quality, ignoreWhite);
            return cmap.GeneratePaletteList();
        }

#if NETCOREAPP
        [SupportedOSPlatform("windows")]
#endif
        private static CMap GetCMap(Bitmap sourceImage, int colorCount, int quality, bool ignoreWhite)
        {
            int _quality = quality < 1 ? DefaultQuality : quality;
            int _colorCount = colorCount <= 0 ? 1 : colorCount;
            IEnumerable<int> pixelEnumerable = EnumeratePixels(sourceImage, _quality);
            return Mmcq.Quantize(pixelEnumerable, _colorCount, ignoreWhite);
        }

        private static CMap GetCMap(IntPtr bitmapBuffer, int bitmapChannel, int bitmapStride, int bitmapWidth, int bitmapHeight, int colorCount, int quality, bool ignoreWhite)
        {
            int _quality = quality < 1 ? DefaultQuality : quality;
            int _colorCount = colorCount <= 0 ? 1 : colorCount;
            IEnumerable<int> pixelEnumerable = EnumeratePixelsInner(bitmapBuffer, bitmapChannel, bitmapStride, bitmapWidth, bitmapHeight, _quality);
            return Mmcq.Quantize(pixelEnumerable, _colorCount, ignoreWhite);
        }

#if NETCOREAPP
        [SupportedOSPlatform("windows")]
#endif
        private static IEnumerable<int> EnumeratePixels(Bitmap sourceImage, int quality)
        {
#if NETCOREAPP
            int chanCount = sourceImage.PixelFormat switch
            {
                PixelFormat.Format32bppRgb => 4,
                PixelFormat.Format32bppArgb => 4,
                PixelFormat.Format24bppRgb => 3,
                _ => throw new NotSupportedException($"Pixel format of the image: {sourceImage.PixelFormat} is unsupported!")
            };
#else
            int chanCount;
            switch (sourceImage.PixelFormat)
            {
                case PixelFormat.Format32bppRgb:
                case PixelFormat.Format32bppArgb:
                    chanCount = 4;
                    break;
                case PixelFormat.Format24bppRgb:
                    chanCount = 3;
                    break;
                default:
                    throw new NotSupportedException($"Pixel format of the image: {sourceImage.PixelFormat} is unsupported!");
            }
#endif
            BitmapData data = sourceImage.LockBits(new Rectangle(new Point(), sourceImage.Size), ImageLockMode.ReadOnly, sourceImage.PixelFormat);
            int bufferLength = data.Width * data.Height * chanCount;

            try
            {
                return EnumeratePixelsInner(data.Scan0, chanCount, data.Stride, data.Width, data.Height, quality);
            }
            finally
            {
                sourceImage.UnlockBits(data);
            }
        }

        private static IEnumerable<int> EnumeratePixelsInner(IntPtr bufferPtr, int channelCount, int stride, int width, int height, int quality)
        {
            int stepX = 0;
            int pixel;
            int offset;

            if (channelCount == 4)
            {
                for (int y = 0; y < height; y += quality)
                {
                    for (int x = stepX; x < width; x += quality)
                    {
                        offset = stride * y + channelCount * x;
                        pixel = GetUnsafeSinglePixelWithAlpha(bufferPtr, offset);

                        if (width - x < quality) stepX = quality - (width - x);
                        yield return pixel;
                    }
                }
            }
            else
            {
                for (int y = 0; y < height; y += quality)
                {
                    for (int x = stepX; x < width; x += quality)
                    {
                        offset = stride * y + channelCount * x;
                        pixel = GetUnsafeSinglePixel(bufferPtr, offset);

                        if (width - x < quality) stepX = quality - (width - x);
                        yield return pixel;
                    }
                }
            }
        }

        private static unsafe Span<T> CreateSpanFromIntPtr<T>(IntPtr ptr, int length)
            where T : unmanaged
        {
            T* bytePtr = (T*)ptr;
            return new Span<T>(bytePtr, length);
        }

        private static unsafe int GetUnsafeSinglePixel(IntPtr ptr, int offset) => *(byte*)(ptr + (offset + 2)) | (*(byte*)(ptr + (offset + 1)) << 8) | (*(byte*)(ptr + (offset + 0)) << 16) | (255 << 24);
        private static unsafe int GetUnsafeSinglePixelWithAlpha(IntPtr ptr, int offset) => *(byte*)(ptr + (offset + 2)) | (*(byte*)(ptr + (offset + 1)) << 8) | (*(byte*)(ptr + (offset + 0)) << 16) | (*(byte*)(ptr + (offset + 3)) << 24);
    }
}
