using System;
using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.DB;
using CarbonEmissionTool.Services;
using CarbonEmissionTool.Settings;

namespace CarbonEmissionTool.Models
{
    public class CarbonDataProcessor
    {
        /// <summary>
        /// Gets the embodied carbon data from the EC schedule in revit as a dictionary
        /// with the material name as the key and the EC value as the value.
        /// </summary>
        public static List<CarbonData> Process()
        {
            var scheduleView = RevitScheduleFilter.TryGetCarbonSchedule();

            var embodiedCarbonList = new List<CarbonData>();

            if (scheduleView == null)
            {
                WarningDialog.ScheduleNotFound();

                return embodiedCarbonList;
            }
            
            using (Transaction trasaction = new Transaction(ApplicationServices.Document, "Get schedule carbon data"))
            {
                // Start a sub transaction that can be rolled back so the scheduleView can be cleared of its sorting and groupings.
                trasaction.Start();

                ScheduleDefinition definition = scheduleView.Definition;
                // Remove all the sorting group fields to prevent fields like totals being mixed into the schedule data.
                definition.ClearSortGroupFields(); 
                definition.IsItemized = false;
                definition.ShowGrandTotal = false;

                ScheduleFieldId fieldId = null;
                ScheduleFieldId fieldId2 = null;

                int materialNameIndex = ScheduleUtils.GetScheduleColumnIndex(definition, definition.GetFieldCount(), ApplicationSettings.ScheduleMaterialColumnName, ref fieldId);
                int eCRatingIndex = ScheduleUtils.GetScheduleColumnIndex(definition, definition.GetFieldCount(), ApplicationSettings.ScheduleOverallEcColumnName, ref fieldId2);

                ScheduleSortGroupField sortGroupField = new ScheduleSortGroupField(fieldId);
                definition.InsertSortGroupField(sortGroupField, 0);

                TableSectionData scheduleTableData = scheduleView.GetTableData().GetSectionData(SectionType.Body);

                // Removes the first two rows from the table data: the header row and the blank row underneath
                // Revit automatically adds to leave only the table data
                bool headersOn = scheduleView.Definition.ShowHeaders;

                int startIndex = headersOn ? 2 : 0;

                // If the required fields are not found
                if (materialNameIndex == -1 | eCRatingIndex == -1)
                {
                    WarningDialog.NoScheduleColumnFound();

                    return embodiedCarbonList;
                }

                int nRows = scheduleTableData.NumberOfRows;
                for (int row = startIndex; row < nRows; row++)
                {
                    string materialName = scheduleView.GetCellText(SectionType.Body, row, materialNameIndex);

                    // Only add the data from the schdule if its material name starts with HBA. Prevents rows such as the
                    // grand total or blank rows from being added to the dictionary.
                    if (materialName.StartsWith("HBA"))
                    {
                        double embodiedCarbon =
                            Convert.ToDouble(scheduleView.GetCellText(SectionType.Body, row, eCRatingIndex));

                        if (embodiedCarbon > 0.1)
                            embodiedCarbonList.Add(new CarbonData(materialName, embodiedCarbon));
                    }
                }

                // If the schedule contains no data.
                if (embodiedCarbonList.Count == 0)
                {
                    WarningDialog.ScheduleContainsNoData();
                    return embodiedCarbonList;
                }

                // Post-process the data and combine any EC ratings less than 2.5%of the largest value into a combined value.
                double maxValue = embodiedCarbonList.Max(o => o.EmbodiedCarbon);

                // A list which stores all the smallest values to enable identification of the key value pairs to remove
                // from the embodiedCarbonDictionary
                var smallValues = new List<double>(); 
                var summedSmallValues = 0.0;
                foreach (var carbonData in embodiedCarbonList)
                {
                    double eCValue = carbonData.EmbodiedCarbon;
                    if (eCValue / maxValue < ApplicationSettings.SmallValueThreshold)
                    {
                        summedSmallValues = +eCValue; //Add the small value to the summedSmallValues
                        smallValues.Add(eCValue);
                    }
                }

                // Remove any small values and combine into one 'other' category
                if (smallValues.Count > 0)
                {
                    // Get the largest value from the smallValueLimit.
                    double smallValueLimit = smallValues.Max(); // Get the largest value from the smallValueLimit

                    // Remove all the small values from the embodiedCarbonDictionary.
                    embodiedCarbonList.RemoveAll(o => o.EmbodiedCarbon <= smallValueLimit);

                    embodiedCarbonList.Add(new CarbonData(ApplicationSettings.CarbonDataSummedName, summedSmallValues));
                }

                trasaction.RollBack();
            }

            // Prepare the data for the charting. Values must be sorted descending (and positive, obviously).
            embodiedCarbonList.Sort((x, y) => y.EmbodiedCarbon.CompareTo(x.EmbodiedCarbon));

            return embodiedCarbonList;
        }
    }
}