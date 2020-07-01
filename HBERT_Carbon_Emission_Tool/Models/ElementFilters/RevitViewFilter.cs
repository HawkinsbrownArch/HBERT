﻿using Autodesk.Revit.DB;
using CarbonEmissionTool.Services;
using System.Collections.Generic;
using System.Linq;
using View3D = Autodesk.Revit.DB.View3D;

namespace CarbonEmissionTool.Models
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
            
            return unplaced3Views;
        }
    }
}