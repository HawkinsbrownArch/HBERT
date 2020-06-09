using System;
using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.DB;
using CarbonEmissionTool.Model.Annotations;
using CarbonEmissionTool.Model.Utilities;
using CarbonEmissionTool.Services;
using CarbonEmissionTool.Services.Caches;

namespace CarbonEmissionTool.Model.Charts
{
    internal class TreeChart
    {
        internal static List<KeyValuePair<string, double>> NormalizeSizes(List<KeyValuePair<string, double>> eCData, double chartWidth, double chartHeight)
        {
            double total_size = eCData.Sum(o => o.Value);
            double total_area = chartWidth * chartHeight;

            List<KeyValuePair<string, double>> eCDataNormalized = new List<KeyValuePair<string, double>>();
            foreach (KeyValuePair<string, double> valuePair in eCData)
            {
                eCDataNormalized.Add(new KeyValuePair<string, double>(valuePair.Key, valuePair.Value * total_area / total_size));
            }
            return eCDataNormalized;
        }

        internal static void PadRectangle(Dictionary<string, object> rect)
        {
            double halfMM = 0.25 / 304.8;
            double fullMM = 0.50 / 304.8;
            rect["x"] = (double)rect["x"] + halfMM;
            rect["dx"] = (double)rect["dx"] - fullMM;
            rect["y"] = (double)rect["y"] + halfMM;
            rect["dy"] = (double)rect["dy"] - fullMM;
        }

        internal static List<Dictionary<string, object>> LayoutRow(List<KeyValuePair<string, double>> eCvaluesNormalized, double x, double y, double chartWidth, double chartHeight)
        {
            //# generate rects for each size in sizes
            //# dx >= dy
            //# they will fill up height dy, and width will be determined by their area
            //# sizes should be pre-normalized wrt dx * dy (i.e., they should be same units)
            double covered_area = eCvaluesNormalized.Sum(o => o.Value);
            double width = covered_area / chartHeight;
            List<Dictionary<string, object>> rects = new List<Dictionary<string, object>>();

            foreach (KeyValuePair<string, double> valuePair in eCvaluesNormalized)
            {
                double size = valuePair.Value;
                Dictionary<string, object> rect = new Dictionary<string, object>()
                {
                    { "x", x},
                    { "y", y},
                    { "dx", width},
                    { "dy", size / width},
                    {"material",  valuePair.Key }
                };

                rects.Add(rect);
                y += size / width;
            };

            return rects;
        }


        internal static List<Dictionary<string, object>> LayoutCol(List<KeyValuePair<string, double>> eCvaluesNormalized, double x, double y, double chartWidth, double chartHeight)
        {
            //# generate rects for each size in sizes
            //# dx < dy
            //# they will fill up width dx, and height will be determined by their area
            //# sizes should be pre-normalized wrt dx * dy (i.e., they should be same units)
            double covered_area = eCvaluesNormalized.Sum(o => o.Value);
            double height = covered_area / chartWidth;
            List<Dictionary<string, object>> rects = new List<Dictionary<string, object>>();
            foreach (KeyValuePair<string, double> valuePair in eCvaluesNormalized)
            {
                double size = valuePair.Value;
                Dictionary<string, object> rect = new Dictionary<string, object>()
                {
                    { "x", x},
                    { "y", y},
                    { "dx", size / height},
                    { "dy", height},
                    {"material",  valuePair.Key }
                };

                rects.Add(rect);

                x += size / height;
            }
            return rects;
        }

        internal static List<Dictionary<string, object>> Layout(List<KeyValuePair<string, double>> eCvaluesNormalized, double x, double y, double chartWidth, double chartHeight)
        {
            if (chartWidth >= chartHeight)
                return LayoutRow(eCvaluesNormalized, x, y, chartWidth, chartHeight);

            return LayoutCol(eCvaluesNormalized, x, y, chartWidth, chartHeight);
        }


        internal static List<double> LeftOverRow(List<KeyValuePair<string, double>> eCvaluesNormalized, double x, double y, double chartWidth, double chartHeight)
        {
            // compute remaining area when dx >= dy
            double covered_area = eCvaluesNormalized.Sum(o => o.Value);
            double width = covered_area / chartHeight;
            double leftover_x = x + width;
            double leftover_y = y;
            double leftover_chartWidth = chartWidth - width;
            double leftover_chartHeight = chartHeight;

            return new List<double> { leftover_x, leftover_y, leftover_chartWidth, leftover_chartHeight };
        }


        internal static List<double> LeftOverCol(List<KeyValuePair<string, double>> eCvaluesNormalized, double x, double y, double chartWidth, double chartHeight)
        {
            // compute remaining area when dx >= dy
            double covered_area = eCvaluesNormalized.Sum(o => o.Value);
            double height = covered_area / chartWidth;
            double leftover_x = x;
            double leftover_y = y + height;
            double leftover_chartWidth = chartWidth;
            double leftover_chartHeight = chartHeight - height;

            return new List<double> { leftover_x, leftover_y, leftover_chartWidth, leftover_chartHeight };
        }

