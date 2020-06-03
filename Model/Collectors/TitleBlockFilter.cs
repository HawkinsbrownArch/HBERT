using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.DB;

namespace CarbonEmissionTool.Model.Collectors
{
    class TitleBlockFilter
    {
        internal static Dictionary<string, FamilySymbol> GetAll(Document doc)
        {
            List<FamilySymbol> titleBlocks = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_TitleBlocks).WhereElementIsElementType().Cast<FamilySymbol>().ToList();

            Dictionary<string, FamilySymbol> titleBlockDict = new Dictionary<string, FamilySymbol>();
            for (int i = 0; i < titleBlocks.Count; i++)
            {
                titleBlockDict[titleBlocks[i].Name] = titleBlocks[i];
            }

            return titleBlockDict;
        }
    }
}
