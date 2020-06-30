using System.Text.RegularExpressions;
using CarbonEmissionTool.Services;
using CarbonEmissionTool.Settings;

namespace CarbonEmissionTool.Models
{
    public class NameUtils
    {
        /// <summary>
        /// Returns the text style name required for HBERT.
        /// </summary>
        public static string GenerateTextStyleName(ColorData colorData, FontSize fontSize)
        {
            return $"{ApplicationSettings.TextStyleNamePrefix}{fontSize.ToString()}_{colorData.Name}";
        }

        /// <summary>
        /// Returns true if the viewName has no illegal characters according to Revit naming rules.
        /// </summary>
        public static bool ValidCharacters(string viewName)
        {
            var viewNameCleaned = ApplicationServices.CleanExpression.Replace(viewName, "");
            
            return viewNameCleaned.Length == viewName.Length;
        }

        /// <summary>
        /// Validates file names by removing any illegal characters.
        /// </summary>
        internal static string ValidateFileName(string projectName) 
        {
            Regex cleanExpression = new Regex(@"[][\:{}|;<>?`~]");

            var fileNameCleaned = cleanExpression.Replace(projectName, "");

            return fileNameCleaned;
        }
    }
}