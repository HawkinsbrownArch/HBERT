using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.DB;

namespace CarbonEmissionTool.Model.Collectors
{
    class DraftingViewFilter
    {
        internal static ElementId GetDraftingViewTypeId(Document doc)
        {
            List<ViewFamilyType> viewTypes = new FilteredElementCollector(doc).OfClass(typeof(ViewFamilyType)).Cast<ViewFamilyType>().ToList();

            ViewFamilyType draftingType = null;
            for (int i = 0; i < viewTypes.Count; i++)
            {
                if (viewTypes[i].ViewFamily == ViewFamily.Drafting)
                {
                    draftingType = viewTypes[i];
                    break;
                }
            }

            return draftingType.Id;
        }
    }
}