using System.Collections.Generic;

namespace ColorThiefDotNet
{
    /// <summary>
    ///     Color map
    /// </summary>
#if NETCOREAPP && NET7_0_OR_GREATER
    internal struct CMap
#else
    internal class CMap
#endif
    {
        private readonly List<VBox> vboxes = new List<VBox>();
        private List<QuantizedColor> palette;

        public CMap() { }

        public void Push(VBox box)
        {
            palette = null;
            vboxes.Add(box);
        }

        public IEnumerable<QuantizedColor> GeneratePalette()
        {
            foreach (VBox vbox in vboxes)
            {
                int[] rgb = vbox.Avg(false);
                CTColor color = FromRgb(rgb[0], rgb[1], rgb[2]);
                yield return new QuantizedColor(color, vbox.Count(false));
            }
        }

        public List<QuantizedColor> GeneratePaletteList()
        {
            if (palette == null)
            {
                palette = new List<QuantizedColor>();
                foreach (VBox vbox in vboxes)
                {
                    int[] rgb = vbox.Avg(false);
                    CTColor color = FromRgb(rgb[0], rgb[1], rgb[2]);
                    palette.Add(new QuantizedColor(color, vbox.Count(false)));
                }
            }

            return palette;
        }

        public CTColor FromRgb(int red, int green, int blue) => new CTColor { R = (byte)red, G = (byte)green, B = (byte)blue };
    }
}