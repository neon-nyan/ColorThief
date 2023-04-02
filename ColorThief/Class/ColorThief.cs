using System;
using System.Drawing;

namespace ColorThiefDotNet
{
    public partial class ColorThief
    {
        public const int DefaultColorCount = 5;
        public const int DefaultQuality = 10;
        public const bool DefaultIgnoreWhite = true;
        public const int ColorDepth = 3;

        /// <summary>
        ///     Use the median cut algorithm to cluster similar colors.
        /// </summary>
        /// <param name="pixelArray">Pixel array.</param>
        /// <param name="colorCount">The color count.</param>
        /// <returns></returns>
        private CMap GetColorMap(byte[][] pixelArray, int colorCount)
        {
            // Send array to quantize function which clusters values using median
            // cut algorithm

            if (colorCount > 0)
            {
                --colorCount;
            }

            return Mmcq.Quantize(pixelArray, colorCount);
        }

        private byte[][] GetPixels(Bitmap sourceImage, int quality, bool ignoreWhite)
        {
            int imgWidth = sourceImage.Width;
            int imgHeight = sourceImage.Height;
            int pixelCount = imgWidth * imgHeight;

            // Store the RGB values in an array format suitable for quantize
            // function

            // numRegardedPixels must be rounded up to avoid an
            // ArrayIndexOutOfBoundsException if all pixels are good.
            int numRegardedPixels = (pixelCount + quality - 1) / quality;

            int numUsedPixels = 0;
            byte[][] pixelArray = new byte[numRegardedPixels][];

            int xstartOffset = 0;

            for (int y = 0; y < imgHeight; y++)
            {
                for (int x = 0 + (Math.Max(imgWidth, xstartOffset) - imgWidth); x < imgWidth; x += quality, xstartOffset = x)
                {
                    Color pixel = sourceImage.GetPixel(x, y);
                    byte r = pixel.R;
                    byte g = pixel.G;
                    byte b = pixel.B;

                    // If pixel is mostly opaque and not white
                    if (!(ignoreWhite && r > 250 && g > 250 && b > 250))
                    {
                        pixelArray[numUsedPixels] = new[] { r, g, b };
                        numUsedPixels++;
                    }
                }
            }

            if (ignoreWhite)
            {
                // Remove unused pixels from the array
                byte[][] copy = new byte[numUsedPixels][];
                Array.Copy(pixelArray, copy, numUsedPixels);
                return copy;
            }
            else
            {
                return pixelArray;
            }
        }
    }
}