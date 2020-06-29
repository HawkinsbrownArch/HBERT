using System.Globalization;
using System.Windows.Controls;

namespace CarbonEmissionTool.ViewModels
{
    class NameBaseValidation : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var textValue = value as string;

            if (string.IsNullOrWhiteSpace(textValue))
                return new ValidationResult(false, "Value cannot be empty");

            return ValidationResult.ValidResult;
        }
    }
}
