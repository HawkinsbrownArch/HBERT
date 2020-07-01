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
        /// Gets the embodied carbon data from the EC schedule in Revit as a list of <see cref="CarbonData"/>.
        /// Only materials with names that start with the <see cref="ApplicationSettings.TextStyleNamePrefix"/>
        /// are included and calculated.
        /// </summary>
        public static List<CarbonData> Process()
        {
            var scheduleView = RevitScheduleFilter.TryGetCarbonSchedule();
            
            if (scheduleView == null)
            {
                WarningDialog.ScheduleNotFound();

                return new List<CarbonData>();
            }

            var embodiedCarbonList = CarbonDataProcessor.GetCarbonData(scheduleView);

            // Prepare the data for the charting. Values must be sorted descending (and positive, obviously).
            embodiedCarbonList.Sort((x, y) => y.EmbodiedCarbon.CompareTo(x.EmbodiedCarbon));

            return embodiedCarbonList;
        }

        /// <summary>
        /// Returns the <see cref="ScheduleDefinition"/> without any sorting or grand totals
        /// to avoid the potential of processing values in the schedule which are not embodied carbon
        /// values.
        /// </summary>
        private static ScheduleDefinition GetFormattedScheduleDefinition(ViewSchedule carbonSchedule)
        {
            ScheduleDefinition definition = carbonSchedule.Definition;
            // Remove all the sorting group fields to prevent fields like totals being mixed into the schedule data.
            definition.ClearSortGroupFields();
            definition.IsItemized = false;
            definition.ShowGrandTotal = false;

            return definition;
        }

        /// <summary>
        /// Gets the carbon data from the <see cref="ViewSchedule"/> as a list of <see cref="CarbonData"/>.
        /// </summary>
        private static List<CarbonData> GetCarbonData(ViewSchedule carbonSchedule)
        {
            var embodiedCarbonList = new List<CarbonData>();

            using (Transaction trasaction = new Transaction(ApplicationServices.Document, "Get schedule carbon data"))
            {
                // Start a sub transaction that can be rolled back so the carbonSchedule can be cleared of its sorting and groupings.
                trasaction.Start();

                ScheduleDefinition definition = CarbonDataProcessor.GetFormattedScheduleDefinition(carbonSchedule);

                int materialNameIndex = ScheduleUtils.GetScheduleColumnIndex(definition, definition.GetFieldCount(), ApplicationSettings.ScheduleMaterialColumnName, out var fieldId);
                int eCRatingIndex = ScheduleUtils.GetScheduleColumnIndex(definition, definition.GetFieldCount(), ApplicationSettings.ScheduleOverallEcColumnName, out _);

                // If the required fields are not found
                if (materialNameIndex == -1 | eCRatingIndex == -1)
                {
                    WarningDialog.NoScheduleColumnFound();

                    return embodiedCarbonList;
                }

                ScheduleSortGroupField sortGroupField = new ScheduleSortGroupField(fieldId);
                definition.InsertSortGroupField(sortGroupField, 0);

                TableSectionData scheduleTableData = carbonSchedule.GetTableData().GetSectionData(SectionType.Body);

                // Removes the first two rows from the table data: the header row and the blank row underneath
                // Revit automatically adds to leave only the table data
                bool headersOn = carbonSchedule.Definition.ShowHeaders;

                int startIndex = headersOn ? 1 : 0;
                
                int nRows = scheduleTableData.NumberOfRows;
                for (int row = startIndex; row < nRows; row++)
                {
                    string materialName = carbonSchedule.GetCellText(SectionType.Body, row, materialNameIndex);

                    // Only add the data from the schdule if its material name starts with HBA. Prevents rows such as the
                    // grand total or blank rows from being added to the dictionary.
                    if (materialName.StartsWith(ApplicationSettings.TextStyleNamePrefix))
                    {
                        double embodiedCarbon =
                            Convert.ToDouble(carbonSchedule.GetCellText(SectionType.Body, row, eCRatingIndex));

                        embodiedCarbonList.Add(new CarbonData(materialName, embodiedCarbon));
                    }
                }

                // If the schedule contains no data.
                if (embodiedCarbonList.Count == 0)
                {
                    WarningDialog.ScheduleContainsNoData();
                    return embodiedCarbonList;
                }

                // Post-processing of the CarbonData is necessary to combine small values into one summed
                // category to minimise the problem with Revit's short curve tolerance when the carbon data is used to
                // generate filled regions for the charts.
                double maxValue = embodiedCarbonList.Max(o => o.EmbodiedCarbon);

                var smallValues = new List<double>();
                var summedSmallValues = 0.0;
                foreach (var carbonData in embodiedCarbonList)
                {
                    double eCValue = carbonData.EmbodiedCarbon;
                    if (eCValue / maxValue < ApplicationSettings.SmallValueThreshold)
                    {
                        summedSmallValues += eCValue;

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

            return embodiedCarbonList;
        }
    }
}