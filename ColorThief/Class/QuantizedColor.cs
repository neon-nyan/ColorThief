using System;
using System.Drawing;

namespace ColorThiefDotNet
{
    public struct QuantizedColor
    {
        public QuantizedColor(Color color, int population)
        {
            Color = color;
            Population = population;
            IsDark = LumaUtils.CalculateYiqLuma(color.R, color.G, color.B) < LumaUtils.DarkThreshold;
        }

        public Color Color { get; private set; }
        public int Population { get; private set; }
        public bool IsDark { get; private set; }
    }

    public static class LumaUtils
    {
        // This Coefficient is based on sRGB (ITU BT.709)
        public static double RCoe = 0.2126;
        public static double GCoe = 0.7152;
        public static double BCoe = 0.0722;

        public static double DarkThreshold = 428f;
        public static double IgnoreWhiteThreshold = 750f;

        // This new algorithm returns at range 0 - 1000
        public static double CalculateYiqLuma(byte R, byte G, byte B)
        {
            double vR = R / 255f;
            double vG = G / 255f;
            double vB = B / 255f;

            double linR = sRGBtoLin(vR);
            double linG = sRGBtoLin(vG);
            double linB = sRGBtoLin(vB);

            double y = (RCoe * linR) + (GCoe * linG) + (BCoe * linB);

            return Math.Round(y * 1000f, 2);
        }

        private static double sRGBtoLin(double colorChannel)
        {
            if (colorChannel <= 0.04045)
                return colorChannel / 12.92;
            else
                return Math.Pow((colorChannel + 0.055) / 1.055, 2.4);
        }

        public static void ChangeCoeToBT709()
        {
            RCoe = 0.2126;
            GCoe = 0.7152;
            BCoe = 0.0722;
        }

        public static void ChangeCoeToBT601()
        {
            RCoe = 0.299;
            GCoe = 0.587;
            BCoe = 0.114;
        }
    }
}