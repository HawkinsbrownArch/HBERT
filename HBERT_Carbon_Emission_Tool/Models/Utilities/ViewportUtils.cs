using System.Linq;
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

            if (ApplicationServices.NoTitleViewportType != null)
                param.Set(ApplicationServices.NoTitleViewportType.Id);
        }

        /// <summary>
        /// Gets the no title viewport type. If it doesn't exist, returns the first viewport type found.
        /// </summary>
        public static ElementType GetNoTitleViewportType()
        {
            var elementTypes = new FilteredElementCollector(ApplicationServices.Document).OfClass(typeof(ElementType)).WhereElementIsElementType();

            foreach (ElementType elementType in elementTypes)
            {
                if (elementType.FamilyName == ApplicationSettings.ViewportFamilyName && elementType.Name == ApplicationSettings.NoTitleViewportTypeName)
                {
                    return elementType;
                }
            }

            foreach (ElementType elementType in elementTypes)
            {
                if (elementType.FamilyName == ApplicationSettings.ViewportFamilyName)
                {
                    return elementType;
                }
            }

            return (ElementType)elementTypes.FirstOrDefault();
        }
    }
}
