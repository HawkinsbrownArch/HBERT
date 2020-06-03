using System.Collections.Generic;
using Autodesk.Revit.DB;

namespace CarbonEmissionTool.Model.Utilities
{
    class ParameterUtils
    {
        internal static void SetParameters(Element element, List<BuiltInParameter> parameterList, List<dynamic> values)
        {
            for(int p = 0; p < parameterList.Count; p++)
            {
                element.get_Parameter(parameterList[p]).Set(values[p]);
            }
        }
    }
}