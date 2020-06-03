using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;
using CarbonEmissionTool.Model.Dialogs;

namespace CarbonEmissionTool.Model.Utilities
{
    class ScheduleUtils
    {
        internal static void ImportECScedule(Document doc)
        {
            //Returns the current version number of Revit as a string
            string revitVersionNumber = doc.Application.VersionNumber;

            string assemblyPath = Assembly.GetExecutingAssembly().Location;
            string filePath = new FileInfo(assemblyPath).Directory.FullName;

            Application app = doc.Application;

            Document docOnDisk = app.OpenDocumentFile(filePath + @"\HBERT_R" + revitVersionNumber + ".rvt");
            
            List<ViewSchedule> viewSchedules = new FilteredElementCollector(docOnDisk).OfClass(typeof(ViewSchedule)).WhereElementIsNotElementType().Cast<ViewSchedule>().ToList();

            ViewSchedule viewSchedule = GetCarbonSchedule(docOnDisk);
            List<ElementId> elementsToCopy = new List<ElementId> { viewSchedule.Id }; //Get the carbon schedule id
            
            ElementTransformUtils.CopyElements(docOnDisk, elementsToCopy, doc, Transform.Identity, new CopyPasteOptions());

            docOnDisk.Close(false);

            doc.Regenerate();
        }

        internal static ViewSchedule GetCarbonSchedule(Document doc)
        {
            List<ViewSchedule> viewSchedules = new FilteredElementCollector(doc).OfClass(typeof(ViewSchedule)).WhereElementIsNotElementType().Cast<ViewSchedule>().ToList();

            ViewSchedule viewSchedule = null;
            for (int i = 0; i < viewSchedules.Count; i++)
            {
                if (viewSchedules[i].Definition.IsMaterialTakeoff & viewSchedules[i].Name == "Embodied Carbon (Do Not Delete)")
                {
                    viewSchedule = viewSchedules[i];
                    break;
                }
            }

            return viewSchedule;
        }

        //Gets the embodied cardbon data from the EC schedule in revit as a dictionary with the material name as the key and the EC value as the value
        internal static List<KeyValuePair<string, double>> GetScheduleData(Document doc, ViewSchedule scheduleView, double smallValueThreshold)
        {
            var embodiedCarbonDictionary = new List<KeyValuePair<string, double>>();
            using (SubTransaction subTrasaction = new SubTransaction(doc))
            {
                //Start a sub transaction that can be rolled back so the scheduleView can be cleared of its sorting and groupings
                subTrasaction.Start();

                ScheduleDefinition definition = scheduleView.Definition;
                definition.ClearSortGroupFields(); //Remove all the sorting group fields to prevent fields like totals being mixed into the schedule data
                definition.IsItemized = false;
                definition.ShowGrandTotal = false;

                ScheduleFieldId fieldId = null;
                ScheduleFieldId fieldId2 = null;

                int materialNameIndex = TaskDialog.GetScheduleColumnIndex(definition, definition.GetFieldCount(), "Material: Name", ref fieldId);
                int eCRatingIndex = TaskDialog.GetScheduleColumnIndex(definition, definition.GetFieldCount(), "Overall EC sum (kgCO2e)", ref fieldId2);

                ScheduleSortGroupField sortGroupField = new ScheduleSortGroupField(fieldId);
                definition.InsertSortGroupField(sortGroupField, 0);

                TableSectionData scheduleTableData = scheduleView.GetTableData().GetSectionData(SectionType.Body);

                //Removes the first two rows from the table data: the header row and the blank row underneath Revit automatically adds to leave only the table data
                bool headersOn = scheduleView.Definition.ShowHeaders;

                int startIndex = headersOn ? 2 : 0;

                //If the required fields are not found
                if (materialNameIndex == -1 | eCRatingIndex == -1)
                    return null;

                int nRows = scheduleTableData.NumberOfRows;
                int nColumns = scheduleTableData.NumberOfColumns;
                for (int row = startIndex; row < nRows; row++)
                {
                    string materialName = scheduleView.GetCellText(SectionType.Body, row, materialNameIndex);

                    //Only add the data from the schdule if its material name starts with HBA. Prevents rows such as the grand total or blank rows from being added to the dictionary.
                    //if(materialName.StartsWith("HBA"))
                    //{
                    double embodiedCarbon = Convert.ToDouble(scheduleView.GetCellText(SectionType.Body, row, eCRatingIndex));

                    if (embodiedCarbon > 0.1)
                        embodiedCarbonDictionary.Add(new KeyValuePair<string, double>(materialName, embodiedCarbon));
                    //}
                }

                //If the schedule contains no data then throw an exception
                if (embodiedCarbonDictionary.Count == 0)
                    return null;

                //Post-process the data and combine any EC ratings less than 2.5%of the largest value into a combined value key pair
                double maxValue = embodiedCarbonDictionary.Max(o => o.Value);
                List<double> smallValues = new List<double>(); //A list which stores all the smallest values to enable identification of the key value pairs to remove from the embodiedCarbonDictionary
                double summedSmallValues = 0.0;
                foreach (KeyValuePair<string, double> valuePair in embodiedCarbonDictionary)
                {
                    double eCValue = valuePair.Value;
                    if (eCValue / maxValue < smallValueThreshold)
                    {
                        summedSmallValues = +eCValue; //Add the small value to the summedSmallValues
                        smallValues.Add(eCValue);
                    }
                }

                //Remove any small values and combine into one 'other' category
                if (smallValues.Count > 0)
                {
                    double smallValueLimit = smallValues.Max(); //Get the largest value from the smallValueLimit
                    embodiedCarbonDictionary.RemoveAll(o => o.Value <= smallValueLimit); //Remove all the small values from the embodiedCarbonDictionary

                    embodiedCarbonDictionary.Add(new KeyValuePair<string, double>("Existing", summedSmallValues));
                }

                subTrasaction.RollBack();
            }
            return embodiedCarbonDictionary;
        }
    }
}