﻿using Autodesk.Revit.DB;
using CarbonEmissionTool.Services;
using CarbonEmissionTool.Settings;

namespace CarbonEmissionTool.Models
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

        /// <summary>
        /// Try's to get the embodied carbon schedule which HBERT tool requires to run. If it cant be found
        /// this method imports it from the supplied Revit templates.
        /// </summary>
        public static ViewSchedule TryGetCarbonSchedule()
        {
            var carbonSchedule = RevitScheduleFilter.GetCarbonSchedule();

            // If the schedule wasn't found in the active document, attempt to import it. 
            if (carbonSchedule == null)
            {
                ScheduleUtils.ImportECScedule();

                carbonSchedule = RevitScheduleFilter.GetCarbonSchedule();
            }

            return carbonSchedule;
        }
    }
}
