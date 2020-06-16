﻿using System;
using System.Windows.Media;

namespace CarbonEmissionTool.Model.Graphics
{
    public class ColorUtils
    {
        /// <summary>
        /// Converts RBG values to a Revit color represented as an int.
        /// </summary>
        public static int ConvertRBGToInt(int r, int g, int b)
        {
            return r + g * (int)Math.Pow(2, 8) + b * (int)Math.Pow(2, 16);
        }

        /// <summary>
        /// Converts a color to a Revit color represented as an int.
        /// </summary>
        public static int ConvertColorToInt(Color color)
        {
            return color.R + color.G * (int)Math.Pow(2, 8) + color.B * (int)Math.Pow(2, 16);
        }
    }
}