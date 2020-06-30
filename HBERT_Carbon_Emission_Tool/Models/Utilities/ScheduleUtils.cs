using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Autodesk.Revit.DB;
using CarbonEmissionTool.Services;

namespace CarbonEmissionTool.Models
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

            var templateDocument = ApplicationServices.Application.OpenDocumentFile($"{filePath}\\HBERT_R{revitVersionNumber}.rvt");

            var viewSchedule = RevitScheduleFilter.GetCarbonSchedule();
            var elementsToCopy = new List<ElementId> { viewSchedule.Id }; //Get the carbon schedule id

            using (var transaction = new Transaction(doc, "Import carbon schedule"))
            {
                transaction.Start();

                ElementTransformUtils.CopyElements(templateDocument, elementsToCopy, doc, Transform.Identity, new CopyPasteOptions());
                
                doc.Regenerate();

                transaction.Commit();
            }

            templateDocument.Close(false);
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