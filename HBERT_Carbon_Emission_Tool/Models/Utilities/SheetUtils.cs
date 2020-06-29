using Autodesk.Revit.DB;
using CarbonEmissionTool.Services;

namespace CarbonEmissionTool.Models
{
    public class SheetUtils
    {
        /// <summary>
        /// Creates a new EC Sheet to place the views and charts. If the user has run the tool before and
        /// created a EC sheet, this method locates and deletes it if the user has not renamed the sheet
        /// name and number. 
        /// </summary>
        public static ViewSheet CreateECSheet(IPublishDetails publishDetails)
        {
            var doc = ApplicationServices.Document;

            //Create the new sheet to present the data 
            ViewSheet newSheet = ViewSheet.Create(doc, publishDetails.TitleBlock.Id);

            newSheet.Name = publishDetails.SheetName;
            newSheet.SheetNumber = publishDetails.SheetNumber;

            return newSheet;
        }

        /// <summary>
        /// Returns true if a sheet in the document has the same number.
        /// </summary>
        public static bool Exists(string sheetNumber)
        {
            var sheetCollector = new FilteredElementCollector(ApplicationServices.Document)
                .OfCategory(BuiltInCategory.OST_Sheets).WhereElementIsNotElementType();

            foreach (ViewSheet sheet in sheetCollector)
            {
                if (sheet.SheetNumber == sheetNumber)
                    return true;
            }

            return false;
        }
    }
}