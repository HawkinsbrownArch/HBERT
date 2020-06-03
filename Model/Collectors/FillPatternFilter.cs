using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;

namespace CarbonEmissionTool.Model.Collectors
{
    class FillPatternFilter
    {
        internal static FillPatternElement GetSolidFillPattern(Document doc)
        {
            List<FillPatternElement> filledRegionType = new FilteredElementCollector(doc).OfClass(typeof(FillPatternElement)).Cast<FillPatternElement>().ToList();

            for (int i = 0; i < filledRegionType.Count; i++)
            {
                FillPatternElement currentFillPattern = filledRegionType[i];
                if (currentFillPattern.GetFillPattern().IsSolidFill)
                {
                    return currentFillPattern;
                }
            }
            return filledRegionType.First();
        }
    }
}
