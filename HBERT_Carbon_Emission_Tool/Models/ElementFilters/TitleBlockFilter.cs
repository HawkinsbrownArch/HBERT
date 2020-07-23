using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.DB;

namespace CarbonEmissionTool.Models
{
    public class TitleBlockFilter
    {
        /// <summary>
        /// Returns a list of all the title block <see cref="FamilySymbol"/> in the active document.
        /// </summary>
        public static List<FamilySymbol> GetAll(Document doc)
        {
            var titleBlocks = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_TitleBlocks)
                .WhereElementIsElementType().Cast<FamilySymbol>().ToList();

            return titleBlocks;
        }
    }
}
