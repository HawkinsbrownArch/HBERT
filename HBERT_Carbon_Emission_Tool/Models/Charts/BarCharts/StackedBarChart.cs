using Autodesk.Revit.DB;
using CarbonEmissionTool.Model.Annotations;
using CarbonEmissionTool.Model.Charts.BarCharts.HeadingPairs;
using CarbonEmissionTool.Model.Charts.BarCharts.Headings;
using CarbonEmissionTool.Model.Charts.TreeCharts;
using CarbonEmissionTool.Model.Extensions;
using CarbonEmissionTool.Model.Interfaces;
using CarbonEmissionTool.Model.Utilities;
using CarbonEmissionTool.Services;
using CarbonEmissionTool.Services.Caches;
using System.Collections.Generic;
using System.Linq;

namespace CarbonEmissionTool.Model.Charts.BarCharts
{
    public class StackedBarChart : IChartData
    {
        private const double _barHeight = 10.0;
        private const double _barLength = 130.0;

        private const double _viewportOriginXCoord = 340.0;
        private const double _viewporOriginYCoord = 214.5;

        public XYZ ViewportOrigin { get; }

        private TreeChartSquares _treeChartSquares;

        public AnnotationCollection ChartAnnotations { get; }

        public IAnnotation MainHeading { get; }

        public IAnnotation Subheading { get; }

        public IAnnotation AverageEmbodiedCarbonTitle { get; }

        public IAnnotation AverageEmbodiedCarbonValue { get; }

        public IAnnotation TotalEmbodiedCarbonTitle { get; }

        public IAnnotation TotalEmbodiedCarbonValue { get; }

        /// <summary>
        /// Constructs a new <see cref="StackedBarChart"/> and places the chart on the <paramref name="sheet"/> as filled regions. 
        /// </summary>
        public StackedBarChart(IProjectDetails projectDetails, ViewSheet sheet, TreeChart treeChart)
        {
            _treeChartSquares = treeChart.TreeChartSquares;

            var view = ViewDrafting.Create(ApplicationServices.Document, ApplicationServices.DraftingViewFamilyType.Id);
            view.Scale = 1;
            
            this.ViewportOrigin = new XYZ(_viewportOriginXCoord.ToDecimalFeet(), _viewporOriginYCoord.ToDecimalFeet(), 0.0);

            this.MainHeading = new BarChartHeadingAnnotation(sheet);

            this.Subheading = new BarChartSubheadingAnnotation(view, projectDetails.Name);

            this.AverageEmbodiedCarbonTitle = new AverageEmbodiedCarbonTitleAnnotation(view);
            this.AverageEmbodiedCarbonValue = new AverageEmbodiedCarbonValueAnnotation(view, projectDetails, projectDetails.CarbonDataCache);

            this.TotalEmbodiedCarbonTitle = new TotalEmbodiedCarbonTitleAnnotation(view);
            this.TotalEmbodiedCarbonValue = new TotalEmbodiedCarbonValueAnnotation(view, projectDetails.CarbonDataCache);

            this.ChartAnnotations = new AnnotationCollection();

            this.CreateFilledRegions(projectDetails, view);

            ViewportUtils.CreateChartViewport(sheet, view, this.ViewportOrigin);
        }

        /// <summary>
        /// Creates the filled regions of the bar chart in the <paramref name="view"/> and adds the
        /// annotation labels to the <see cref="ChartAnnotations"/>.
        /// </summary>
        public void CreateFilledRegions(IProjectDetails projectDetails, Autodesk.Revit.DB.View view)
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

            for(int i = 0; i < barLengths.Count; i++)
            { 
                //Stack the bar segments
                var distanceAlong = (barLengths.Take(i).Sum(b => b.Value) / totalLengthOfBars) * _barLength;

                var ptOrigin = new XYZ(distanceAlong, 0.0, 0.0);
                var ptBottomLeft = new XYZ(ptOrigin.X, _barHeight, 0.0);

                var barLength = (barLengths[i].Value / totalLengthOfBars) * _barLength;
                var ptBottomRight = new XYZ(distanceAlong + barLength, _barHeight, 0.0);
                var ptTopRight = new XYZ(ptBottomRight.X, 0.0, 0.0);
                
                //Only add the bar segment if its greater than the short curve tolerance
                if(ptBottomLeft.DistanceTo(ptBottomRight) >= ApplicationServices.ShortCurveTolerance)
                {
                    var barCornerPoints = new List<XYZ> { ptOrigin, ptBottomLeft, ptBottomRight, ptTopRight };

                    FilledRegionUtils.FromPoints(projectDetails, barCornerPoints, view, barLengths[i].Key);

                    var origin = new XYZ((distanceAlong + (barLength / 2)) - 0.00360886, ptBottomLeft.Y + 0.00325, 0.0);
                    var barChartAnnotation = new BarChartAnnotation(view, origin, barLengths[i].Key);

                    //Add the material name to the annotation object
                    ChartAnnotations.Add(barChartAnnotation);
                }
            }
        }

        /// <summary>
        /// Returns a list of label headings in this chart.
        /// </summary>
        public List<IAnnotation> GetAllLabels()
        {
            var chartHeadingList = new List<IAnnotation>
            {
                this.MainHeading,
                this.Subheading,
                this.AverageEmbodiedCarbonTitle,
                this.AverageEmbodiedCarbonValue,
                this.TotalEmbodiedCarbonTitle,
                this.TotalEmbodiedCarbonValue
            };

            chartHeadingList.AddRange(this.ChartAnnotations);

            return chartHeadingList;
        }
    }
}
