using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;
using CarbonEmissionTool.Model.Dialogs;
using CarbonEmissionTool.Services;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using CarbonEmissionTool.Model.Collectors;

namespace CarbonEmissionTool.Model.Utilities
{
    public class ScheduleUtils
    {
        /// <summary>
        /// Imports the EC Schedule into the active document from the sample template.
        /// </summary>
        public static void ImportECScedule()
        {
            var doc = ApplicationServices.Document;

            //Returns the current version number of Revit as a string
            string revitVersionNumber = doc.Application.VersionNumber;

            string assemblyPath = Assembly.GetExecutingAssembly().Location;
            string filePath = new FileInfo(assemblyPath).Directory.FullName;

            Application app = doc.Application;

            Document templateDocument = app.OpenDocumentFile(filePath + @"\HBERT_R" + revitVersionNumber + ".rvt");
            
            List<ViewSchedule> viewSchedules = new FilteredElementCollector(templateDocument).OfClass(typeof(ViewSchedule)).WhereElementIsNotElementType().Cast<ViewSchedule>().ToList();

            ViewSchedule viewSchedule = RevitScheduleFilter.GetCarbonSchedule();
            List<ElementId> elementsToCopy = new List<ElementId> { viewSchedule.Id }; //Get the carbon schedule id
            
            ElementTransformUtils.CopyElements(templateDocument, elementsToCopy, doc, Transform.Identity, new CopyPasteOptions());

            templateDocument.Close(false);

            doc.Regenerate();
        }

        /// <summary>
        /// Returns the index of the column which matches the <paramref name="columnName"/>. If no match is found
        /// returns -1.
        /// </summary>
        public static int GetScheduleColumnIndex(ScheduleDefinition definition, int columnCount, string columnName, ref ScheduleFieldId fieldId)
        {
            int columnIndex = -1;
            for (int i = 0; i < columnCount; i++)
            {
                ScheduleField field = definition.GetField(i);
                if (field.GetName() == columnName)
                {
                    fieldId = field.FieldId;
                    columnIndex = i;
                    break;
                }
            }

            // Display a warning to the user if the column name cant be found in the schedule.
            if (columnIndex == -1)
                WarningDialog.NoScheduleColumnFound(columnName);

            return columnIndex;
        }
    }
}