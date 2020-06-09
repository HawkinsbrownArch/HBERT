using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.DB;
using CarbonEmissionTool.Model.Utilities;
using CarbonEmissionTool.Services;

namespace CarbonEmissionTool.Model.Collectors
{
    class RevitViewFilter
    {
        /// <summary>
        /// Returns a dictionary of the 3D views in the 
        /// </summary>
        public static Dictionary<string, View3D> Get3DViews()
        {
            var document = ApplicationServices.Document;

            ViewSheet viewSheet = new FilteredElementCollector(document).OfClass(typeof(ViewSheet)).WhereElementIsNotElementType().Cast<ViewSheet>().First();
            List<View3D> view3D = new FilteredElementCollector(document).OfClass(typeof(View3D)).WhereElementIsNotElementType().Cast<View3D>().ToList();

            Dictionary<string, View3D> elementDict = new Dictionary<string, View3D>();
            for(int i =0; i < view3D.Count; i++)
            {
                View3D currentView = view3D[i];
                
                bool viewNotPlaced = Viewport.CanAddViewToSheet(document, viewSheet.Id, currentView.Id);

                //Only store views which have not been placed on sheets
                if (viewNotPlaced)
                    elementDict[currentView.Name] = currentView;
            }

            ViewSheet oldECSheet = SheetUtils.GetOldECSheet();
            if(oldECSheet != null)
            {
                List<ElementId> placedViewIds = oldECSheet.GetAllPlacedViews().ToList();

                for (int i = 0; i < placedViewIds.Count; i++)
                {
                    Autodesk.Revit.DB.View currentView = document.GetElement(placedViewIds[i]) as Autodesk.Revit.DB.View;
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