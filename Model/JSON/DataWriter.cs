using CarbonEmissionTool.Model.Interfaces;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CarbonEmissionTool.Model.JSON
{
    class DataWriter
    {
        private static void WriteDictionaries(JsonWriter writer, Dictionary<string, bool> dataDictionary)
        {
            foreach (KeyValuePair<string, bool> keyValuePair in dataDictionary)
            {
                writer.WritePropertyName(keyValuePair.Key);
                writer.WriteValue(keyValuePair.Value);
            }
        }

        internal static void WriteJSON(IProjectDetails projectDetails, List<KeyValuePair<string, double>> eCData, string fullJSONFilePath, string date, string time)
        {
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);

            JsonWriter writer = new JsonTextWriter(sw);         
            writer.WriteStartObject();

            writer.WritePropertyName("Final");
            writer.WriteValue(projectDetails.ExportStatus.ToString());

            writer.WritePropertyName("Date");
            writer.WriteValue(date);

            writer.WritePropertyName("Time");
            writer.WriteValue(time);

            writer.WritePropertyName("Project_Name");
            writer.WriteValue(projectDetails.ProjectName);

            writer.WritePropertyName("Project_Version");
            writer.WriteValue(projectDetails.ProjectVersion);

            writer.WritePropertyName("Project_Address");
            writer.WriteValue(projectDetails.ProjectAddress);

            writer.WritePropertyName("Total_Floor_Area");
            writer.WriteValue(projectDetails.FloorArea);

            WriteDictionaries(writer, projectDetails.BuildElementsSelection);

            writer.WritePropertyName("RIBA_Workstage");
            writer.WriteValue(projectDetails.RibaWorkstage);

            writer.WritePropertyName("New_Build");
            writer.WriteValue(projectDetails.NewBuild);

            WriteDictionaries(writer, projectDetails.SectorSelection);

            foreach (KeyValuePair<string, double> keyValuePair in eCData)
            {
                writer.WritePropertyName(keyValuePair.Key);
                writer.WriteValue(keyValuePair.Value);
            }

            writer.WriteEndObject();
            writer.Close();
            sw.Close();

            //write string to file
            File.WriteAllText(fullJSONFilePath, sw.ToString());
        }
    }
}
