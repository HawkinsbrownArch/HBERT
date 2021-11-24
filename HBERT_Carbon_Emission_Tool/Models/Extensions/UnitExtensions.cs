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

            if (ApplicationServices.RevitVersion < ApplicationServices.RevitAPINewUnitsVersion)
                return value.ToDecimalFeetCompatibility();


            var forgeTypeId = new ForgeTypeId(UnitTypeId.Millimeters.TypeId);

            return UnitUtils.ConvertToInternalUnits(value, forgeTypeId);
        }




    }
}
