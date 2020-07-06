using System;
using Autodesk.Revit.DB;

namespace CarbonEmissionTool.Models
{
    /// <summary>
    /// Stores data input by the user for a given document, enabling the HBERT form to be
    /// re-populated with the same data the next time it is opened.
    /// </summary>
    class InputCapture
    {
        /// <summary>
        /// The address of the project input by the user.
        /// </summary>
        public string Address { get; }

        /// <summary>
        /// The name of the project input by the user.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The revision of the project input by the user.
        /// </summary>
        public string Revision { get; }

        /// <summary>
        /// The RIBA workstage input by the user.
        /// </summary>
        public RibaWorkstage RibaWorkstage { get; }

        /// <summary>
        /// The project type input by the user.
        /// </summary>
        public ProjectType ProjectType { get; }

        /// <summary>
        /// The floor area input by the user.
        /// </summary>
        public double FloorArea { get; }

        /// <summary>
        /// The building elements selected by the user.
        /// </summary>
        public CheckBoxItemCollection BuildElements { get; }

        /// <summary>
        /// The sectors selected by the user.
        /// </summary>
        public CheckBoxItemCollection Sectors { get; }

        /// <summary>
        /// The document hashcode which the session inputs belong to.
        /// </summary>
        public int DocumentHash { get; }

        /// <summary>
        /// Constructs a new <see cref="InputCapture"/> from the provided <paramref name="projectDetails"/>.
        /// </summary>
        public InputCapture(Document document, IProjectDetails projectDetails)
        {
            this.DocumentHash = document.GetHashCode();

            this.Name = projectDetails.Name;
            this.Address = projectDetails.Address;
            this.Revision = projectDetails.Revision;
            this.ProjectType = projectDetails.ProjectType;
            this.RibaWorkstage = projectDetails.RibaWorkstage;
            this.BuildElements = projectDetails.BuildElements;
            this.Sectors = projectDetails.Sectors;
            this.FloorArea = projectDetails.FloorArea;
        }
    }
}
