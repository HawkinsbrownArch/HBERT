using Autodesk.Revit.DB;

namespace CarbonEmissionTool.Model.Extensions
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

        /// <summary>
        /// Converts the input value in decimal feet to millimeters.
        /// </summary>
        public static double ToMillimeters(this double valueDecimalFeet)
        {
            return UnitUtils.ConvertFromInternalUnits(valueDecimalFeet, DisplayUnitType.DUT_MILLIMETERS);
        }
    }
}
