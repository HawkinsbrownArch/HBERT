using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;

namespace CarbonEmissionTool.Compatibility
{
    public static class UnitExtensionsCompatibility
    {
        /// <summary>
        /// Converts the input value into Revit internal units - decimal feet.
        /// </summary>
        public static double ToDecimalFeetCompatibility(this double value)
        {
            
            return UnitUtils.ConvertToInternalUnits(value, DisplayUnitType.DUT_MILLIMETERS);
        }
    }
}
