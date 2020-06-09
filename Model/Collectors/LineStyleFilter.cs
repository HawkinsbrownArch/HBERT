using Autodesk.Revit.DB;
using System.Collections.Generic;
using System.Linq;

namespace CarbonEmissionTool.Model.Collectors
{
    class LineStyleFilter
    {
        /// <summary>
        /// Returns the invisible line style element ID so charts have no boarders.
        /// </summary>
        internal static ElementId GetInvisibleStyleId(Document doc, string invisibleLineStyleName)
        {
            List<GraphicsStyle> lineStyles = new FilteredElementCollector(doc).OfClass(typeof(GraphicsStyle)).Cast<GraphicsStyle>().ToList();

            GraphicsStyle invisibleGraphicStyle = null;
            foreach (GraphicsStyle lStyle in lineStyles)
            {
                if (lStyle.Name.Contains(invisibleLineStyleName))
                {
                    invisibleGraphicStyle = lStyle;
                    break;
                }
            }
            return invisibleGraphicStyle.Id;
        }
    }
}
