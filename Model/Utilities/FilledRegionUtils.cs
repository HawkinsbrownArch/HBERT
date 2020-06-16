using Autodesk.Revit.DB;
using CarbonEmissionTool.Model.Interfaces;
using CarbonEmissionTool.Services;
using System.Collections.Generic;

namespace CarbonEmissionTool.Model.Utilities
{
    public class FilledRegionUtils
    {
        /// <summary>
        /// Creates a <see cref="FilledRegion"/> region from a list of points.
        /// </summary>
        public static FilledRegion FromPoints(IProjectDetails projectDetails, List<XYZ> cornerPoints, Autodesk.Revit.DB.View view, string materialKey)
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

            var filledRegionType = projectDetails.FilledRegionCache.GetByName(materialKey);

            var filledRegion = FilledRegion.Create(document, filledRegionType.Id, view.Id, new List<CurveLoop> { curveLoop } );
            filledRegion.SetLineStyleId(ApplicationServices.InvisibleLinesId);

            return filledRegion;
        }
    }
}
