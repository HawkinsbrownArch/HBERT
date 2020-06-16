using System.Text.RegularExpressions;

namespace CarbonEmissionTool.Model.Utilities
{
    public class NamingUtils
    {
        /// <summary>
        /// Validates file names by removing any illegal characters input by the user
        /// </summary>
        public static string ValidateFileName(string projectName)
        {
            Regex cleanExpression = new Regex(@"[][\:{}|;<>?`~]");

            var fileNameCleaned = cleanExpression.Replace(projectName, ""); //The new view name cleaned

            return fileNameCleaned;
        }
    }
}