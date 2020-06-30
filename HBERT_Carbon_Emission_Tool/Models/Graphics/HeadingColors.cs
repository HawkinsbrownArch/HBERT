using System.Windows.Media;

namespace CarbonEmissionTool.Models
{
    /// <summary>
    /// A collection of <see cref="ColorData"/> objects used for headings.
    /// </summary>
    static class HeadingColors
    {
        public static ColorData Black = new ColorData
        {
            Name = "Black",
            Color = new Color
            {
                R = 0,
                G = 0,
                B = 0,
                A = 255
            }
        };

        public static ColorData White = new ColorData
        {
            Name = "White",
            Color = new Color
            {
                R = 254,
                G = 254,
                B = 254,
                A = 255
            }
        };

        public static ColorData Red = new ColorData
        {
            Name = "Red",
            Color = new Color
            {
                R = 232,
                G = 70,
                B = 16,
                A = 255
            }
        };

        public static ColorData LightGrey = new ColorData
        {
            Name = "LightGrey",
            Color = new Color
            {
                R = 128,
                G = 128,
                B = 128,
                A = 255
            }
        };
    }
}