        internal static List<double> LeftOver(List<KeyValuePair<string, double>> eCvaluesNormalized, double x, double y, double chartWidth, double chartHeight)
        {
            if (chartWidth >= chartHeight)
                return LeftOverRow(eCvaluesNormalized, x, y, chartWidth, chartHeight);

            return LeftOverCol(eCvaluesNormalized, x, y, chartWidth, chartHeight);
        }

        internal static double WorstRatio(List<KeyValuePair<string, double>> eCvaluesNormalized, double x, double y, double chartWidth, double chartHeight)
        {
            List<double> maxList = new List<double>();
            foreach(Dictionary<string, object> rect in Layout(eCvaluesNormalized, x, y, chartWidth, chartHeight))
            {
                maxList.Add(new List<double> { (double)rect["dx"] / (double)rect["dy"], (double)rect["dy"] / (double)rect["dx"]}.Max());
            }
            return maxList.Max();
        }

        private static List<Dictionary<string, object>> Squarify(List<KeyValuePair<string, double>> eCvaluesNormalized, double x, double y, double chartWidth, double chartHeight)
        {
            //# sizes should be pre-normalized wrt dx * dy (i.e., they should be same units)
            //# or dx * dy == sum(sizes)
            //# sizes should be sorted biggest to smallest
            int sizesCount = eCvaluesNormalized.Count;

            if (sizesCount == 0)
                return new List<Dictionary<string, object>>();


            if (sizesCount == 1)
                return Layout(eCvaluesNormalized, x, y, chartWidth, chartHeight);

            //#figure out where 'split' should be
            int i = 1;

            while (i < sizesCount & WorstRatio(eCvaluesNormalized.Take(i).ToList(), x, y, chartWidth, chartHeight) >= WorstRatio(eCvaluesNormalized.Take(i + 1).ToList(), x, y, chartWidth, chartHeight))
            {
                i += 1;
            }

            List<KeyValuePair<string, double>> current = eCvaluesNormalized.Take(i).ToList();
            List<KeyValuePair<string, double>> remaining = eCvaluesNormalized.Skip(i).ToList();

            List<double> leftOver = LeftOver(current, x, y, chartWidth, chartHeight);

            List<Dictionary<string, object>> layout = Layout(current, x, y, chartWidth, chartHeight);
            layout.AddRange(Squarify(remaining, leftOver[0], leftOver[1], leftOver[2], leftOver[3]));

            return layout;
        }

        internal static List<Dictionary<string, object>> PaddedSquarify(List<KeyValuePair<string, double>> eCvaluesNormalized, double x, double y, double chartWidth, double chartHeight)
        {
            List<Dictionary<string, object>> rects = Squarify(eCvaluesNormalized, x, y, chartWidth, chartHeight);
            foreach (Dictionary<string, object> rect in rects)
            {
                PadRectangle(rect);
            }
            return rects;
        }

        internal List<FilledRegion> GenerateTreeGraph(List<Dictionary<string, object>> paddedRects, Dictionary<string, FilledRegionType> filledRegionTypeDictionary, ElementId newTreeViewDrawingId, Annotation annotation)
        {
            var document = ApplicationServices.Document;

            var invisibleLinesId = ApplicationServices.InvisibleLinesId;

            var total = paddedRects.Sum(r => (double)r["dx"] * (double)r["dy"]);
            var smallCurveTolerance = ApplicationServices.ShortCurveTolerance;

            var rectanglist = new List<FilledRegion>();
            foreach (Dictionary<string, object> rectangle in paddedRects)
            {
                var width = (double)rectangle["dx"];
                var height = (double)rectangle["dy"];

                XYZ origin;

                var rectangleBoundaries = CurveLoopUtils.GenerateCurveLoop(rectangle, width, height, smallCurveTolerance, out origin);
                if(rectangleBoundaries != null)
                {
                    var typeId = FilledRegionCache.GetTypeId(document, rectangle["material"].ToString(), filledRegionTypeDictionary);

                    var filledRegion = FilledRegion.Create(document, typeId, newTreeViewDrawingId, rectangleBoundaries);

                    filledRegion.SetLineStyleId(invisibleLinesId);

                    rectanglist.Add(filledRegion);

                    annotation.OriginPoints.Add(origin);
                    annotation.TextPointSize.Add(FontSizeProcessor.FindBestPointSize(height, width));

                    var percentage = Math.Round(((width * height) / total) * 100, 1);
                    annotation.TextValues.Add($" {percentage}%");
                }
            }

            return rectanglist;
        }
    }
}
