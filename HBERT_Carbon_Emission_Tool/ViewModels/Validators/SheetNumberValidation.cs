using System.Globalization;
using System.Windows.Controls;
using CarbonEmissionTool.Models;
using CarbonEmissionTool.Services;

namespace CarbonEmissionTool.ViewModels
{
    class SheetNumberValidation : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var sheetNumber = value as string;

            if (string.IsNullOrWhiteSpace(sheetNumber))
                return new ValidationResult(false, "Value cannot be empty");

            var invalidCharacters = ApplicationServices.InvalidCharacters;

            if (NameUtils.HasValidCharacters(sheetNumber))
                return new ValidationResult(false, $"Value cannot contain any of the following characters: {invalidCharacters}");

            if (SheetUtils.Exists(sheetNumber))
                return new ValidationResult(false, "Sheet number in use. Please input a unique number");

            return ValidationResult.ValidResult;
        }
    }
}
