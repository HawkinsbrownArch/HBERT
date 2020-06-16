using System.Collections.Generic;
using Autodesk.Revit.DB;
using CarbonEmissionTool.Model.Annotations;

namespace CarbonEmissionTool.Model.Interfaces
{
    public interface IChartData
    {
        XYZ ViewportOrigin { get; }

        AnnotationCollection ChartAnnotations { get; }

        IAnnotation MainHeading { get; }

        /// <summary>
        /// Returns a list of label headings in this chart.
        /// </summary>
        List<IAnnotation> GetAllLabels();

        /// <summary>
        /// Creates filled regions of this chart in a Revit view.
        /// </summary>
        void CreateFilledRegions(IProjectDetails projectDetails, Autodesk.Revit.DB.View view);
    }
}
