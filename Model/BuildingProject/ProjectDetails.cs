using System.Collections.Generic;
using Autodesk.Revit.DB;
using CarbonEmissionTool.Model.Enums;
using CarbonEmissionTool.Model.Interfaces;
using CarbonEmissionTool.Model.Utilities;
using CarbonEmissionTool.Services;

namespace CarbonEmissionTool.Model.BuildingProject
{
    public class ProjectDetails : IProjectDetails
    {
        /// <summary> The address of the project. This is passed into the form when it is intialized to auto-populate the field.</summary>
        public string ProjectAddress { get; set; }
        /// <summary> The name of the project. This is passed into the form when it is intialized to auto-populate the field</summary>
        public string ProjectName { get; set; }

        /// <summary>The export status of the project</summary>
        public CarbonExportStatus ExportStatus { get; set; } = CarbonExportStatus.None;

        public string Education { get; set; } = "";
        public string Workplace { get; set; } = "";
        public string Infrastructure { get; set; } = "";
        public string Residential { get; set; } = "";
        public string Civic { get; set; } = "";

        public string ProjectVersion { get; set; } = "";
        public string RibaWorkstage { get; set; } = "";
        public bool NewBuild { get; set; } 
        public bool FormShutDown { get; set; } = false;

        public double FloorArea { get; set; } = 0.0;

        public string Sector => StringUtils.GenerateSectorString(new[] { Education, Workplace, Infrastructure, Residential, Civic });

        /// <summary>
        /// Todo: this should be a dedicated type which implements IEnumerable and contains a type which represents the selection object.
        /// </summary>
        public Dictionary<string, bool> BuildElementsSelection { get; set; }

        /// <summary>
        /// Todo: this should be a dedicated type which implements IEnumerable and contains a type which represents the selection object.
        /// </summary>
        public Dictionary<string, bool> SectorSelection { get; set; }

        public ProjectDetails()
        {
            ProjectInfo projectInfo = ApplicationServices.Document.ProjectInformation;

            ProjectName = projectInfo.Name;
            ProjectAddress = projectInfo.Address;
            BuildElementsSelection = new Dictionary<string, bool>
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
        }
    }
}
