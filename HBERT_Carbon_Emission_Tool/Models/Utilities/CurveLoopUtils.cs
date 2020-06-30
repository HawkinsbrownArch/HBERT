using System.Collections.Generic;
using Autodesk.Revit.DB;

namespace CarbonEmissionTool.Models
{
    public class CurveLoopUtils
    {
        /// <summary>
        /// Creates a list of <see cref="CurveLoop"/>'s from a list of rectangles.
        /// </summary>
        public static List<CurveLoop> GenerateCurveLoop(Dictionary<string, object> rectangle, double width, double height, double smallCurveTolerance, out XYZ ptTopLeft)
        {
            XYZ origin = new XYZ((double)rectangle["x"], (double)rectangle["y"], 0.0);

            Transform transformX = Transform.CreateTranslation(new XYZ(width, 0.0, 0.0));
            Transform transformY = Transform.CreateTranslation(new XYZ(0.0, height, 0.0));

            XYZ ptBottomRight = transformX.OfPoint(origin);
            XYZ ptTopRight = transformY.OfPoint(ptBottomRight);
            ptTopLeft = new XYZ(origin.X, ptTopRight.Y, 0.0);

            List<XYZ> cornerPoints = new List<XYZ> { origin, ptBottomRight, ptTopRight, ptTopLeft };

            if (origin.DistanceTo(ptTopLeft) < smallCurveTolerance | origin.DistanceTo(ptBottomRight) < smallCurveTolerance)
                return null;

            CurveLoop curveLoop = new CurveLoop();
            for (int i = 0; i < cornerPoints.Count; i++)
            {
                XYZ ptStart = cornerPoints[i];
                XYZ ptEnd = cornerPoints[i + 1 == cornerPoints.Count ? 0 : i + 1];

                Line lnEdge = Line.CreateBound(ptStart, ptEnd);

                curveLoop.Append(lnEdge);
            }

            return new List<CurveLoop> { curveLoop };
        }
    }
}