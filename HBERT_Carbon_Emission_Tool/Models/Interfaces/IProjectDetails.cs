
using System.ComponentModel;

namespace CarbonEmissionTool.Models
{
    /// <summary>
    /// Stores the details about a project and implemented as a view model.
    /// </summary>
    public interface IProjectDetails : INotifyDataErrorInfo
    {
        /// <summary>
        /// The address of the project. This is passed into the form when it is intialized to auto-populate the field.
        /// </summary>
        string Address { get; set; }

        /// <summary>
        /// The name of the project. This is passed into the form when it is intialized to auto-populate the field.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// The revision of the project input by the user.
        /// </summary>
        string Revision { get; set; }

        /// <summary>
        /// The projects RIBA workstage number.
        /// </summary>
        RibaWorkstage RibaWorkstage { get; set; }

        /// <summary>
        /// The total floor area in m2 input by the user.
        /// </summary>
        double FloorArea { get; set; }

        /// <summary>
        /// The <see cref="ProjectType"/>.
        /// </summary>
        ProjectType ProjectType { get; set; }

        /// <summary>
        /// The building elements displayed as check boxes on the UI window.
        /// </summary>
        CheckBoxItemCollection BuildElements { get; }

        /// <summary>
        /// The building sectors displayed as check boxes on the UI window.
        /// </summary>
        CheckBoxItemCollection Sectors { get; }

        /// <summary>
        /// The <see cref="IDataCapture"/> object.
        /// </summary>
        IDataCapture DataCapture { get; }
    }
}
