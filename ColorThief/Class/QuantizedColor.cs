using System;
using System.Drawing;

namespace ColorThiefDotNet
{
#if NETCOREAPP && NET7_0_OR_GREATER
    public struct QuantizedColor
#else
    public class QuantizedColor
#endif
    {
        public QuantizedColor(Color color, int population)
        {
            Color = color;
            Population = population;
            IsDark = CalculateYiqLuma(color) < 128;
        }

        public Color Color { get; private set; }
        public int Population { get; private set; }
        public bool IsDark { get; private set; }

        public int CalculateYiqLuma(Color color) => (int)Math.Round((299 * color.R + 587 * color.G + 114 * color.B) / 1000f);
    }
}