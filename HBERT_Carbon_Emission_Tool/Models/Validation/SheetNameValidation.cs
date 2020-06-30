using CarbonEmissionTool.Services;

namespace CarbonEmissionTool.Models
{
    class SheetNameValidation
    {
        /// <summary>
        /// Validates the sheet name input by the user.
        /// </summary>
        public static string Validate(string sheetName)
        {
            if (string.IsNullOrWhiteSpace(sheetName))
                return "Value cannot be empty";

            var invalidCharacters = ApplicationServices.InvalidCharacters;

            if (!NameUtils.ValidCharacters(sheetName))
                return $"Value cannot contain any of the following characters: {invalidCharacters}";

            return string.Empty;
        }
    }
}
