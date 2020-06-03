using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.DB;

namespace CarbonEmissionTool.Model.Utilities
{
    class ViewportUtils
    {
        internal static void SetViewportType(Document doc, Viewport viewport)
        {
            List<ElementType> elementTypes = new FilteredElementCollector(doc).OfClass(typeof(ElementType)).WhereElementIsElementType().Cast<ElementType>().ToList();

            for (int i = 0; i < elementTypes.Count; i++)
            {
                if (elementTypes[i].Name == "No Title")
                {
                    Parameter param = viewport.get_Parameter(BuiltInParameter.ELEM_TYPE_PARAM);
                    param.Set(elementTypes[i].Id);
                    break;
                }
            }
        }
    }
}