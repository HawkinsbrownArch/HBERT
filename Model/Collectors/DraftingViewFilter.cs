using Autodesk.Revit.DB;
using CarbonEmissionTool.Services;

namespace CarbonEmissionTool.Model.Collectors
{
    public class DraftingViewFilter
    {
        /// <summary>
        /// Returns the drafting view <see cref="ViewFamilyType"/>.
        /// </summary>
        public static ViewFamilyType GetDraftingViewFamilyType()
        {
            var viewFamilyTypes = new FilteredElementCollector(ApplicationServices.Document).OfClass(typeof(ViewFamilyType)).WhereElementIsElementType();

            foreach (ViewFamilyType viewFamilyType in viewFamilyTypes)
            {
                if (viewFamilyType.ViewFamily == ViewFamily.Drafting)
                {
                    return viewFamilyType;
                }
            }
            return null;
        }
    }
}