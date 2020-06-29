using System.Linq;
using Autodesk.Revit.DB;
using CarbonEmissionTool.Services;

namespace CarbonEmissionTool.Models
{
    public class FillPatternFilter
    {
        /// <summary>
        /// Returns the solid fill pattern from the active Revit document.
        /// </summary>
        public static FillPatternElement GetSolidFillPattern()
        {
            var filledRegionTypes =
                new FilteredElementCollector(ApplicationServices.Document).OfClass(typeof(FillPatternElement));

            foreach (FillPatternElement filledRegionType in filledRegionTypes)
            {
                if (filledRegionType.GetFillPattern().IsSolidFill)
                    return filledRegionType;
            }

            return (FillPatternElement)filledRegionTypes.First();
        }
    }
}
