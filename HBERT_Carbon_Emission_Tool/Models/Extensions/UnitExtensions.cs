using Autodesk.Revit.DB;

namespace CarbonEmissionTool.Models
{
    public static class UnitExtensions
    {
        /// <summary>
        /// Converts the input value into Revit internal units - decimal feet.
        /// </summary>
        public static double ToDecimalFeet(this double value, DisplayUnitType unitType = DisplayUnitType.DUT_MILLIMETERS)
        {
            return UnitUtils.ConvertToInternalUnits(value, unitType);
        }
    }
}
