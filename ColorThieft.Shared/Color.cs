using System;

namespace ColorThiefDotNet
{
    /// <summary>
    ///     Defines a color in RGB space.
    /// </summary>
    public struct CTColor
    {
        /// <summary>
        ///     Get or Set the Blue component value for sRGB.
        /// </summary>
        public byte B;

        /// <summary>
        ///     Get or Set the Green component value for sRGB.
        /// </summary>
        public byte G;

        /// <summary>
        ///     Get or Set the Red component value for sRGB.
        /// </summary>
        public byte R;

        /// <summary>
        ///     Get HSL color.
        /// </summary>
        /// <returns></returns>
        public CTHslColor ToHsl()
        {
            const double toDouble = 1.0 / 255;
            var r = toDouble * R;
            var g = toDouble * G;
            var b = toDouble * B;
            var max = Math.Max(Math.Max(r, g), b);
            var min = Math.Min(Math.Min(r, g), b);
            var chroma = max - min;
            double h1;

            // ReSharper disable CompareOfFloatsByEqualityOperator
            if(chroma == 0)
            {
                h1 = 0;
            }
            else if(max == r)
            {
                h1 = (g - b) / chroma % 6;
            }
            else if(max == g)
            {
                h1 = 2 + (b - r) / chroma;
            }
            else //if (max == b)
            {
                h1 = 4 + (r - g)/chroma;
            }

            var lightness = 0.5 * (max - min);
            var saturation = chroma == 0 ? 0 : chroma / (1 - Math.Abs(2*lightness - 1));
            CTHslColor ret;
            ret.H = 60 * h1;
            ret.S = saturation;
            ret.L = lightness;
            return ret;
            // ReSharper restore CompareOfFloatsByEqualityOperator
        }

        public string ToHexString() => "#" + R.ToString("X2") + G.ToString("X2") + B.ToString("X2");

        public override string ToString() => ToHexString();
    }
}