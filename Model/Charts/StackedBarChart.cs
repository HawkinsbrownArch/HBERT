using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.DB;
using CarbonEmissionTool.Model.Annotations;
using CarbonEmissionTool.Model.Utilities;

namespace CarbonEmissionTool.Model.Charts
{
    class StackedBarChart
    {
        internal List<FilledRegion> GenerateStackedBars(Document doc, List<Dictionary<string, object>> paddedRects, Dictionary<string, FilledRegionType> filledRegionTypeDictionary, ElementId newDrawingId, Annotation annotation, double barHeight, double barGraphLength, double originX, double originY, double gapInFt)
        {
            double shortCurveTolerance = doc.Application.ShortCurveTolerance;

            ElementId invisibleLinesId = FilledRegionUtils.GetInvisibleLineStyleId(doc);

            double totalArea = paddedRects.Sum(d => (double)d["dx"] * (double)d["dy"]);

            List<KeyValuePair<string, double>> barLengths = new List<KeyValuePair<string, double>>();
            foreach (Dictionary<string, object> rectangle in paddedRects)
            {
                double xDimension = (double)rectangle["dx"];
                double yDimension = (double)rectangle["dy"];

                double area = xDimension * yDimension;

                //Add the bar length to the barLengths list
                KeyValuePair<string, double> keyValuePair = new KeyValuePair<string, double>(rectangle["material"].ToString(), area / barHeight);

                barLengths.Add(keyValuePair);
            }

            double totalLengthOfBars = barLengths.Sum(b => b.Value);

            XYZ chartOrigin = new XYZ(originX, originY, 0.0);

            List<FilledRegion> barFilledRegions = new List<FilledRegion>();
            for(int i = 0; i < barLengths.Count; i++)
            {
                double distanceAlong = (barLengths.Take(i).Sum(b => b.Value) / totalLengthOfBars) * barGraphLength; //Stack the bar segments

                XYZ ptOrigin = new XYZ(distanceAlong, 0.0, 0.0);
                XYZ ptBottomLeft = new XYZ(ptOrigin.X, barHeight, 0.0);

                double barLength = (barLengths[i].Value / totalLengthOfBars) * barGraphLength;
                XYZ ptBottomRight = new XYZ(distanceAlong + barLength, barHeight, 0.0);
                XYZ ptTopRight = new XYZ(ptBottomRight.X, 0.0, 0.0);

                //Only add the bar segment if its greater than the short curve tolerance
                if(ptBottomLeft.DistanceTo(ptBottomRight) >= doc.Application.ShortCurveTolerance)
                {
                    List<XYZ> barCornerPoints = new List<XYZ>() { ptOrigin, ptBottomLeft, ptBottomRight, ptTopRight };

                    FilledRegion filledRegion = new FilledRegionUtils().FromPoints(doc, barCornerPoints, chartOrigin, filledRegionTypeDictionary, newDrawingId, invisibleLinesId, barLengths[i].Key);

                    barFilledRegions.Add(filledRegion);

                    //Add the material name to the annotation object
                    annotation.TextValues.Add(barLengths[i].Key);
                    annotation.TextPointSize.Add(Annotation.FontSize.Six);
                    annotation.OriginPoints.Add(new XYZ((distanceAlong + (barLength / 2)) - 0.00360886, ptBottomLeft.Y + 0.00325, 0.0));
                }
            }

            return barFilledRegions;
        }
    }
}
