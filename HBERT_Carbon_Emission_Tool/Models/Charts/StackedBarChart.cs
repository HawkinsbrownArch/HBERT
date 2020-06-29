using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.DB;
using CarbonEmissionTool.Models.Annotations;
using CarbonEmissionTool.Models.Headings;
using CarbonEmissionTool.Services;

namespace CarbonEmissionTool.Models.Charts
{
    public class StackedBarChart : IChartData
    {
        private const double _barHeight = 10.0;
        private const double _barLength = 130.0;

        private const double _viewportOriginXCoord = 340.0;
        private const double _viewporOriginYCoord = 214.5;

        public XYZ ViewportOrigin { get; }

        private TreeChartSquares _treeChartSquares;

        public HeadingCollection ChartHeadings { get; }

        public IHeading MainHeading { get; }

        public IHeading Subheading { get; }

        public IHeading AverageEmbodiedCarbonTitle { get; }

        public IHeading AverageEmbodiedCarbonValue { get; }

        public IHeading TotalEmbodiedCarbonTitle { get; }

        public IHeading TotalEmbodiedCarbonValue { get; }

        /// <summary>
        /// Constructs a new <see cref="StackedBarChart"/> and places the chart on the <paramref name="sheet"/> as filled regions. 
        /// </summary>
        public StackedBarChart(IProjectDetails projectDetails, CarbonDataCache carbonDataCache, FilledRegionCache filledRegionCache, ViewSheet sheet, TreeChart treeChart)
        {
            _treeChartSquares = treeChart.TreeChartSquares;

            var view = ViewDrafting.Create(ApplicationServices.Document, ApplicationServices.DraftingViewFamilyType.Id);
            view.Scale = 1;

            this.ViewportOrigin = new XYZ(_viewportOriginXCoord.ToDecimalFeet(), _viewporOriginYCoord.ToDecimalFeet(), 0.0);

            this.MainHeading = new BarChartHeadingHeading(sheet);

            this.Subheading = new BarChartSubheadingHeading(view, projectDetails.Name);

            this.AverageEmbodiedCarbonTitle = new AverageEmbodiedCarbonTitleHeading(view);
            this.AverageEmbodiedCarbonValue = new AverageEmbodiedCarbonValueHeading(view, projectDetails, carbonDataCache);

            this.TotalEmbodiedCarbonTitle = new TotalEmbodiedCarbonTitleHeading(view);
            this.TotalEmbodiedCarbonValue = new TotalEmbodiedCarbonValueHeading(view, carbonDataCache);

            this.ChartHeadings = new HeadingCollection();

            this.CreateFilledRegions(filledRegionCache, view);

            ViewportUtils.CreateChartViewport(sheet, view, this.ViewportOrigin);
        }

        /// <summary>
        /// Creates the filled regions of the bar chart in the <paramref name="view"/> and adds the
        /// heading labels to the <see cref="ChartHeadings"/>.
        /// </summary>
        public void CreateFilledRegions(FilledRegionCache filledRegionCache, View view)
        {
            var barLengths = new List<KeyValuePair<string, double>>();
            foreach (var rectangle in _treeChartSquares)
            {
                var xDimension = (double)rectangle["dx"];
                var yDimension = (double)rectangle["dy"];

                var area = xDimension * yDimension;

                //Add the bar length to the barLengths list
                KeyValuePair<string, double> keyValuePair = new KeyValuePair<string, double>(rectangle["material"].ToString(), area / _barHeight);

                barLengths.Add(keyValuePair);
            }

            var totalLengthOfBars = barLengths.Sum(b => b.Value);

            for (int i = 0; i < barLengths.Count; i++)
            {
                //Stack the bar segments
                var distanceAlong = (barLengths.Take(i).Sum(b => b.Value) / totalLengthOfBars) * _barLength;

                var ptOrigin = new XYZ(distanceAlong, 0.0, 0.0);
                var ptBottomLeft = new XYZ(ptOrigin.X, _barHeight, 0.0);

                var barLength = (barLengths[i].Value / totalLengthOfBars) * _barLength;
                var ptBottomRight = new XYZ(distanceAlong + barLength, _barHeight, 0.0);
                var ptTopRight = new XYZ(ptBottomRight.X, 0.0, 0.0);

                //Only add the bar segment if its greater than the short curve tolerance
                if (ptBottomLeft.DistanceTo(ptBottomRight) >= ApplicationServices.ShortCurveTolerance)
                {
                    var barCornerPoints = new List<XYZ> { ptOrigin, ptBottomLeft, ptBottomRight, ptTopRight };

                    FilledRegionUtils.FromPoints(filledRegionCache, barCornerPoints, view, barLengths[i].Key);

                    var origin = new XYZ((distanceAlong + (barLength / 2)) - 0.00360886, ptBottomLeft.Y + 0.00325, 0.0);
                    var barChartAnnotation = new BarChartHeading(view, origin, barLengths[i].Key);

                    //Add the material name to the heading object
                    ChartHeadings.Add(barChartAnnotation);
                }
            }
        }

        /// <summary>
        /// Returns a list of label headings in this chart.
        /// </summary>
        public List<IHeading> GetAllLabels()
        {
            var chartHeadingList = new List<IHeading>
            {
                this.MainHeading,
                this.Subheading,
                this.AverageEmbodiedCarbonTitle,
                this.AverageEmbodiedCarbonValue,
                this.TotalEmbodiedCarbonTitle,
                this.TotalEmbodiedCarbonValue
            };

            chartHeadingList.AddRange(this.ChartHeadings);

            return chartHeadingList;
        }
    }
}
