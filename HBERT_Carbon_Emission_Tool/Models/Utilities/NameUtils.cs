using System.Text.RegularExpressions;
using System.Windows.Media;
using CarbonEmissionTool.Settings;

namespace CarbonEmissionTool.Models
{
    public class NameUtils
    {
        private static Regex _cleanExpression = new Regex(@"[][\:{}|;<>?`~]");
        
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
        public static bool HasValidCharacters(string viewName)
        {
            var viewNameCleaned = _cleanExpression.Replace(viewName, "");

            if (viewNameCleaned.Length < viewName.Length)
                return false;

            return true;
        }
    }
}