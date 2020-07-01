using System.Collections.Generic;
using CarbonEmissionTool.Models;
using CarbonEmissionTool.Services;

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
        /// The default font name.
        /// </summary>
        public const string FontDefault = "Arial";

        /// <summary>
        /// The name of JSON storing the color data associated with each carbon material and used to color the charts.
        /// </summary>
        public const string MaterialColorJsonFileName = "HB_Material_ColourScheme.json";

        /// <summary>
        /// The fallback chart color name if a material cannot be found in the <see cref="ChartColorCache"/>.
        /// </summary>
        public const string NotFoundColorName = "Not_Found";

        /// <summary>
        /// The prefix added to the name of the text styles required by HBERT. 
        /// </summary>
        public const string TextStyleNamePrefix = "HBA";

        /// <summary>
        /// The name assigned to a <see cref="CarbonData"/> if its value is derived from summed values of multiple
        /// materials. This occurs if the carbon value of each material is less than the <see cref="SmallValueThreshold"/>.
        /// </summary>
        public const string CarbonDataSummedName = "Other";

        /// <summary>
        /// The legal statement displayed to the user under the Legal terms of the tool.
        /// </summary>
        public const string LegalStatement = @"This tool is supplied by Hawkins\Brown Architects as a beta version for research and academic use. It is intended for use at RIBA work stages 0 to 2 as a concept stage iterative design tool to carry out comparative studies for finite building elements or whole structures. The tool was developed in collaboration with University College London's Institute for Environmental Design and Engineering. All carbon factors are taken from the Bath Inventory of Carbon and Energy 2011 database. Developed by Hawkins\Brown Architects in collaboration with Bimorph Digital Engineering. Copyright 2020.";

        /// <summary>
        /// The disclaimer for the carbon calculation.
        /// </summary>
        public const string CarbonCalculationDisclaimer =
            "System boundary: Life Cycle Stages A1-A5, B4, C1-C4 according to BS EN 15978.\r\rEmbodied carbon does not include carbon sequestration(stored embodied carbon).";

        /// <summary>
        /// The maximum width in mm of the text note headings.
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
        /// The offset in mm from the left side of the sheet which is used to position headings consistently
        /// on the sheet view.
        /// </summary>
        public const double HeadingOffsetFromLeftSide = 15.0;

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