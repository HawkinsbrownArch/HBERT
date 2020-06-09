using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using CarbonEmissionTool.Model.Collectors;

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
        /// The name of the invisible line style in Revit.
        /// </summary>
        private const string InvisibleLineStyleName = "Invisible lines";

        /// <summary>
        /// The name of the carbon schedule HBERT requires to run.
        /// </summary>
        public const string EmbodiedCarbonScheduleName = "Embodied Carbon (Do Not Delete)";

        /// <summary>
        /// The name of the column in the HBERT Carbon schedule which displays the material type.
        /// </summary>
        public const string ScheduleMaterialColumnName = "Material: Name";

        /// <summary>
        /// The name of the column in the HBERT Carbon schedule which displays the overall CO2.
        /// </summary>
        public const string ScheduleOverallEcColumnName = "Overall EC sum (kgCO2e)";

        /// <summary>
        /// The number of the sheet output by the HBERT tool when the user runs the tool.
        /// </summary>
        public const string SheetNumber = "CarbonEmissionToolMain";

        /// <summary>
        /// The name of the sheet output by the HBERT tool when the user runs the tool.
        /// </summary>
        public const string SheetName = "EC Evaluation";

        /// <summary>
        /// The <see cref="ElementId"/> of the invisible line style in Revit.
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
        /// Processes which are required for the warning tool on startup.
        /// </summary>
        public static void OnStartup(Document document)
        {
            Document = document;

            Application = document.Application;

            UIApp = new UIApplication(Application);

            ShortCurveTolerance = document.Application.ShortCurveTolerance;

            InvisibleLinesId = LineStyleFilter.GetInvisibleStyleId(document, InvisibleLineStyleName);

            CarbonSchedule = ;

            TitleBlock = ;

            AxoView = ;
            // double convertPointToMm = 4.347826087; //Converts the fontSizes from point to mm
        }

        /// <summary>
        /// Processes which are required when the warning tool is closed.
        /// </summary>
        public static void OnShutDown()
        {

        }
    }
}