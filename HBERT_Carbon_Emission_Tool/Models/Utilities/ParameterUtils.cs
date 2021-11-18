using System.Collections.Generic;
using Autodesk.Revit.DB;
using CarbonEmissionTool.Settings;
using Color = System.Windows.Media.Color;

namespace CarbonEmissionTool.Models
{
    public class ParameterUtils
    {
        /// <summary>
        /// Sets the parameters of the <paramref name="element"/>.
        /// </summary>
        public static void SetParameters(Element element, List<BuiltInParameter> parameterList, List<dynamic> values)
        {
            for (int p = 0; p < parameterList.Count; p++)
            {
               // element.get_Parameter(parameterList[p]).Set(values[p]);
            }
        }

        /// <summary>
        /// Sets the parameters of the <paramref name="textNoteType"/>.
        /// </summary>
        public static void SetTextNoteTypeParameters(TextNoteType textNoteType, FontSize fontSize, ColorData colorData)
        {
            var parameters = new List<BuiltInParameter> { BuiltInParameter.LINE_COLOR, BuiltInParameter.TEXT_SIZE, BuiltInParameter.TEXT_BACKGROUND };

            double fontSizeFt = fontSize.ToDecimalFeet();

            var colorInt = ColorUtils.ConvertColorToInt(colorData);
            var values = new List<dynamic> { colorInt, fontSizeFt, 1 };

            for (int p = 0; p < parameters.Count; p++)
            {
                textNoteType.get_Parameter(parameters[p]).Set(values[p]);
            }

            BuiltInParameter fontType = BuiltInParameter.TEXT_FONT;
            var fontTypeParameter = textNoteType.get_Parameter(fontType);

            fontTypeParameter.Set(ApplicationSettings.FontDefault);
        }
    }
}