using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using CarbonEmissionTool.Models;
using CarbonEmissionTool.Settings;

namespace CarbonEmissionTool.Services
{
    public class ApplicationServices
    {
        public static Document Document { get; private set; }
        public static Application Application { get; private set; }
        public static UIApplication UIApp { get; private set; }

        /// <summary>
        /// Revits short curve tolerance.
        /// </summary>
        public static double ShortCurveTolerance { get; private set; }

        /// <summary>
        /// The <see cref="Autodesk.Revit.DB.ElementId"/> of the invisible line style in Revit.
        /// </summary>
        public static ElementId InvisibleLinesId { get; private set; }

        /// <summary>
        /// The embodied <see cref="CarbonDataCache"/> HBERT requires to run.
        /// </summary>
        public static CarbonDataCache CarbonDataCache { get; private set; }

        /// <summary>
        /// The 'No Title' viewport element type from the active Revit document.
        /// </summary>
        public static ElementType NoTitleViewportType { get; private set; }

        /// <summary>
        /// The Drafting View element type.
        /// </summary>
        public static ViewFamilyType DraftingViewFamilyType { get; private set; }

        /// <summary>
        /// The <see cref="ElementId"/> of the Revit Solid fill pattern.
        /// </summary>
        public static ElementId SolidFillPatternId { get; private set; }
        
        /// <summary>
        /// The characters which Revit disallows if used to name views or sheets.
        /// </summary>
        public static string InvalidCharacters = @"[]\:{ }|;<>?`~";

        /// <summary>
        /// Processes which are required for the warning tool on startup.
        /// </summary>
        public static void OnStartup(Document document)
        {
            Document = document;

            Application = document.Application;

            UIApp = new UIApplication(Application);

            ShortCurveTolerance = document.Application.ShortCurveTolerance;

            InvisibleLinesId = LineStyleFilter.GetInvisibleStyleId(document, ApplicationSettings.InvisibleLineStyleName);

            NoTitleViewportType = ViewportUtils.GetNoTitleViewportType();

            DraftingViewFamilyType = DraftingViewFilter.GetDraftingViewFamilyType();

            CarbonDataCache = new CarbonDataCache();
            
            SolidFillPatternId = new ElementId(3);
        }

        /// <summary>
        /// Processes which are required when the warning tool is closed.
        /// </summary>
        public static void OnShutdown()
        {

        }
    }
}