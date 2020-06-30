using System.Collections.Generic;
using Autodesk.Revit.DB;
using CarbonEmissionTool.Models.Annotations;

namespace CarbonEmissionTool.Models.Headings
{
    class HeadingGenerator
    {
        /// <summary>
        /// Generates all the heading heading and label text notes on the sheet.
        /// </summary>
        public static void Create(IProjectDetails projectDetails, IChartData treeChart, IChartData stackedBarChart, ViewSheet view)
        {
            // Any class which implements IHeading needs to be added to this list. 
            var annotationList = new List<IHeading>();

            annotationList.AddRange(treeChart.GetAllLabels());
            annotationList.AddRange(stackedBarChart.GetAllLabels());

            annotationList.Add(new ProjectRevisionHeading(view, projectDetails));
            annotationList.Add(new ProjectNameHeading(view, projectDetails));

            annotationList.Add(new CarbonCalculationDisclaimerHeading(view));

            // Annotation pairs on the top-left of the sheet.
            annotationList.Add(new DateTitleHeading(view));
            annotationList.Add(new DateValueHeading(view));

            annotationList.Add(new RibaWorkstageTitleHeading(view));
            annotationList.Add(new RibaWorkstageValueHeading(view, projectDetails));

            annotationList.Add(new LocationTitleHeading(view));
            annotationList.Add(new LocationValueHeading(view, projectDetails));

            annotationList.Add(new FloorAreaTitleHeading(view));
            annotationList.Add(new FloorAreaValueHeading(view, projectDetails));

            annotationList.Add(new TypeTitleHeading(view));
            annotationList.Add(new TypeValueHeading(view, projectDetails));

            annotationList.Add(new SectorTitleHeading(view));
            annotationList.Add(new SectorValueHeading(view, projectDetails));

            var textNoteCreator = new TextNoteCreator();
            foreach (var annotation in annotationList)
            {
                textNoteCreator.Create(annotation);
            }
        }
    }
}
