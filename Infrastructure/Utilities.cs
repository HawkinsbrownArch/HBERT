using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Newtonsoft.Json;
using Autodesk.Revit.UI;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.UI.Events;
using System.Text.RegularExpressions;

namespace HBERT.Infrastructure
{
    class Utilities
    {

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

                int materialNameIndex = ExceptionUtils.GetScheduleColumnIndex(definition, definition.GetFieldCount(), "Material: Name", ref fieldId);
                int eCRatingIndex = ExceptionUtils.GetScheduleColumnIndex(definition, definition.GetFieldCount(), "Overall EC sum (kgCO2e)", ref fieldId2);

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

        internal static int ConvertColourToInt(int r, int g, int b)
        {
            return r + g * (int)Math.Pow(2, 8) + b * (int)Math.Pow(2, 16);
        }

        internal static void SetParameters(Element element, List<BuiltInParameter> parameterList, List<dynamic> values)
        {
            for(int p = 0; p < parameterList.Count; p++)
            {
                element.get_Parameter(parameterList[p]).Set(values[p]);
            }
        }


        #region <<<<----SHEET AND VIEW HELPER FUNCTIONS---->>>>
        //Validates file names by removing any illegal characters input by the user
        internal static string ValidateFileName(string projectName) //  #Uses a Regular Expression to clean the new view name of any illegal characters
        {
            Regex cleanExpression = new Regex(@"[][\:{}|;<>?`~]");

            var fileNameCleaned = cleanExpression.Replace(projectName, ""); //The new view name cleaned

            return fileNameCleaned;
        }

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

        //Used to collect either 3D views or ViewSchedules from the active document
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

        internal static ElementId GetDraftingViewTypeId(Document doc)
        {
            List<ViewFamilyType> viewTypes = new FilteredElementCollector(doc).OfClass(typeof(ViewFamilyType)).Cast<ViewFamilyType>().ToList();

            ViewFamilyType draftingType = null;
            for (int i = 0; i < viewTypes.Count; i++)
            {
                if (viewTypes[i].ViewFamily == ViewFamily.Drafting)
                {
                    draftingType = viewTypes[i];
                    break;
                }
            }

            return draftingType.Id;
        }

        internal static bool DeleteOldECSheet(Document doc, ViewSheet viewSheet)
        {
            if (viewSheet != null && viewSheet.Id == doc.ActiveView.Id)
            {
                return true;
            }
            else
            { 
                try
                {
                    doc.Delete(viewSheet.Id);
                }
                catch
                {

                }
            }

            return false;
        }

        internal static ViewSheet GetOldECSheet(Document doc, string name, string number)
        {
            List<ViewSheet> sheets = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_Sheets).Cast<ViewSheet>().ToList();

            ViewSheet viewSheet = sheets.Find(s => s.Name == name & s.SheetNumber == number);

            return viewSheet;
        }

        internal static void SetViewportType(Document doc, Viewport viewport)
        {
            List<ElementType> elementTypes = new FilteredElementCollector(doc).OfClass(typeof(ElementType)).WhereElementIsElementType().Cast<ElementType>().ToList();

            for (int i = 0; i < elementTypes.Count; i++)
            {
                if (elementTypes[i].Name == "No Title")
                {
                    Parameter param = viewport.get_Parameter(BuiltInParameter.ELEM_TYPE_PARAM);
                    param.Set(elementTypes[i].Id);
                    break;
                }
            }
        }
        #endregion ----SHEET HELPER FUNCTIONS----
    }
}