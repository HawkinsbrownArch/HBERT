using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.DB;
using CarbonEmissionTool.Services;

namespace CarbonEmissionTool.Models
{
    public class TitleBlockFilter
    {
        /// <summary>
        /// Returns a list of all the title block <see cref="FamilySymbol"/> in the active document.
        /// </summary>
        public static List<FamilySymbol> GetAll()
        {
            var titleBlocks = new FilteredElementCollector(ApplicationServices.Document).OfCategory(BuiltInCategory.OST_TitleBlocks)
                .WhereElementIsElementType().Cast<FamilySymbol>().ToList();

            return titleBlocks;
        }
    }
}
