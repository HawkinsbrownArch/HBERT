using Autodesk.Revit.UI;
using CarbonEmissionTool.Services;

namespace CarbonEmissionTool.Models
{
    public class WarningDialog
    {
        /// <summary>
        /// Displays a warning to the user if the <paramref name="columnName"/> cant be found in the
        /// <see cref="ApplicationServices.CarbonSchedule"/>.
        /// </summary>
        public static void NoScheduleColumnFound(string columnName = "")
        {
            var warningStatement = columnName == "" ? "the required fields" : $"a required field: '{columnName}'";

            TaskDialog td = new TaskDialog("Invalid HBA Embodied Carbon Schedule")
            {
                MainInstruction = $"The input Embodied Carbon schedule does not contain {warningStatement}. To resolve, delete the schedule and re-import the 'Embodied Carbon (Do Not Delete)' schedule from the supplied H\\B:ERT Revit template.",
                AllowCancellation = false,
                CommonButtons = TaskDialogCommonButtons.Ok
            };
            td.Show();
        }


        /// <summary>
        /// Thrown if the schedule with the EC ratings contains no data
        /// </summary>
        public static void ScheduleContainsNoData()
        {
            TaskDialog td = new TaskDialog("No embodied carbon data found")
            {
                MainInstruction = @"Carbon Rating procedure cancelled: no embodied carbon data found in the schedule. Ensure your schedule shows EC values greater than 0.0 under the 'Overall EC sum (kgCO2e)' column.",
                AllowCancellation = false,
                CommonButtons = TaskDialogCommonButtons.Ok
            };
            td.Show();
        }

        /// <summary>
        /// Thrown if the schedule with the EC ratings is not found in the active document even after
        /// attempting to automatically import it.
        /// </summary>
        public static void ScheduleNotFound()
        {
            TaskDialog td = new TaskDialog("Carbon schedule not found")
            {
                MainInstruction = @"HBERT can't run as the embodied carbon schedule cannot be found. Please ensure the supplied Revit templates were installed to automatically import the schedule when you next run the tool.",
                AllowCancellation = false,
                CommonButtons = TaskDialogCommonButtons.Ok
            };
            td.Show();
        }
    }
}
