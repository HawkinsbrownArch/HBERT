using System.Text.RegularExpressions;

namespace CarbonEmissionTool.Model.Utilities
{
    class NamingUtils
    {

        //Validates file names by removing any illegal characters input by the user
        internal static string ValidateFileName(string projectName) //  #Uses a Regular Expression to clean the new view name of any illegal characters
        {
            Regex cleanExpression = new Regex(@"[][\:{}|;<>?`~]");

            var fileNameCleaned = cleanExpression.Replace(projectName, ""); //The new view name cleaned

            return fileNameCleaned;
        }
    }
}