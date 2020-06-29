using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

namespace CarbonEmissionTool.ViewModels
{
    /// <summary>
    /// Multi-value converter which takes an input of <see cref="Control"/>'s and validates the input
    /// is not null or empty.
    /// </summary>
    class ControlInputValidationMultiConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var isInputValid = new List<bool>();
            foreach (var value in values)
            {
                if (value == null)
                    return false;

                var controlType = value.GetType().ToString();
                switch (controlType)
                {
                    case "System.String":
                        var text = value.ToString();

                        isInputValid.Add(text.Length > 0);
                        break;

                    default:
                        var comboBox = (ComboBox)value;

                        isInputValid.Add(comboBox.SelectedIndex > -1);
                        break;
                }
            }

            return isInputValid.TrueForAll(r => r);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
