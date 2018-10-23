using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using Google.Apis.Auth.OAuth2; // Google.Apis.Auth --version 1.30.0
using Google.Cloud.Storage.V1;
using HBERT.Infrastructure;

namespace HBERT_UI.Infrastructure
{
    [Obfuscation(Feature = "Apply to type MyNamespace.ResourceClass1: renaming", Exclude = true, ApplyToMembers = false)]
    class JSONColor
    {
        internal static string thisAssemblyPath = Assembly.GetExecutingAssembly().Location;
        internal static dynamic JsonArray { get; set; }

        public static void LoadJson()
        {
            string jsonConfigFile = Path.GetDirectoryName(thisAssemblyPath) + @"\HB_Material_ColourScheme.json";
            using (StreamReader r = new StreamReader(jsonConfigFile))
            {
                string jsonFileContents = r.ReadToEnd();
                JsonArray = JsonConvert.DeserializeObject(jsonFileContents);
            }
        }
    }

    [Obfuscation(Feature = "Apply to type MyNamespace.ResourceClass1: renaming", Exclude = true, ApplyToMembers = false)]
    class JSONAuth
    {
        internal static string thisAssemblyPath = Assembly.GetExecutingAssembly().Location;
        internal static string JsonArray { get; set; }

        public static string LoadJson()
        {
            string jsonConfigFile = Path.GetDirectoryName(thisAssemblyPath) + @"\hbert-json-storage-fc1413b06760.json";
            using (StreamReader r = new StreamReader(jsonConfigFile))
            {
                string jsonFileContents = r.ReadToEnd();
                JsonArray = JsonConvert.DeserializeObject(jsonFileContents).ToString();
                return JsonArray;
            }
        }
    }

    [Obfuscation(Feature = "Apply to type MyNamespace.ResourceClass1: renaming", Exclude = true, ApplyToMembers = false)]
    class DataController
    {
        private static void WriteDictionaries(JsonWriter writer, Dictionary<string, bool> dataDictionary)
        {
            foreach (KeyValuePair<string, bool> keyValuePair in dataDictionary)
            {
                writer.WritePropertyName(keyValuePair.Key);
                writer.WriteValue(keyValuePair.Value);
            }
        }

        internal static void WriteJSON(ProjectDetails projectDetails, List<KeyValuePair<string, double>> eCData, string fullJSONFilePath, string date, string time)
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

    [Obfuscation(Feature = "Apply to type MyNamespace.ResourceClass1: renaming", Exclude = true, ApplyToMembers = false)]
    static class GoogleCloud
    {
        internal static void UploadToCloud(ProjectDetails projectDetails, List<KeyValuePair<string, double>> eCData)
        {
            DateTime dateTime = DateTime.Now;
            string date = dateTime.Year + "-" + dateTime.Month + "-" + dateTime.Day;
            string time = dateTime.TimeOfDay.ToString();

            string thisAssemblyPath = Assembly.GetExecutingAssembly().Location;
            string jsonFolderLocation = Path.GetDirectoryName(thisAssemblyPath);
            string jsonName = Utilities.ValidateFileName(projectDetails.ProjectName) + " " + date + "_" + dateTime.Hour + "-" + dateTime.Minute + ".json";

            string jsonFilePath = jsonFolderLocation + @"\" + jsonName;

            //Create a new JSON data file
            DataController.WriteJSON(projectDetails, eCData, jsonFilePath, date, time);

            string bucketName = projectDetails.ExportStatus == HBFormUtils.CarbonExportStatus.Final ? @"hbert-json-storage.appspot.com" : @"hbert-json-draft-storage";

            GoogleCredential credential = null;
            try
            {
                credential = GoogleCredential.FromJson(JSONAuth.LoadJson());
                var storageClient = StorageClient.Create(credential);

                using (var fileStream = new FileStream(jsonFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    storageClient.UploadObject(bucketName, jsonName, @"application /json", fileStream);
                }

                File.Delete(jsonFilePath);
            }
            catch
            {
                //If the web connection cant be made (Future Update: then update the failed upload list so the next time the user has a connection, all the JSON files are posted)
                File.Delete(jsonFilePath);
            }
        }

        //// List objects
        //foreach (var obj in storageClient.ListObjects(bucketName, ""))
        //{
        //    Console.WriteLine(obj.Name);
        //}

        //// Download file
        //using (var fileStream = File.Create("Program-copy.cs"))
        //{
        //    storageClient.DownloadObject(bucketName, "Program.cs", fileStream);
        //}

        //foreach (var obj in Directory.GetFiles("."))
        //{
        //    Console.WriteLine(obj);
        //}

        //WriteLine("**Environment**");
        //WriteLine($"Platform: .NET Core 2.0");
        //WriteLine($"OS: {RuntimeInformation.OSDescription}");
        //WriteLine(message);
        //WriteLine();
    }
}
