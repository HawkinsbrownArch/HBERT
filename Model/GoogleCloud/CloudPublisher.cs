using CarbonEmissionTool.Model.Interfaces;
using CarbonEmissionTool.Model.Utilities;
using CarbonEmissionTool.Services.Caches;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using System;
using System.IO;
using System.Reflection;
using CarbonEmissionTool.Model.JSONManager;

namespace CarbonEmissionTool.Model.GoogleCloud
{
    public class CloudPublisher : ICloudPublisher
    {
        /// <summary>
        /// Uploads the form data input by the user to the Google Drive. 
        /// </summary>
        public static void Upload(IProjectDetails projectDetails)
        {
            DateTime dateTime = DateTime.Now;
            string date = $"{dateTime.Year}-{dateTime.Month}-{dateTime.Day}";
            string time = dateTime.TimeOfDay.ToString();

            string thisAssemblyPath = Assembly.GetExecutingAssembly().Location;
            string jsonFolderLocation = Path.GetDirectoryName(thisAssemblyPath);
            string jsonName = $"{NamingUtils.ValidateFileName(projectDetails.Name)} {date}_{dateTime.Hour}-{dateTime.Minute}.json";

            string jsonFilePath = $"{jsonFolderLocation}\\{jsonName}";

            //Create a new JSON data file
            DataWriter.WriteJSON(projectDetails, jsonFilePath, date, time);

            string bucketName = @"hbert-json-storage.appspot.com";

            GoogleCredential credential = null;
            try
            {
                credential = GoogleCredential.FromJson(JsonAuthentication.LoadCredentials());
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
