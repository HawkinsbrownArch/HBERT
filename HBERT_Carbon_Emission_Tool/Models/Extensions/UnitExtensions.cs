using Autodesk.Revit.DB;
using CarbonEmissionTool.Compatibility;
using CarbonEmissionTool.Services;

namespace CarbonEmissionTool.Models
{
    public static class UnitExtensions
    {
        /// <summary>
        /// Converts the input value into Revit internal units - decimal feet.
        /// </summary>
        public static double ToDecimalFeet(this double value)
        {
            if (ApplicationServices.RevitVersionNumber < ApplicationServices.RevitAPINewUnitsVersion)
            {
                return value.ToDecimalFeetCompatibility();
            }

            return ToForgeDecimalFeet(value);
        }


        /// <summary>
        /// Convert a value into decimal feet using the updated Forge Type ID unit system
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static double ToForgeDecimalFeet(double value)
        {
            var forgeTypeId = new ForgeTypeId(UnitTypeId.Millimeters.TypeId);

            return UnitUtils.ConvertToInternalUnits(value, forgeTypeId);
        }
    }
}
