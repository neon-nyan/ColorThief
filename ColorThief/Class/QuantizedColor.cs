using System;

namespace ColorThiefDotNet
{
    public class QuantizedColor
    {
        public QuantizedColor(CTColor color, int population)
        {
            Color = color;
            Population = population;
            IsDark = CalculateYiqLuma(color) < 128;
        }

        public CTColor Color { get; private set; }
        public int Population { get; private set; }
        public bool IsDark { get; private set; }

        public int CalculateYiqLuma(CTColor color) => (int)Math.Round((299 * color.R + 587 * color.G + 114 * color.B) / 1000f);
    }
}