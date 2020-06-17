using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.DB;
using CarbonEmissionTool.Model.Utilities;
using CarbonEmissionTool.Services;
using View3D = Autodesk.Revit.DB.View3D;

namespace CarbonEmissionTool.Model.Collectors
{
    public class RevitViewFilter
    {
        /// <summary>
        /// Returns a dictionary of the 3D views in the active document.
        /// </summary>
        public static List<View3D> Get3DViews()
        {
            var document = ApplicationServices.Document;

            var viewSheet = new FilteredElementCollector(document).OfClass(typeof(ViewSheet)).WhereElementIsNotElementType().Cast<ViewSheet>().First();
            var threeDViews = new FilteredElementCollector(document).OfClass(typeof(View3D)).WhereElementIsNotElementType();

            var unplaced3Views = new List<View3D>();
            foreach (View3D view3D in threeDViews)
            {
                bool viewNotPlaced = Viewport.CanAddViewToSheet(document, viewSheet.Id, view3D.Id);

                //Only store views which have not been placed on sheets
                if (viewNotPlaced)
                    unplaced3Views.Add(view3D);
            }

            // If HBERT has already been run and the EC sheet exists, get the sheet and then
            // get the existing (placed) 3D view and add it to the list enabling the user to
            // select it if they re-run the tool.
            var existingECSheet = SheetUtils.GetECSheet();
            if(existingECSheet != null)
            {
                var placedViewIds = existingECSheet.GetAllPlacedViews().ToList();

                foreach (var viewId in placedViewIds)
                {
                    var currentView = document.GetElement(viewId) as View3D;
                    if (currentView.ViewType == ViewType.ThreeD)
                    {
                        unplaced3Views.Add(currentView);
                        break;
                    }
                }
            }

            return unplaced3Views;
        }
    }
}