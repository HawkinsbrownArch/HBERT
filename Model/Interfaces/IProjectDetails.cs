using System.ComponentModel;
using CarbonEmissionTool.Model.Collections;
using CarbonEmissionTool.Model.Enums;
using CarbonEmissionTool.Services.Caches;

namespace CarbonEmissionTool.Model.Interfaces
{
    public interface IProjectDetails : INotifyPropertyChanged
    {
        /// <summary> The address of the project. This is passed into the form when it is intialized to auto-populate the field.</summary>
        string Address { get; set;  }

        /// <summary> The name of the project. This is passed into the form when it is intialized to auto-populate the field</summary>
        string Name { get; set; }

        /// <summary>
        /// The version of the project input by the user.
        /// </summary>
        string Version { get; set; }

        /// <summary>
        /// The projects RIBA workstage number.
        /// </summary>
        string RibaWorkstage { get; set; }

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
        SelectedItemCollection BuildElements { get; }

        /// <summary>
        /// The building sectors displayed as check boxes on the UI window.
        /// </summary>
        SelectedItemCollection Sectors { get; }

        /// <summary>
        /// The <see cref="CarbonDataCache"/>.
        /// </summary>
        CarbonDataCache CarbonDataCache { get; }

        /// <summary>
        /// The <see cref="ChartColorCache"/>.
        /// </summary>
        ChartColorCache ChartColorCache { get; }

        /// <summary>
        /// The <see cref="FilledRegionCache"/>.
        /// </summary>
        FilledRegionCache FilledRegionCache { get; }

        /// <summary>
        /// The <see cref="TextStyleCache"/>.
        /// </summary>
        TextStyleCache TextStyleCache { get; }
    }
}
