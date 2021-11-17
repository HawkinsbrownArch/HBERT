using System.Windows.Controls;
using Autodesk.Revit.DB;

namespace CarbonEmissionTool.Models
{
    public static class UnitExtensions
    {

#if USE_FORGETYPEID
        //var forgeTypeId = new ForgeTypeId(UnitTypeId.SquareMeters.TypeId);
        //areaMetric = UnitUtils.ConvertFromInternalUnits(areaInternal, forgeTypeId);

        /// <summary>
        /// Converts the input value into Revit internal units - decimal feet.
        /// </summary>
        public static double ToDecimalFeet(this double value)
        {
            var forgeTypeId = new ForgeTypeId(UnitTypeId.Millimeters.TypeId);

            return UnitUtils.ConvertToInternalUnits(value, forgeTypeId);
        }
#else

        /// <summary>
        /// Converts the input value into Revit internal units - decimal feet.
        /// </summary>
        //public static double ToDecimalFeet(this double value, DisplayUnitType unitType = DisplayUnitType.DUT_MILLIMETERS)
        //{
        //    return UnitUtils.ConvertToInternalUnits(value, unitType);
        //}
#endif

    }
}
