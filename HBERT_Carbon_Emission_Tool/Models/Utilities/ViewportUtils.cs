using Autodesk.Revit.DB;
using CarbonEmissionTool.Services;

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
        /// Gets the no title viewport type. If it doesn't exist, returns the first viewport type found.
        /// </summary>
        public static ElementType GetNoTitleViewportType()
        {
            ElementId id = new ElementId(BuiltInParameter.ALL_MODEL_FAMILY_NAME);
            ParameterValueProvider provider = new ParameterValueProvider(id);
            FilterStringRuleEvaluator evaluator = new FilterStringEquals();

            string elementTypeFamilyName = "Viewport";

            FilterRule rule = new FilterStringRule(provider, evaluator, elementTypeFamilyName, false);

            ElementParameterFilter filter = new ElementParameterFilter(rule);

            var elementTypes = new FilteredElementCollector(ApplicationServices.Document).OfClass(typeof(ElementType)).WhereElementIsElementType().WherePasses(filter);

            foreach (ElementType elementType in elementTypes)
            {
                if (elementType.Name == "No Title")
                {
                    return elementType;
                }
            }

            return (ElementType)elementTypes.FirstElement();
        }
    }
}