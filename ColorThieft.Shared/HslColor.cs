namespace ColorThiefDotNet
{
    /// <summary>
    ///     Defines a color in Hue/Saturation/Lightness (HSL) space.
    /// </summary>
    public struct CTHslColor
    {
        /// <summary>
        ///     The Hue in 0..360 range.
        /// </summary>
        public double H;

        /// <summary>
        ///     The Lightness in 0..1 range.
        /// </summary>
        public double L;

        /// <summary>
        ///     The Saturation in 0..1 range.
        /// </summary>
        public double S;
    }
}