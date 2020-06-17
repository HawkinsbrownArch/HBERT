using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using CarbonEmissionTool.Services;
using CarbonEmissionTool.View;
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

            this.IconName = "HBERT_icon.png";
        }

        /// <summary>
        /// The process which executes when the button is clicked by the user.
        /// </summary>
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Document doc = commandData.Application.ActiveUIDocument.Document; // Update the static field to hold current database document
            
            ApplicationServices.OnStartup(doc);

            var hbertMainWindow = new HbertMainWindow();
            hbertMainWindow.ShowDialog();

            ApplicationServices.OnShutdown();

            return Result.Succeeded;
        }
    }
}