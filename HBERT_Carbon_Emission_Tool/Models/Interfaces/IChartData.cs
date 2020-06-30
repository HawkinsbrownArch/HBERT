using System.Collections.Generic;
using Autodesk.Revit.DB;
using CarbonEmissionTool.Services;

namespace CarbonEmissionTool.Models
{
    public interface IChartData
    {
        XYZ ViewportOrigin { get; }

        HeadingCollection ChartHeadings { get; }

        IHeading MainHeading { get; }

        /// <summary>
        /// Returns a list of label headings in this chart.
        /// </summary>
        List<IHeading> GetAllLabels();

        /// <summary>
        /// Creates filled regions of this chart in a Revit view.
        /// </summary>
        void CreateFilledRegions(FilledRegionCache filledRegionCache, View view);
    }
}
