using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.DB;

namespace CarbonEmissionTool.Model.Utilities
{
    class SheetUtils
    {
        internal static bool DeleteOldECSheet(Document doc, ViewSheet viewSheet)
        {
            if (viewSheet != null && viewSheet.Id == doc.ActiveView.Id)
            {
                return true;
            }
            else
            { 
                try
                {
                    doc.Delete(viewSheet.Id);
                }
                catch
                {

                }
            }

            return false;
        }

        internal static ViewSheet GetOldECSheet(Document doc, string name, string number)
        {
            List<ViewSheet> sheets = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_Sheets).Cast<ViewSheet>().ToList();

            ViewSheet viewSheet = sheets.Find(s => s.Name == name & s.SheetNumber == number);

            return viewSheet;
        }
    }
}