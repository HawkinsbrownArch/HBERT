using CarbonEmissionTool.Model.Collections;
using CarbonEmissionTool.Model.Interfaces;
using Newtonsoft.Json;
using System.IO;
using System.Text;

namespace CarbonEmissionTool.Model.JSONManager
{
    public class DataWriter
    {
        /// <summary>
        /// Writes the user selection from the <paramref name="itemCollection"/> to the writer.
        /// </summary>
        private static void WriteCollection(JsonWriter writer, SelectedItemCollection itemCollection)
        {
            foreach (var item in itemCollection)
            {
                writer.WritePropertyName(item.Name);
                writer.WriteValue(item.IsSelected);
            }
        }

        /// <summary>
        /// Writes the JSON file capturing all the data input by the user in the UI window.
        /// </summary>
        public static void WriteJSON(IProjectDetails projectDetails, string jsonFilePath, string date, string time)
        {
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);

            JsonWriter writer = new JsonTextWriter(sw);         
            writer.WriteStartObject();

            writer.WritePropertyName("Date");
            writer.WriteValue(date);

            writer.WritePropertyName("Time");
            writer.WriteValue(time);

            writer.WritePropertyName("Project_Name");
            writer.WriteValue(projectDetails.Name);

            writer.WritePropertyName("Project_Version");
            writer.WriteValue(projectDetails.Version);

            writer.WritePropertyName("Project_Address");
            writer.WriteValue(projectDetails.Address);

            writer.WritePropertyName("Total_Floor_Area");
            writer.WriteValue(projectDetails.FloorArea);

            WriteCollection(writer, projectDetails.BuildElements);

            writer.WritePropertyName("RIBA_Workstage");
            writer.WriteValue(projectDetails.RibaWorkstage);

            writer.WritePropertyName("New_Build");
            writer.WriteValue(projectDetails.ProjectType);

            WriteCollection(writer, projectDetails.Sectors);

            foreach (var carbonData in projectDetails.CarbonDataCache)
            {
                writer.WritePropertyName(carbonData.MaterialName);
                writer.WriteValue(carbonData.EmbodiedCarbon);
            }

            writer.WriteEndObject();
            writer.Close();
            sw.Close();

            //write string to file
            File.WriteAllText(jsonFilePath, sw.ToString());
        }
    }
}
