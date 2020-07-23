using Autodesk.Revit.DB;
using CarbonEmissionTool.Services;

namespace CarbonEmissionTool.Models
{
    /// <summary>
    /// Extension methods for <see cref="FilledRegionType"/>
    /// </summary>
    public static class FilledRegionTypeExtensions
    {


        /// <summary>
        /// Sets the color and fill pattern of the <see cref="FilledRegionType"/>.
        /// </summary>
        public static void SetColorAndPattern(this FilledRegionType filledRegionType, System.Windows.Media.Color newColor, FillPatternElement fillPattern)
        {
            var revitVersion = ApplicationServices.RevitVersionNumber;

            // 2019 and above.
            var newRevitVersion = revitVersion > 2018;

            var colorInt = ColorUtils.ConvertRBGToInt(newColor.R, newColor.G, newColor.B);
            filledRegionType.get_Parameter(newRevitVersion ? BuiltInParameter.BACKGROUND_PATTERN_COLOR_PARAM : BuiltInParameter.LINE_COLOR).Set(colorInt);

            filledRegionType.get_Parameter(newRevitVersion ? BuiltInParameter.BACKGROUND_DRAFT_PATTERN_ID_PARAM : BuiltInParameter.ANY_PATTERN_ID_PARAM_NO_NO).Set(fillPattern.Id);

            // Set the foreground pattern to null so it is invisible. Only applies to new versions of Revit.
            if (newRevitVersion)
            {
                filledRegionType.get_Parameter(BuiltInParameter.FOREGROUND_ANY_PATTERN_ID_PARAM).Set(ElementId.InvalidElementId);
            }
        }

    }
}
