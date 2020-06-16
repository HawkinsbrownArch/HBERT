using System;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Events;
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

            UIDocument uiDoc = new UIDocument(doc);
            UIApplication uiapp = uiDoc.Application;
            uiapp.DialogBoxShowing += new EventHandler<DialogBoxShowingEventArgs>(app_DialogBoxShowing);

            var hbertWindowApplication = new HbertMainWindow();
            hbertWindowApplication.ShowDialog();

            return Result.Succeeded;
        }

        public void app_DialogBoxShowing(object sender, DialogBoxShowingEventArgs args)
        {
            if (args.DialogId == "Dialog_Revit_PasteSimilarSymbolsPaste")
            {
                args.OverrideResult((int)TaskDialogResult.Yes);
            }
        }
    }
}