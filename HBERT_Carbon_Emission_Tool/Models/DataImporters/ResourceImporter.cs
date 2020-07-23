using Autodesk.Revit.DB;
using CarbonEmissionTool.Services;
using CarbonEmissionTool.Settings;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace CarbonEmissionTool.Models
{
    class ResourceImporter
    {
        /// <summary>
        /// Imports the EC Schedule, library and Hawkins Brown title block into the active document from the sample template.
        /// </summary>
        public static void GetResources()
        {
            var doc = ApplicationServices.Document;

            //Returns the current version number of Revit as a string
            string revitVersionNumber = doc.Application.VersionNumber;

            string assemblyPath = Assembly.GetExecutingAssembly().Location;
            string filePath = new FileInfo(assemblyPath).Directory.FullName;

            var templateDocument = ApplicationServices.Application.OpenDocumentFile($"{filePath}\\Revit Templates\\HBERT_R{revitVersionNumber}.rvt");

            var carbonSchedule = RevitScheduleFilter.GetScheduleByName(templateDocument, ApplicationSettings.EmbodiedCarbonScheduleName);
            var librarySchedule = RevitScheduleFilter.GetScheduleByName(templateDocument, ApplicationSettings.EmbodiedCarbonLibraryName);

            var titleBlock = TitleBlockFilter.GetAll(templateDocument).Find(t => t.Name == ApplicationSettings.TitleBlockName);

            var elementsToCopy = new List<ElementId> { carbonSchedule.Id, librarySchedule.Id?? ElementId.InvalidElementId, titleBlock.Id ?? ElementId.InvalidElementId };

            using (var transaction = new Transaction(doc, "Import carbon schedules and title block"))
            {
                transaction.Start();

                ElementTransformUtils.CopyElements(templateDocument, elementsToCopy, doc, Transform.Identity, new CopyPasteOptions());

                doc.Regenerate();

                transaction.Commit();
            }

            templateDocument.Close(false);
        }
    }
}
