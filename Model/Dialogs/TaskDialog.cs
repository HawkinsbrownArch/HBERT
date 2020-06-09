﻿using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using CarbonEmissionTool.Model.Enums;
using CarbonEmissionTool.Services;

namespace CarbonEmissionTool.Model.Dialogs
{
    class TaskDialog
    {
        /// <summary>
        /// Validates if the name of the <paramref name="schedule"/> matches the
        /// <see cref="ApplicationServices.EmbodiedCarbonScheduleName"/>.
        /// </summary>
        internal static void ValidateCarbonSchedule(ViewSchedule schedule, ref bool isValid)
        {
            string scheduleName = ApplicationServices.EmbodiedCarbonScheduleName;

            if (schedule.Name != scheduleName)
            {
                Autodesk.Revit.UI.TaskDialog td = new Autodesk.Revit.UI.TaskDialog("CarbonEmissionToolMain")
                {
                    Title = "Invalid HBA Embodied Carbon Schedule",
                    MainInstruction = $"The input schedule name is invalid. Ensure the '{scheduleName}' schedule is imported into your poject from the supplied H\\B:ERT Revit template",
                    AllowCancellation = false,
                    CommonButtons = TaskDialogCommonButtons.Ok
                };
                td.Show();

                isValid = false;
            }
        }

        /// <summary>
        /// Displays a warning to the user if the <paramref name="columnName"/> cant be found in the
        /// <see cref="ApplicationServices.CarbonSchedule"/>.
        /// </summary>
        internal static void NoScheduleColumnIndexFound(int columnIndex, string columnName)
        {
            if(columnIndex < 0)
            {
                Autodesk.Revit.UI.TaskDialog td = new Autodesk.Revit.UI.TaskDialog("CarbonEmissionToolMain")
                {
                    Title = "Invalid HBA Embodied Carbon Schedule",
                    MainInstruction = $"The input Embodied Carbon schedule does not contain a required field: '{columnName}'. To resolve, delete the schedule and re-import the 'Embodied Carbon (Do Not Delete)' schedule from the supplied H\\B:ERT Revit template.",
                    AllowCancellation = false,
                    CommonButtons = TaskDialogCommonButtons.Ok
                };
                td.Show();
            }
        }

        /// <summary>
        /// Function which determines the actions to take should the user close the form prematurely using the
        /// close button.
        /// </summary>
        internal static bool UserClosedForm(dynamic form)
        {
            // If the form is cancelled by the user, throw UserCancelledForm message. AcceptAndRun = true means the
            // AcceptAndRun button has been clicked by the user and the form is closed. Hence != ensures the if
            // statement doesn't accidentally fire as a result of it being disposed on the close() command when
            // the button is successfully clicked.
            if (form.IsDisposed & form.ExportStatus == CarbonExportStatus.None)  
            {
                Autodesk.Revit.UI.TaskDialog td = new Autodesk.Revit.UI.TaskDialog("CarbonEmissionToolMain")
                {
                    Title = "CarbonEmissionToolMain Carbon Rating Tool",
                    MainInstruction = "Carbon Rating procedure cancelled."
                };
                td.Show();

                return false;
            }
            return true;
        }

        /// <summary>
        /// Thrown if the EC sheet has been created in a previous run and is the active view when the tool is run again
        /// </summary>
        internal static void SheetIsActive()
        {
            //If the ebodied carbon sheet is active it cant be deleted which prevents the process from re-running. Inform the user so they can activate another view to circumvent the problem
            Autodesk.Revit.UI.TaskDialog td = new Autodesk.Revit.UI.TaskDialog("CarbonEmissionToolMain")
            {
                Title = "Warning: Unable to re-compute the embodied carbon rating",
                MainInstruction = @"The embodied carbon rating cannot be re-processed while the report sheet is active. Please activate a different view and try again.",
                AllowCancellation = false,
                CommonButtons = TaskDialogCommonButtons.Ok
            };
            td.Show();
        }

        /// <summary>
        /// Thrown if the schedule with the EC ratings contains no data
        /// </summary>
        internal static void ScheduleContainsNoData()
        {
            Autodesk.Revit.UI.TaskDialog td = new Autodesk.Revit.UI.TaskDialog("CarbonEmissionToolMain")
            {
                Title = "Warning: No embodied carbon data found",
                MainInstruction = @"Carbon Rating procedure cancelled: no embodied carbon data found in the schedule. Ensure your schedule shows EC values greater than 0.0 under the 'Overall EC sum (kgCO2e)' column.",
                AllowCancellation = false,
                CommonButtons = TaskDialogCommonButtons.Ok
            };
            td.Show();
        }

        /// <summary>
        /// Thrown if the one of the inputs on the input form is not complete
        /// </summary>
        internal static void MakeFormSelection(string message)
        {
            Autodesk.Revit.UI.TaskDialog td = new Autodesk.Revit.UI.TaskDialog("CarbonEmissionToolMain")
            {
                Title = "Form Selection",
                MainInstruction = message,
                AllowCancellation = false,
                CommonButtons = TaskDialogCommonButtons.Ok
            };
            td.Show();
        }
    }
}
