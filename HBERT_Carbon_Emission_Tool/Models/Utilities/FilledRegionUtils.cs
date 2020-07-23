using System.Collections.Generic;
using Autodesk.Revit.DB;
using CarbonEmissionTool.Services;

namespace CarbonEmissionTool.Models
{
    public class FilledRegionUtils
    {
        /// <summary>
        /// Creates a <see cref="FilledRegion"/> region from a list of points.
        /// </summary>
        public static FilledRegion FromPoints(FilledRegionCache filledRegionCache, List<XYZ> cornerPoints, View view, string materialKey)
        {
            var document = ApplicationServices.Document;

            var curveLoop = new CurveLoop();
            for (int i = 0; i < cornerPoints.Count; i++)
            {
                var ptStart = cornerPoints[i];
                var ptEnd = cornerPoints[i + 1 == cornerPoints.Count ? 0 : i + 1];

                var lnEdge = Line.CreateBound(ptStart, ptEnd);

                curveLoop.Append(lnEdge);
            }

            var filledRegionType = filledRegionCache.GetByName(materialKey);

            var filledRegion = FilledRegion.Create(document, filledRegionType.Id, view.Id, new List<CurveLoop> { curveLoop });
            filledRegion.SetLineStyleId(ApplicationServices.InvisibleLinesId);

            return filledRegion;
        }
    }
}
