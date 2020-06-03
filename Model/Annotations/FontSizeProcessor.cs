using CarbonEmissionTool.Model.Enums;

namespace CarbonEmissionTool.Model.Annotations
{
    class FontSizeProcessor
    {
        public static FontSize FindBestTextPointSize(double height, double width)
        {
            double heightInMM = height * convertToFt;
            double widthInMM = width * convertToFt;
            //Sizes shown in mm (by multiplying convertToFt)
            if (heightInMM < 4.0 | widthInMM < 4.0)
            {
                return FontSize.Six;
            }
            else if (heightInMM < 15.0 | widthInMM < 15.0)
            {
                return FontSize.Eleven;
            }
            else if (heightInMM < 40.0 | widthInMM < 40.0)
            {
                return FontSize.Sixteen;
            }
            else
            {
                return FontSize.Thirty;
            }
        }
    }
}