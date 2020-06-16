using CarbonEmissionTool.Model.Enums;
using CarbonEmissionTool.Model.Extensions;

namespace CarbonEmissionTool.Model.Annotations
{
    public class FontSizeProcessor
    {
        /// <summary>
        /// Returns a <see cref="FontSize"/> via a best match with the input <paramref name="height"/>
        /// and <paramref name="width"/>.
        /// </summary>
        /// <param name="height"> The height of the text in mm.</param>
        /// <param name="width"> The width of the text in mm.</param>
        public static FontSize FindBestPointSize(double height, double width)
        {
            double heightInMM = height.ToDecimalFeet();
            double widthInMM = width.ToDecimalFeet();

            //Sizes shown in mm (by multiplying convertToFt)
            if (heightInMM < 4.0 | widthInMM < 4.0)
            {
                return FontSize.Six;
            }
            if (heightInMM < 15.0 | widthInMM < 15.0)
            {
                return FontSize.Eleven;
            }
            if (heightInMM < 40.0 | widthInMM < 40.0)
            {
                return FontSize.Sixteen;
            }
            
            return FontSize.Thirty;
        }
    }
}