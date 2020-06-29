using System.Windows.Media;
using CarbonEmissionTool.Services;
using CarbonEmissionTool.Settings;

namespace CarbonEmissionTool.Models
{
    public class NameUtils
    {
        /// <summary>
        /// Returns the text style name required for HBERT.
        /// </summary>
        public static string GenerateTextStyleName(Color color, FontSize fontSize)
        {
            var colorKey = ColorUtils.ConvertColorToInt(color).ToString();

            return $"{ApplicationSettings.TextStyleNamePrefix}{fontSize.ToString()}_{colorKey}";
        }

        /// <summary>
        /// Returns true if the viewName has no illegal characters according to Revit naming rules.
        /// </summary>
        public static bool ValidCharacters(string viewName)
        {
            var viewNameCleaned = ApplicationServices.CleanExpression.Replace(viewName, "");
            
            return viewNameCleaned.Length == viewName.Length;
        }
    }
}