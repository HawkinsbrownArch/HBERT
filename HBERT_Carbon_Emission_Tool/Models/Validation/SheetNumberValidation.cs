using CarbonEmissionTool.Services;

namespace CarbonEmissionTool.Models
{
    class SheetNumberValidation
    {
        /// <summary>
        /// Validates the sheet number input by the user.
        /// </summary>
        public static string Validate(string sheetNumber)
        {
            if (string.IsNullOrWhiteSpace(sheetNumber))
                return "Value cannot be empty";

            var invalidCharacters = ApplicationServices.InvalidCharacters;

            if (!NameUtils.ValidCharacters(sheetNumber))
                return $"Value cannot contain any of the following characters: {invalidCharacters}";

            if (SheetUtils.Exists(sheetNumber))
                return "Sheet number in use. Please input a unique number";

            return string.Empty;
        }
    }
}
