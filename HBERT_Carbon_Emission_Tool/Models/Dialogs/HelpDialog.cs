using Autodesk.Revit.UI;

namespace CarbonEmissionTool.Models
{
    public class HelpDialog
    {
        /// <summary>
        /// Displays if the HBERT tool successfully runs, providing feedback to the user that the process has completed.
        /// </summary>
        public static void HbertSuccessfullyRun()
        {
            TaskDialog td = new TaskDialog("HBERT Process Completed")
            {
                MainInstruction = @"HBERT has successfully run. Please locate your sheet to view the results!",
                AllowCancellation = false,
                CommonButtons = TaskDialogCommonButtons.Ok
            };
            td.Show();
        }
    }
}
