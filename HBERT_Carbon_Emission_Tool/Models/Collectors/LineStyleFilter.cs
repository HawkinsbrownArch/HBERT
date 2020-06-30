using System.Linq;
using Autodesk.Revit.DB;

namespace CarbonEmissionTool.Models
{
    public class LineStyleFilter
    {
        /// <summary>
        /// Returns the invisible line style element ID so charts have no boarders.
        /// </summary>
        public static ElementId GetInvisibleStyleId(Document doc, string invisibleLineStyleName)
        {
            var lineStyles = new FilteredElementCollector(doc).OfClass(typeof(GraphicsStyle)).Cast<GraphicsStyle>().ToList();

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
