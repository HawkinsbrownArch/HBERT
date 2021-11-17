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
            // var newRevitVersion = revitVersion > 2018;

            var colorInt = ColorUtils.ConvertRBGToInt(newColor.R, newColor.G, newColor.B);

#if REVIT2018
            filledRegionType.get_Parameter( BuiltInParameter.LINE_COLOR).Set(colorInt);

            filledRegionType.get_Parameter( BuiltInParameter.ANY_PATTERN_ID_PARAM_NO_NO).Set(fillPattern.Id);

#endif
#if (REVIT2019 || REVIT2020 || REVIT2021 || REVIT2022)


            // Available in Revit 2019 onwards
            filledRegionType.get_Parameter(BuiltInParameter.BACKGROUND_PATTERN_COLOR_PARAM ).Set(colorInt);

            filledRegionType.get_Parameter(BuiltInParameter.BACKGROUND_DRAFT_PATTERN_ID_PARAM).Set(fillPattern.Id);

            filledRegionType.get_Parameter(BuiltInParameter.FOREGROUND_ANY_PATTERN_ID_PARAM).Set(ElementId.InvalidElementId);


#endif

        }

    }
}
