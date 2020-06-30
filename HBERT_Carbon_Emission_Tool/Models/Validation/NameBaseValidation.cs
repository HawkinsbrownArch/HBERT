namespace CarbonEmissionTool.Models
{
    class NameBaseValidation
    {
        /// <summary>
        /// Validates a name input by the user.
        /// </summary>
        public static string Validate(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return "Value cannot be empty";

            return string.Empty;
        }
    }
}
