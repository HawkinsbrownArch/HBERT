using System;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using CarbonEmissionTool.Models;
using CarbonEmissionTool.Services;
using CarbonEmissionTool.Views;
using RevitAddIn.Interfaces;

namespace RevitAddIn.ButtonData
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    class HbertButtonData : IButtonData
    {
        public string ToolTip { get; }

        public string VisibleButtonName { get; }

        public string InternalButtonName { get; }

        public string IconName { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        public HbertButtonData()
        {
            this.VisibleButtonName = "HBERT";
            this.InternalButtonName = "cmdHBERT";
            this.ToolTip = "Launch the Hawkins Brown HBERT Tool.";

            this.IconName = "HBERT_icon_Revit.png";
        }

        /// <summary>
        /// The process which executes when the button is clicked by the user.
        /// </summary>
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Document doc = commandData.Application.ActiveUIDocument.Document; // Update the static field to hold current database document

            var dataCapture = new DataCapture();

            ApplicationServices.OnStartup(doc, dataCapture);

            try
            {
                if (!ApplicationServices.CarbonDataCache.IsEmpty)
                {
                    var hbertMainWindow = new HbertMainWindow();
                    hbertMainWindow.ShowDialog();
                }
            }
            catch (Exception e)
            {
                throw new InvalidOperationException(e.StackTrace);
            }
            finally
            {
                ApplicationServices.OnShutdown();
            }

            return Result.Succeeded;
        }
    }
}