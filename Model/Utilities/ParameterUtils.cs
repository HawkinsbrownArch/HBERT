using Autodesk.Revit.DB;
using System.Collections.Generic;

namespace CarbonEmissionTool.Model.Utilities
{
    public class ParameterUtils
    {
        /// <summary>
        /// Sets the parameters of the <paramref name="element"/>.
        /// </summary>
        public static void SetParameters(Element element, List<BuiltInParameter> parameterList, List<dynamic> values)
        {
            for(int p = 0; p < parameterList.Count; p++)
            {
                element.get_Parameter(parameterList[p]).Set(values[p]);
            }
        }
    }
}