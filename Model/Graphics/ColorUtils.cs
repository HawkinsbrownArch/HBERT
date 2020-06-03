using System;

namespace CarbonEmissionTool.Model.Graphics
{
    class ColorUtils
    {
        internal static int ConvertColourToInt(int r, int g, int b)
        {
            return r + g * (int)Math.Pow(2, 8) + b * (int)Math.Pow(2, 16);
        }
    }
}