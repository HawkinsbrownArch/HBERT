using Autodesk.Revit.DB;
using CarbonEmissionTool.Services;
using CarbonEmissionTool.Settings;

namespace CarbonEmissionTool.Models
{
    public class ViewportUtils
    {
        /// <summary>
        /// Creates a new viewport on the <paramref name="newSheet"/> 
        /// </summary>
        public static void CreateChartViewport(ViewSheet newSheet, Autodesk.Revit.DB.View hostView, XYZ viewportOrigin)
        {
            ApplicationServices.Document.Regenerate();

            var viewport = Viewport.Create(ApplicationServices.Document, newSheet.Id, hostView.Id, viewportOrigin);

            ViewportUtils.SetViewportNoTitle(viewport);
        }

        /// <summary>
        /// Sets the viewport type to Revit's No Title view family type.
        /// </summary>
        public static void SetViewportNoTitle(Viewport viewport)
        {
            Parameter param = viewport.get_Parameter(BuiltInParameter.ELEM_TYPE_PARAM);
            param.Set(ApplicationServices.NoTitleViewportType.Id);
        }

        /// <summary>
        /// Gets the no title viewport type.
        /// </summary>
        public static ElementType GetNoTitleViewportType()
        {
            var elementTypes = new FilteredElementCollector(ApplicationServices.Document).OfClass(typeof(ElementType)).WhereElementIsElementType();

            foreach (ElementType elementType in elementTypes)
            {
                if (elementType.Name == "No Title")
                {
                    return elementType;
                }
            }

            return null;
        }
    }
}