using Autodesk.Revit.DB;
using CarbonEmissionTool.Services;
using CarbonEmissionTool.Settings;

namespace CarbonEmissionTool.Models
{
    public class RevitScheduleFilter
    {
        /// <summary>
        /// Gets a schedule from the document by matching the provided name. If no schedule is found returns null.
        /// </summary>
        public static ViewSchedule GetScheduleByName(Document doc, string scheduleName)
        {
            var viewSchedules = new FilteredElementCollector(doc).OfClass(typeof(ViewSchedule)).WhereElementIsNotElementType();

            foreach (ViewSchedule viewSchedule in viewSchedules)
            {
                var isECSchedule = viewSchedule.Definition.IsMaterialTakeoff &
                                   viewSchedule.Name == scheduleName;
                if (isECSchedule)
                    return viewSchedule;
            }
            
            return null;
        }

        /// <summary>
        /// Try's to get the embodied carbon schedule which HBERT tool requires to run. If it cant be found
        /// this method imports it from the supplied Revit templates.
        /// </summary>
        public static ViewSchedule TryGetCarbonSchedule()
        {
            var doc = ApplicationServices.Document;

            var carbonScheduleName = ApplicationSettings.EmbodiedCarbonScheduleName;

            var carbonSchedule = RevitScheduleFilter.GetScheduleByName(doc, carbonScheduleName);

            // If the schedule wasn't found in the active document, attempt to import it. 
            if (carbonSchedule == null)
            {
                ResourceImporter.GetResources();

                carbonSchedule = RevitScheduleFilter.GetScheduleByName(doc, carbonScheduleName);
            }

            return carbonSchedule;
        }
    }
}
