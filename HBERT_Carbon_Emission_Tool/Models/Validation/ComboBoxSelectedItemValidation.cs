namespace CarbonEmissionTool.Models
{
    class ComboBoxSelectedItemValidation
    {
        /// <summary>
        /// Validates the combo box selection by the user.
        /// </summary>
        public static string Validate(object value)
        {
            if (value == null)
                return "Please make a selection";

            return string.Empty;
        }
    }
}
