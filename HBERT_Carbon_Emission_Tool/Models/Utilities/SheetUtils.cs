using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.DB;
using CarbonEmissionTool.Services;
using CarbonEmissionTool.Settings;

namespace CarbonEmissionTool.Model.Utilities
{
    public class SheetUtils
    {
        /// <summary>
        /// Returns true if HBERT has been run and generated a <see cref="ViewSheet"/> and the user
        /// has this sheet as the active view in the Revit document.
        /// </summary>
        public static bool ECSheetActive(ViewSheet viewSheet)
        {
            if (viewSheet != null && viewSheet.Id == ApplicationServices.Document.ActiveView.Id)
                return true;

            return false;
        }

        /// <summary>
        /// Deletes the existing EC Sheet if it already exists in the document.
        /// </summary>
        public static bool DeleteECSheet(ViewSheet viewSheet)
        {
            try
            {
                ApplicationServices.Document.Delete(viewSheet.Id);
            }
            catch
            {

            }

            return false;
        }

        /// <summary>
        /// Creates a new EC Sheet to place the views and charts. If the user has run the tool before and
        /// created a EC sheet, this method locates and deletes it if the user has not renamed the sheet
        /// name and number. 
        /// </summary>
        public static ViewSheet CreateECSheet(FamilySymbol titleBlock)
        {
            var existingECSheet = SheetUtils.GetECSheet();

            if (existingECSheet != null)
                SheetUtils.DeleteECSheet(existingECSheet);

            var doc = ApplicationServices.Document;

            //Create the new sheet to present the data 
            ViewSheet newSheet = ViewSheet.Create(doc, titleBlock.Id);

            newSheet.Name = ApplicationSettings.SheetName;
            newSheet.SheetNumber = ApplicationSettings.SheetNumber;

            return newSheet;
        }

        /// <summary>
        /// Returns the <see cref="ViewSheet"/> created by HBERT if it has been previously run. If not found
        /// returns null.
        /// </summary>
        public static ViewSheet GetECSheet()
        {
            var sheets = new FilteredElementCollector(ApplicationServices.Document).OfCategory(BuiltInCategory.OST_Sheets).WhereElementIsNotElementType();

            foreach (ViewSheet sheet in sheets)
            {
                if (sheet.Name == ApplicationSettings.SheetName & sheet.SheetNumber == ApplicationSettings.SheetNumber)
                {
                    return sheet;
                }
            }

            return null;
        }
    }
}