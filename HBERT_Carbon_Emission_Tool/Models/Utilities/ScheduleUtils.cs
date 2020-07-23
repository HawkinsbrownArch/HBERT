using Autodesk.Revit.DB;

namespace CarbonEmissionTool.Models
{
    public class ScheduleUtils
    {
        /// <summary>
        /// Returns the index of the column which matches the <paramref name="columnName"/>. If no match is found
        /// returns -1.
        /// </summary>
        public static int GetScheduleColumnIndex(ScheduleDefinition definition, int columnCount, string columnName, out ScheduleFieldId fieldId)
        {
            fieldId = ScheduleFieldId.InvalidScheduleFieldId;

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