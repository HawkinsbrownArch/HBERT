using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBERT_UI
{
    public class ProjectDetails
    {
        /// <summary> The address of the project. This is passed into the form when it is intialized to auto-populate the field.</summary>
        internal string ProjectAddress { get; set; }
        /// <summary> The name of the project. This is passed into the form when it is intialized to auto-populate the field</summary>
        internal string ProjectName { get; set; }

        /// <summary>The export status of the project</summary>
        internal HBFormUtils.CarbonExportStatus ExportStatus { get; set; } = HBFormUtils.CarbonExportStatus.None;

        internal Document ActiveDocument { get; set; }

        internal string Education { get; set; } = "";
        internal string Workplace { get; set; } = "";
        internal string Infrastructure { get; set; } = "";
        internal string Residential { get; set; } = "";
        internal string Civic { get; set; } = "";

        internal string ProjectVersion { get; set; } = "";
        internal string RibaWorkstage { get; set; } = "";
        internal bool NewBuild { get; set; } 
        internal bool FormShutDown { get; set; } = false;

        internal double FloorArea { get; set; } = 0.0;
        internal string Sector => HBFormUtils.GenerateSectorString(new string[] { Education, Workplace, Infrastructure, Residential, Civic });

        internal Dictionary<string, bool> BuildElementsSelection { get; set; }
        internal Dictionary<string, bool> SectorSelection { get; set; }

        internal FamilySymbol TitleBlock { get; set; }
        internal ViewSchedule CarbonSchedule { get; set; }
        internal View3D AxoView { get; set; }

        internal ProjectDetails(Document doc)
        {
            ProjectInfo projectInfo = doc.ProjectInformation;

            ProjectName = projectInfo.Name;
            ProjectAddress = projectInfo.Address;
            BuildElementsSelection = new Dictionary<string, bool>()
            {
                {"StructuralFrame", false },
                {"Facade", false },
                {"ExternalWorks", false },
                {"Roof", false },
                {"Fittings", false },
                {"Foundations", false },
                {"ExternalWindowsDoors", false },
                {"InternalWalls", false },
                {"InternalFinishes", false },
                {"Other", false }
            };

            SectorSelection = new Dictionary<string, bool>
            {
                {"Education", false },
                {"Workplace", false },
                {"Infrastructure", false },
                {"Residential", false },
                {"Civic", false }
            };
            
            ActiveDocument = doc;
        }
    }
}
