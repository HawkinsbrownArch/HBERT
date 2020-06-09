using CarbonEmissionTool.Model.Interfaces;
using CarbonEmissionTool.Model.JSON;
using CarbonEmissionTool.Model.Utilities;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using CarbonEmissionTool.Model.Enums;

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

            string bucketName = projectDetails.ExportStatus == CarbonExportStatus.Final ? @"hbert-json-storage.appspot.com" : @"hbert-json-draft-storage";

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
                // If the web connection cant be made.
                File.Delete(jsonFilePath);
            }
        }
    }
}
