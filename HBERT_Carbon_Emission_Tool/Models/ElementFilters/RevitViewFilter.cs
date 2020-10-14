using Autodesk.Revit.DB;
using CarbonEmissionTool.Services;
using System.Collections.Generic;
using View3D = Autodesk.Revit.DB.View3D;

namespace CarbonEmissionTool.Models
{
    public class RevitViewFilter
    {
        /// <summary>
        /// Returns a list of the 3D views in the active document.
        /// </summary>
        public static List<View3D> Get3DViews()
        {
            var document = ApplicationServices.Document;

            var threeDViews = new FilteredElementCollector(document).OfClass(typeof(View3D)).WhereElementIsNotElementType();

            var unplaced3Views = new List<View3D>();
            using (var tempTransaction = new Transaction(document, "Validate 3d view placement on temp sheet"))
            {
                tempTransaction.Start();
                
                var tempSheet = ViewSheet.Create(document, ElementId.InvalidElementId);

                var tempSheetId = tempSheet.Id;

                foreach (View3D view3D in threeDViews)
                {
                    var viewId = view3D.Id;

                    bool canAddViewToSheet = Viewport.CanAddViewToSheet(document, tempSheetId, viewId);

                    // Only store views which have not been placed on sheets
                    // If the view is empty it cant be placed on the sheet. Only way to check is create a viewport.
                    // If its null the view is empty and cant be placed. CanAddViewToSheet doesn't check this.
                    if (canAddViewToSheet && Viewport.Create(document, tempSheetId, viewId, new XYZ()) != null)
                        unplaced3Views.Add(view3D);
                }

                tempTransaction.RollBack();
            }
            
            return unplaced3Views;
        }
    }
}