using System.Collections.Generic;
using CarbonEmissionTool.Model.Enums;
using CarbonEmissionTool.Services.Caches;

namespace CarbonEmissionTool.Settings
{
    public class ApplicationSettings
    {
        /// <summary>
        /// The name of the invisible line style in Revit.
        /// </summary>
        public const string InvisibleLineStyleName = "Invisible lines";

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
        /// The font name used for all annotation in the output HBERT sheet.
        /// </summary>
        public const string HawkinsBrownFont = "HelveticaNeueLT Std";

        /// <summary>
        /// The default font name if <see cref="HawkinsBrownFont"/> cant be found.
        /// </summary>
        public const string FontDefault = "Arial";

        /// <summary>
        /// The name of JSON storing the color data associated with each carbon material.
        /// </summary>
        public const string MaterialColorJsonFileName = "HB_Material_ColourScheme.json";

        /// <summary>
        /// The fallback chart color name if a material cannot be found in the <see cref="ChartColorCache"/>.
        /// </summary>
        public const string NotFoundColorName = "Not_Found";

        /// <summary>
        /// The maximum width in mm of the headings.
        /// </summary>
        public const double MaxTextNoteWidth = 160.0;

        /// <summary>
        /// The spacing in mm between the squares in the tree graph.
        /// </summary>
        public const double TreeGraphPadding = 0.50;

        /// <summary>
        /// The smallest value permissible to display in the charts otherwise values below this threshold
        /// are summed into 1 grouped category.
        /// </summary>
        public const double SmallValueThreshold = 0.025;

        /// <summary>
        /// Converts a <see cref="FontSize"/> from point to mm
        /// </summary>
        public const double ConvertPointToMm = 4.347826087;

        /// <summary>
        /// The building element options displayed as checkboxes under the Building Elements heading in the UI window.
        /// </summary>
        public static readonly List<string> BuildingElementNames = new List<string>
        {
            "Structural Frame",
            "Facade",
            "External Works",
            "Roof",
            "Foundations",
            "Fittings, Furnishings + Equipment",
            "Windows + External Doors",
            "Internal Walls + Partitions",
            "Internal Finishes",
            "Other"
        };

        /// <summary>
        /// The sectors options displayed as checkboxes under the Sector heading in the UI window.
        /// </summary>
        public static readonly List<string> SectorNames = new List<string>
        {
            "Education",
            "Workplace",
            "Infrastructure + Transport",
            "Residential",
            "Civic, Community + Culture"
        };
    }
}