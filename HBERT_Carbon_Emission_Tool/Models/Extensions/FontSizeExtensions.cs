namespace CarbonEmissionTool.Models
{
    public static class FontSizeExtensions
    {
        /// <summary>
        /// Converts a <see cref="FontSize"/> into mm.
        /// </summary>
        public static double ToMillimeters(this FontSize fontSize)
        {
            var pointSize = (double)fontSize;

            return pointSize * 0.3528888888888888;
        }

        /// <summary>
        /// Converts a <see cref="FontSize"/> into decimal feet.
        /// </summary>
        public static double ToDecimalFeet(this FontSize fontSize)
        {
            return fontSize.ToMillimeters().ToDecimalFeet();
        }
    }
}
