using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.DB;
using CarbonEmissionTool.Model.Utilities;

namespace CarbonEmissionTool.Model.Collectors
{
    class RevitViewFilter
    {
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

            ViewSheet oldECSheet = SheetUtils.GetOldECSheet(doc, sheetName, sheetNumber);
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