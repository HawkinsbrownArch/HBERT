using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;

namespace CarbonEmissionTool.Model.Collectors
{
    class LineStyleFilter
    {
        //Returns the invisible line style element ID so charts have no boarders
        internal static ElementId GetInvisibleStyleId(Document doc)
        {
            List<GraphicsStyle> lineStyles = new FilteredElementCollector(doc).OfClass(typeof(GraphicsStyle)).Cast<GraphicsStyle>().ToList();

            GraphicsStyle invisibleGraphicStyle = null;
            foreach (GraphicsStyle lStyle in lineStyles)
            {
                if (lStyle.Name == "<Invisible lines>")
                {
                    invisibleGraphicStyle = lStyle;
                    break;
                }
            }
            return invisibleGraphicStyle.Id;
        }
    }
}
