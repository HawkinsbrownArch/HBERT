using Autodesk.Revit.DB;
using CarbonEmissionTool.Services;
using CarbonEmissionTool.Services.Caches;
using System.Collections.Generic;

namespace CarbonEmissionTool.Model.Utilities
{
    class FilledRegionUtils
    {
        public static FilledRegion FromPoints(List<XYZ> cornerPoints, Dictionary<string, FilledRegionType> filledRegionTypeDictionary, ElementId newDrawingId, string materialKey)
        {
            var document = ApplicationServices.Document;

            CurveLoop curveLoop = new CurveLoop();
            for (int i = 0; i < cornerPoints.Count; i++)
            {
                XYZ ptStart = cornerPoints[i];
                XYZ ptEnd = cornerPoints[i + 1 == cornerPoints.Count ? 0 : i + 1];
                
                Line lnEdge = Line.CreateBound(ptStart, ptEnd);

                curveLoop.Append(lnEdge);
            }

            ElementId typeId = FilledRegionCache.GetTypeId(document, materialKey, filledRegionTypeDictionary);

            FilledRegion filledRegion = FilledRegion.Create(document, typeId, newDrawingId, new List<CurveLoop> { curveLoop } );
            filledRegion.SetLineStyleId(ApplicationServices.InvisibleLinesId);

            return filledRegion;
        }
    }
}
