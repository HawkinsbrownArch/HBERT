using System.Collections.Generic;
using Autodesk.Revit.DB;

namespace CarbonEmissionTool.Models
{
    public interface IPublishDetails
    {
        /// <summary>
        /// A list of all the 3D views in the active document.
        /// </summary>
        List<View3D> ThreeDViews { get; }

        /// <summary>
        /// A list of all the title block <see cref="Autodesk.Revit.DB.FamilySymbol"/>'s in the active document.
        /// </summary>
        List<FamilySymbol> TitleBlocks { get; }

        /// <summary>
        /// The title block selected by the user for creating the sheet to present the embodied carbon result.
        /// </summary>
        FamilySymbol TitleBlock { get; set; }

        /// <summary>
        /// The 3D view in Revit used by HBERT for processing and analysing the model.
        /// </summary>
        View3D AxoView { get; set; }

        /// <summary>
        /// 
        /// </summary>
        string SheetName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        string SheetNumber { get; set; }
    }
}
