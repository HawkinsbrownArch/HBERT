using System.Globalization;
using System.Windows.Controls;

namespace CarbonEmissionTool.ViewModels
{
    class FloorAreaValidation : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            int? areaValue = value as int?;

            if (areaValue == null)
                return new ValidationResult(false, "Enter a valid area value");

            if (areaValue <= 0)
                return new ValidationResult(false, "Area must be greater than 0");

            return ValidationResult.ValidResult;
        }
    }
}
