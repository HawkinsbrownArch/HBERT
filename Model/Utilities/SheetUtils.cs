using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.DB;
using CarbonEmissionTool.Services;

namespace CarbonEmissionTool.Model.Utilities
{
    class SheetUtils
    {
        /// <summary>
        /// Returns true if HBERT has been run and generated a <see cref="ViewSheet"/> and the user
        /// has this sheet as the active view in the Revit document.
        /// </summary>
        internal static bool ExistingECSheetActive(Document doc, ViewSheet viewSheet)
        {
            if (viewSheet != null && viewSheet.Id == doc.ActiveView.Id)
                return true;

            doc.Delete(viewSheet.Id);

            return false;
        }

        /// <summary>
        /// Returns the <see cref="ViewSheet"/> created by HBERT if it has been previously run.
        /// </summary>
        internal static ViewSheet GetOldECSheet()
        {
            List<ViewSheet> sheets = new FilteredElementCollector(ApplicationServices.Document).OfCategory(BuiltInCategory.OST_Sheets).Cast<ViewSheet>().ToList();

            ViewSheet viewSheet = sheets.Find(s => s.Name == ApplicationServices.SheetName & s.SheetNumber == ApplicationServices.SheetNumber);

            return viewSheet;
        }
    }
}