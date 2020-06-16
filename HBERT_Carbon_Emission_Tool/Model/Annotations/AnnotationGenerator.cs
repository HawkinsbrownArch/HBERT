using System.Collections.Generic;
using Autodesk.Revit.DB;
using CarbonEmissionTool.Model.Annotations.HeadingPairs;
using CarbonEmissionTool.Model.Annotations.Headings;
using CarbonEmissionTool.Model.Charts.BarCharts;
using CarbonEmissionTool.Model.Charts.TreeCharts;
using CarbonEmissionTool.Model.Interfaces;

namespace CarbonEmissionTool.Model.Annotations
{
    class AnnotationGenerator
    {
        /// <summary>
        /// Generates all the annotation heading and label text notes on the sheet.
        /// </summary>
        public static void Create(IProjectDetails projectDetails, TreeChart treeChart, StackedBarChart stackedBarChart, ViewSheet view)
        {
            var annotationList = new List<IAnnotation>();

            annotationList.AddRange(treeChart.GetAllLabels());
            annotationList.AddRange(stackedBarChart.GetAllLabels());

            annotationList.Add(new ProjectVersionAnnotation(view, projectDetails));
            annotationList.Add(new ProjectNameAnnotation(view, projectDetails));

            // Annotation pairs on the top-left of the sheet.
            annotationList.Add(new DateTitleAnnotation(view));
            annotationList.Add(new DateValueAnnotation(view));

            annotationList.Add(new RibaWorkstageTitleAnnotation(view));
            annotationList.Add(new RibaWorkstageValueAnnotation(view, projectDetails));

            annotationList.Add(new LocationTitleAnnotation(view));
            annotationList.Add(new LocationValueAnnotation(view, projectDetails));

            annotationList.Add(new FloorAreaTitleAnnotation(view));
            annotationList.Add(new FloorAreaValueAnnotation(view, projectDetails));

            annotationList.Add(new TypeTitleAnnotation(view));
            annotationList.Add(new TypeValueAnnotation(view, projectDetails));

            annotationList.Add(new SectorTitleAnnotation(view));
            annotationList.Add(new SectorValueAnnotation(view, projectDetails));

            foreach (var annotation in annotationList)
            {
                TextNoteCreator.Create(projectDetails, annotation);
            }
        }
    }
}
