using CarbonEmissionTool.Models;
using System;
using System.Globalization;
using System.Windows.Data;

namespace CarbonEmissionTool.ViewModels
{
    class ProjectTypeToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var projectType = (ProjectType)value;

            var projectTypeSender = Enum.Parse(typeof(ProjectType), parameter.ToString());

            return (ProjectType)projectTypeSender == projectType;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var projectTypeSender = (ProjectType)Enum.Parse(typeof(ProjectType), parameter.ToString());

            var isChecked = value != null && (bool) value;

            var inverseProjectType = projectTypeSender == ProjectType.NewBuild
                ? ProjectType.Refurbishment
                : ProjectType.NewBuild;

            return isChecked ? projectTypeSender : inverseProjectType;
        }
    }
}
