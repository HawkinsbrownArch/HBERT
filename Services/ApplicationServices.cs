using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace CarbonEmissionTool.Services
{
    public class ApplicationServices
    {
        public static Document Document { get; private set; }

        public static Application Application { get; private set; }

        public static UIApplication UIApp { get; private set; }

        /// <summary>
        /// Processes which are required for the warning tool on startup.
        /// </summary>
        public static void OnStartup(Document document)
        {
            Document = document;

            Application = document.Application;

            UIApp = new UIApplication(Application);
        }

        /// <summary>
        /// Processes which are required when the warning tool is closed.
        /// </summary>
        public static void OnShutDown()
        {

        }
    }
}