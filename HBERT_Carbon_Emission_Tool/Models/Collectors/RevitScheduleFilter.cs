using Autodesk.Revit.DB;
using CarbonEmissionTool.Services;
using CarbonEmissionTool.Settings;

namespace CarbonEmissionTool.Model.Collectors
{
    public class RevitScheduleFilter
    {
        /// <summary>
        /// Gets the embodied carbon schedule which HBERT tool requires to run.
        /// </summary>
        public static ViewSchedule GetCarbonSchedule()
        {
            var viewSchedules = new FilteredElementCollector(ApplicationServices.Document).OfClass(typeof(ViewSchedule)).WhereElementIsNotElementType();

            foreach (ViewSchedule viewSchedule in viewSchedules)
            {
                var isECSchedule = viewSchedule.Definition.IsMaterialTakeoff &
                                   viewSchedule.Name == ApplicationSettings.EmbodiedCarbonScheduleName;
                if (isECSchedule)
                    return viewSchedule;
            }

            return null;
        }
    }
}
