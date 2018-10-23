using System;
using System.IO;
using System.Reflection;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Windows.Media.Imaging;
using Autodesk.Revit.UI.Events;

namespace HBERT_UI
{
    /// <remarks>
    /// This application's main class. The class must be Public.
    /// </remarks>
    public class HbertRibbon : IExternalApplication
    {
        // Both OnStartup and OnShutdown must be implemented as public method
        public Result OnStartup(UIControlledApplication application)
        {
            // Create a custom ribbon tab
            String tabName = "HBERT";
            application.CreateRibbonTab(tabName);

            // Add a new ribbon panel
            RibbonPanel ribbonPanel = application.CreateRibbonPanel(tabName, "HBERT");

            // Create a push button to trigger a command add it to the ribbon panel.
            string thisAssemblyPath = Assembly.GetExecutingAssembly().Location;

            Uri uriImage = new Uri(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\HBERT_icon_32x32.png");
            BitmapImage largeImage = new BitmapImage(uriImage);

            PushButtonData buttonDataHBERT = new PushButtonData("cmdHBERT", "HBERT", thisAssemblyPath, "HBERT_UI.LaunchHbert");
            PushButton pushButtonHBERT = ribbonPanel.AddItem(buttonDataHBERT) as PushButton;
            pushButtonHBERT.ToolTip = "Launch the Hawkins Brown HBERT Tool.";
            pushButtonHBERT.LargeImage = largeImage;

            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication application)
        {
            // nothing to clean up in this simple case

            return Result.Succeeded;
        }
    }

    /// <remarks>
    /// Launches the HBERT Tool.
    /// </remarks>
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class LaunchHbert : IExternalCommand
    {
        // The main Execute method (inherited from IExternalCommand) must be public
        public Result Execute(ExternalCommandData exernalCmd, ref string message, ElementSet elements)
        {
            Document doc = exernalCmd.Application.ActiveUIDocument.Document; // Update the static field to hold current database document

            UIDocument uiDoc = new UIDocument(doc);
            UIApplication uiapp = uiDoc.Application;
            uiapp.DialogBoxShowing += new EventHandler<DialogBoxShowingEventArgs>(app_DialogBoxShowing);

            new HBERT.HBERT().ComputeEmbodiedCarbon(doc);

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
