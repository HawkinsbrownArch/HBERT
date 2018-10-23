using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using HBERT.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBERT_UI
{
    class HBFormUtils
    {
        /// <summary>
        /// Enum for the status of the export
        /// </summary>
        internal enum CarbonExportStatus
        {
            None,
            Final,
            Draft
        }

        /// <summary>
        /// Concatenates the sector types into a comma delineated string for representation on a Revit sheet
        /// </summary>
        internal static string GenerateSectorString(string[] sectorList)
        {
            string sector = "";
            for (int i = 0; i < sectorList.Length; i++)
            {
                string currentSector = sectorList[i];

                if (currentSector != "")
                    sector += currentSector + ", ";
            }

            if (sector.EndsWith(", "))
                sector = sector.Remove(sector.Length - 2, 2);

            return sector;
        }

        internal static Dictionary<string, FamilySymbol> GetTitleBlocks(Document doc)
        {
            List<FamilySymbol> titleBlocks = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_TitleBlocks).WhereElementIsElementType().Cast<FamilySymbol>().ToList();

            Dictionary<string, FamilySymbol> titleBlockDict = new Dictionary<string, FamilySymbol>();
            for (int i = 0; i < titleBlocks.Count; i++)
            {
                titleBlockDict[titleBlocks[i].Name] = titleBlocks[i];
            }

            return titleBlockDict;
        }

        //Used to collect 3D views from the active document
        internal static Dictionary<string, View3D> Get3DViews(Document doc, string sheetNumber, string sheetName)
        {
            ViewSheet viewSheet = new FilteredElementCollector(doc).OfClass(typeof(ViewSheet)).WhereElementIsNotElementType().Cast<ViewSheet>().First();
            List<View3D> view3D = new FilteredElementCollector(doc).OfClass(typeof(View3D)).WhereElementIsNotElementType().Cast<View3D>().ToList();

            Dictionary<string, View3D> elementDict = new Dictionary<string, View3D>();
            for(int i =0; i < view3D.Count; i++)
            {
                View3D currentView = view3D[i];
                
                bool viewNotPlaced = Viewport.CanAddViewToSheet(doc, viewSheet.Id, currentView.Id);

                //Only store views which have not been placed on sheets
                if (viewNotPlaced)
                    elementDict[currentView.Name] = currentView;
            }

            ViewSheet oldECSheet = Utilities.GetOldECSheet(doc, sheetName, sheetNumber);
            if(oldECSheet != null)
            {
                List<ElementId> placedViewIds = oldECSheet.GetAllPlacedViews().ToList();

                for (int i = 0; i < placedViewIds.Count; i++)
                {
                    View currentView = doc.GetElement(placedViewIds[i]) as View;
                    if (currentView.ViewType == ViewType.ThreeD)
                    {
                        elementDict[currentView.Name] = (View3D)currentView;
                    }
                }
            }

            return elementDict;
        }
    }
}
