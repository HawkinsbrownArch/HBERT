using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using CarbonEmissionTool.Model.Interfaces;
using CarbonEmissionTool.Model.JSON;
using CarbonEmissionTool.Model.RevitProject;
using CarbonEmissionTool.Model.Utilities;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;

namespace CarbonEmissionTool.Model.GoogleCloud
{
    class CloudPublisher : ICloudPublisher
    {
        internal static void Upload(IProjectDetails projectDetails, List<KeyValuePair<string, double>> eCData)
        {
            DateTime dateTime = DateTime.Now;
            string date = dateTime.Year + "-" + dateTime.Month + "-" + dateTime.Day;
            string time = dateTime.TimeOfDay.ToString();

            string thisAssemblyPath = Assembly.GetExecutingAssembly().Location;
            string jsonFolderLocation = Path.GetDirectoryName(thisAssemblyPath);
            string jsonName = NamingUtils.ValidateFileName(projectDetails.ProjectName) + " " + date + "_" + dateTime.Hour + "-" + dateTime.Minute + ".json";

            string jsonFilePath = jsonFolderLocation + @"\" + jsonName;

            //Create a new JSON data file
            DataWriter.WriteJSON(projectDetails, eCData, jsonFilePath, date, time);

            string bucketName = projectDetails.ExportStatus == StringUtils.CarbonExportStatus.Final ? @"hbert-json-storage.appspot.com" : @"hbert-json-draft-storage";

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
