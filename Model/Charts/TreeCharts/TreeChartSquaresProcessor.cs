using CarbonEmissionTool.Model.Extensions;
using CarbonEmissionTool.Services;
using CarbonEmissionTool.Services.Caches;
using System.Collections.Generic;
using System.Linq;
using CarbonEmissionTool.Settings;

namespace CarbonEmissionTool.Model.Charts.TreeCharts
{
    class TreeChartSquaresProcessor
    {
        public static List<KeyValuePair<string, double>> NormalizeSizes(CarbonDataCache carbonDataCache, double chartWidth, double chartHeight)
        {
            double total_size = carbonDataCache.Sum(o => o.EmbodiedCarbon);
            double total_area = chartWidth * chartHeight;

            List<KeyValuePair<string, double>> eCDataNormalized = new List<KeyValuePair<string, double>>();
            foreach (var carbonData in carbonDataCache)
            {
                eCDataNormalized.Add(new KeyValuePair<string, double>(carbonData.MaterialName, carbonData.EmbodiedCarbon * total_area / total_size));
            }
            return eCDataNormalized;
        }

        /// <summary>
        /// Creates padded squares using the <see cref="ApplicationSettings.TreeGraphPadding"/> as the spacing value.
        /// </summary>
        private static void PadSquares(Dictionary<string, object> rect)
        {
            double spacing = ApplicationSettings.TreeGraphPadding.ToDecimalFeet();
            double halfSpacing = spacing / 2.0;
            
            rect["x"] = (double)rect["x"] + halfSpacing;
            rect["dx"] = (double)rect["dx"] - spacing;
            rect["y"] = (double)rect["y"] + halfSpacing;
            rect["dy"] = (double)rect["dy"] - spacing;
        }

        private static List<Dictionary<string, object>> LayoutRow(List<KeyValuePair<string, double>> eCvaluesNormalized, double x, double y, double chartWidth, double chartHeight)
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

        private static List<Dictionary<string, object>> LayoutCol(List<KeyValuePair<string, double>> eCvaluesNormalized, double x, double y, double chartWidth, double chartHeight)
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

        private static List<Dictionary<string, object>> Layout(List<KeyValuePair<string, double>> eCvaluesNormalized, double x, double y, double chartWidth, double chartHeight)
        {
            if (chartWidth >= chartHeight)
                return LayoutRow(eCvaluesNormalized, x, y, chartWidth, chartHeight);

            return LayoutCol(eCvaluesNormalized, x, y, chartWidth, chartHeight);
        }

        private static List<double> LeftOverRow(List<KeyValuePair<string, double>> eCvaluesNormalized, double x, double y, double chartWidth, double chartHeight)
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

        private static List<double> LeftOverCol(List<KeyValuePair<string, double>> eCvaluesNormalized, double x, double y, double chartWidth, double chartHeight)
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

        private static List<double> LeftOver(List<KeyValuePair<string, double>> eCvaluesNormalized, double x, double y, double chartWidth, double chartHeight)
        {
            if (chartWidth >= chartHeight)
                return LeftOverRow(eCvaluesNormalized, x, y, chartWidth, chartHeight);

            return LeftOverCol(eCvaluesNormalized, x, y, chartWidth, chartHeight);
        }

        private static double WorstRatio(List<KeyValuePair<string, double>> eCvaluesNormalized, double x, double y, double chartWidth, double chartHeight)
        {
            List<double> maxList = new List<double>();
            foreach (Dictionary<string, object> rect in Layout(eCvaluesNormalized, x, y, chartWidth, chartHeight))
            {
                maxList.Add(new List<double> { (double)rect["dx"] / (double)rect["dy"], (double)rect["dy"] / (double)rect["dx"] }.Max());
            }
            return maxList.Max();
        }

        /// <summary>
        /// Creates unpadded tree graph squares.
        /// </summary>
        public static List<Dictionary<string, object>> Unpadded(List<KeyValuePair<string, double>> eCvaluesNormalized, double x, double y, double chartWidth, double chartHeight)
        {
            //# sizes should be pre-normalized wrt dx * dy (i.e., they should be same units)
            //# or dx * dy == sum(sizes)
            //# sizes should be sorted biggest to smallest
            int sizesCount = eCvaluesNormalized.Count;

            if (sizesCount == 0)
                return new List<Dictionary<string, object>>();


            if (sizesCount == 1)
                return TreeChartSquaresProcessor.Layout(eCvaluesNormalized, x, y, chartWidth, chartHeight);

            //#figure out where 'split' should be
            int i = 1;

            while (i < sizesCount & TreeChartSquaresProcessor.WorstRatio(eCvaluesNormalized.Take(i).ToList(), x, y, chartWidth, chartHeight) >= TreeChartSquaresProcessor.WorstRatio(eCvaluesNormalized.Take(i + 1).ToList(), x, y, chartWidth, chartHeight))
            {
                i += 1;
            }

            List<KeyValuePair<string, double>> current = eCvaluesNormalized.Take(i).ToList();
            List<KeyValuePair<string, double>> remaining = eCvaluesNormalized.Skip(i).ToList();

            List<double> leftOver = TreeChartSquaresProcessor.LeftOver(current, x, y, chartWidth, chartHeight);

            List<Dictionary<string, object>> layout = TreeChartSquaresProcessor.Layout(current, x, y, chartWidth, chartHeight);
            layout.AddRange(Unpadded(remaining, leftOver[0], leftOver[1], leftOver[2], leftOver[3]));

            return layout;
        }

        /// <summary>
        /// Creates padded tree graph squares.
        /// </summary>
        public static List<Dictionary<string, object>> Padded(List<KeyValuePair<string, double>> eCvaluesNormalized, double x, double y, double chartWidth, double chartHeight)
        {
            List<Dictionary<string, object>> rects = Unpadded(eCvaluesNormalized, x, y, chartWidth, chartHeight);
            foreach (Dictionary<string, object> rect in rects)
            {
                TreeChartSquaresProcessor.PadSquares(rect);
            }
            return rects;
        }
    }
}