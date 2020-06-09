using CarbonEmissionTool.Model.Enums;

namespace CarbonEmissionTool.Model.Interfaces
{
    interface IProjectDetails
    {
        /// <summary> The address of the project. This is passed into the form when it is intialized to auto-populate the field.</summary>
        string ProjectAddress { get; set; }

        /// <summary> The name of the project. This is passed into the form when it is intialized to auto-populate the field</summary>
        string ProjectName { get; set; }

        /// <summary>
        /// The version of the project input by the user.
        /// </summary>
        string ProjectVersion { get; set; }

        /// <summary>
        /// The projects RIBA workstage number.
        /// </summary>
        string RibaWorkstage { get; set; }

        /// <summary>
        /// The total floor area in m2 input by the user.
        /// </summary>
        double FloorArea { get; set; }

        /// <summary>
        /// True if the project is a new build, otherwise false.
        /// </summary>
        bool NewBuild { get; set; }

        /// <summary>The export status of the project</summary>
        CarbonExportStatus ExportStatus { get; set; }
    }
}
