using System;
using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.DB;
using CarbonEmissionTool.Models.Annotations;
using CarbonEmissionTool.Models.Headings;
using CarbonEmissionTool.Services;

namespace CarbonEmissionTool.Models.Charts
{
    /// <summary>
    /// Creates a tree chart on the EC sheet using Revit filled regions.
    /// </summary>
    public class TreeChart : IChartData
    {
        private const double _viewportOriginXCoord = 328.0;
        private const double _viewporOriginYCoord = 96.0;

        public XYZ ViewportOrigin { get; }

        public HeadingCollection ChartHeadings { get; }

        public TreeChartSquares TreeChartSquares { get; }

        public IHeading MainHeading { get; }

        /// <summary>
        /// Constructs a new <see cref="TreeChart"/> and places the chart on the <paramref name="sheet"/> as filled regions. 
        /// </summary>
        public TreeChart(CarbonDataCache carbonDataCache, FilledRegionCache filledRegionCache, ViewSheet sheet)
        {
            View view = ViewDrafting.Create(ApplicationServices.Document, ApplicationServices.DraftingViewFamilyType.Id);
            view.Scale = 1;

            this.ViewportOrigin = new XYZ(_viewportOriginXCoord.ToDecimalFeet(), _viewporOriginYCoord.ToDecimalFeet(), 0.0);

            this.TreeChartSquares = new TreeChartSquares(carbonDataCache);

            this.MainHeading = new TreeChartHeadingHeading(sheet);

            this.ChartHeadings = new HeadingCollection();

            this.CreateFilledRegions(filledRegionCache, view);

            ViewportUtils.CreateChartViewport(sheet, view, this.ViewportOrigin);
        }

        /// <summary>
        /// Creates the filled regions in Revit on the <paramref name="view"/> and adds the heading
        /// text notes as each one is created.
        /// </summary>
        public void CreateFilledRegions(FilledRegionCache filledRegionCache, View view)
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
                if (rectangleBoundaries != null)
                {
                    var filledRegionName = rectangle["material"].ToString();

                    var filledRegionType = filledRegionCache.GetByName(filledRegionName);

                    var filledRegion = FilledRegion.Create(document, filledRegionType.Id, viewId, rectangleBoundaries);

                    filledRegion.SetLineStyleId(invisibleLinesId);

                    var percentage = Math.Round(((width * height) / total) * 100, 1);

                    var chartAnnotation = new TreeChartHeading(view, origin, $" {percentage}%", height, width);

                    this.ChartHeadings.Add(chartAnnotation);
                }
            }
        }

        /// <summary>
        /// Returns a list of label headings in this chart.
        /// </summary>
        public List<IHeading> GetAllLabels()
        {
            var chartHeadingList = new List<IHeading>();
            chartHeadingList.Add(this.MainHeading);
            chartHeadingList.AddRange(this.ChartHeadings);

            return chartHeadingList;
        }
    }
}
