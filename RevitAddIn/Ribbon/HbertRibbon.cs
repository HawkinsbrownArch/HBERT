using Autodesk.Revit.UI;
using RevitAddIn.ButtonData;

namespace RevitAddIn.Ribbon
{
    /// <remarks>
    /// This application's main class to create the ribbon in Revit. 
    /// </remarks>
    public class HbertRibbon : IExternalApplication
    {
        // Both OnStartup and OnShutdown must be implemented as public method
        public Result OnStartup(UIControlledApplication application)
        {
            // Create a custom ribbon tab
            var tabName = "HBERT";
            application.CreateRibbonTab(tabName);

            // Add a new ribbon panel
            RibbonPanel ribbonPanel = application.CreateRibbonPanel(tabName, "HBERT");

            RibbonUtils.AddButton(ribbonPanel, new HbertButtonData());

            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication application)
        {
            // nothing to clean up in this simple case

            return Result.Succeeded;
        }
    }
}
