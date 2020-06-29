using System.Globalization;
using System.Windows.Controls;
using CarbonEmissionTool.Models;
using CarbonEmissionTool.Services;

namespace CarbonEmissionTool.ViewModels
{
    class SheetNameValidation : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var sheetName = value as string;

            if (string.IsNullOrWhiteSpace(sheetName))
                return new ValidationResult(false, "Value cannot be empty");

            var invalidCharacters = ApplicationServices.InvalidCharacters;

            if (NameUtils.HasValidCharacters(sheetName))
                return new ValidationResult(false, $"Value cannot contain any of the following characters: {invalidCharacters}");

            return ValidationResult.ValidResult;
        }
    }
}
