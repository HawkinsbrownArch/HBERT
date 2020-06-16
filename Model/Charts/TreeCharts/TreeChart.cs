using Autodesk.Revit.DB;
using CarbonEmissionTool.Model.Annotations;
using CarbonEmissionTool.Model.Extensions;
using CarbonEmissionTool.Model.Interfaces;
using CarbonEmissionTool.Model.Utilities;
using CarbonEmissionTool.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using CarbonEmissionTool.Model.Charts.TreeCharts.Headings;

namespace CarbonEmissionTool.Model.Charts.TreeCharts
{
    /// <summary>
    /// Creates a tree chart on the EC sheet using Revit filled regions.
    /// </summary>
    public class TreeChart : IChartData
    {
        private const double _viewportOriginXCoord = 328.0;
        private const double _viewporOriginYCoord = 96.0;

        public XYZ ViewportOrigin { get; }

        public AnnotationCollection ChartAnnotations { get; }

        public TreeChartSquares TreeChartSquares { get; }

        public IAnnotation MainHeading { get; }

        /// <summary>
        /// Constructs a new <see cref="TreeChart"/> and places the chart on the <paramref name="sheet"/> as filled regions. 
        /// </summary>
        public TreeChart(IProjectDetails projectDetails, ViewSheet sheet)
        {
            Autodesk.Revit.DB.View view = ViewDrafting.Create(ApplicationServices.Document, ApplicationServices.DraftingViewFamilyType.Id);
            view.Scale = 1;

            this.ViewportOrigin = new XYZ(_viewportOriginXCoord.ToDecimalFeet(), _viewporOriginYCoord.ToDecimalFeet(), 0.0);

            this.TreeChartSquares = new TreeChartSquares(projectDetails.CarbonDataCache);

            this.MainHeading = new TreeChartHeadingAnnotation(sheet);

            this.ChartAnnotations = new AnnotationCollection();

            this.CreateFilledRegions(projectDetails, view);

            ViewportUtils.CreateChartViewport(sheet, view, this.ViewportOrigin);
        }

        /// <summary>
        /// Creates the filled regions in Revit on the <paramref name="view"/> and adds the annotation
        /// text notes as each one is created.
        /// </summary>
        public void CreateFilledRegions(IProjectDetails projectDetails, Autodesk.Revit.DB.View view)
        {
            var document = ApplicationServices.Document;
            var viewId = view.Id;

            var invisibleLinesId = ApplicationServices.InvisibleLinesId;

            var total = this.TreeChartSquares.Sum(r => (double)r["dx"] * (double)r["dy"]);
            var smallCurveTolerance = ApplicationServices.ShortCurveTolerance;

            foreach (Dictionary<string, object> rectangle in this.TreeChartSquares)
            {
                var width = (double)rectangle["dx"];
                var height = (double)rectangle["dy"];

                XYZ origin;

                var rectangleBoundaries = CurveLoopUtils.GenerateCurveLoop(rectangle, width, height, smallCurveTolerance, out origin);
                if(rectangleBoundaries != null)
                {
                    var filledRegionName = rectangle["material"].ToString();

                    var filledRegionType = projectDetails.FilledRegionCache.GetByName(filledRegionName);

                    var filledRegion = FilledRegion.Create(document, filledRegionType.Id, viewId, rectangleBoundaries);

                    filledRegion.SetLineStyleId(invisibleLinesId);

                    var percentage = Math.Round(((width * height) / total) * 100, 1);

                    var chartAnnotation = new TreeChartAnnotation(view, origin, $" {percentage}%", height, width);

                    this.ChartAnnotations.Add(chartAnnotation);
                }
            }
        }

        /// <summary>
        /// Returns a list of label headings in this chart.
        /// </summary>
        public List<IAnnotation> GetAllLabels()
        {
            var chartHeadingList = new List<IAnnotation>();
            chartHeadingList.Add(this.MainHeading);
            chartHeadingList.AddRange(this.ChartAnnotations);

            return chartHeadingList;
        }
    }
}
