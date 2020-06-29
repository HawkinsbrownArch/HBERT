using System.Globalization;
using System.Windows.Controls;

namespace CarbonEmissionTool.ViewModels
{
    class ComboBoxSelectedItemValidation : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value == null)
                return new ValidationResult(false, "Please make a selection");

            return ValidationResult.ValidResult;
        }
    }
}
