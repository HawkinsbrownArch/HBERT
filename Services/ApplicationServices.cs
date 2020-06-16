using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using CarbonEmissionTool.Model.Collectors;
using CarbonEmissionTool.Model.Utilities;
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
        /// The embodied carbon <see cref="ViewSchedule"/> HBERT requires to run.
        /// </summary>
        public static ViewSchedule CarbonSchedule { get; private set; }

        /// <summary>
        /// The title block used by HBERT for creating the sheet to present the embodied carbon result.
        /// </summary>
        public static FamilySymbol TitleBlock { get; private set; }

        /// <summary>
        /// The default 3D view in Revit used by HBERT for processing and analysing the model.
        /// </summary>
        public static View3D AxoView { get; private set; }
        
        /// <summary>
        /// The 'No Title' viewport element type from the active Revit document.
        /// </summary>
        public static ElementType NoTitleViewportType { get; private set; }

        /// <summary>
        /// The Drafting View element type.
        /// </summary>
        public static ViewFamilyType DraftingViewFamilyType { get; private set; }

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

            bool isValidSchedule = true;

            TaskDialog.ValidateCarbonSchedule(embodiedCarbonSchedule, ref isValidSchedule);
            CarbonSchedule = RevitScheduleFilter.GetCarbonSchedule();

            TitleBlock = TitleBlockFilter.;

            AxoView = RevitViewFilter.;
        }

        /// <summary>
        /// Processes which are required when the warning tool is closed.
        /// </summary>
        public static void OnShutDown()
        {

        }
    }
}