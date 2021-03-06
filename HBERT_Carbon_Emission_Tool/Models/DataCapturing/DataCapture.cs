﻿using System;
using System.IO;
using CarbonEmissionTool.Services;

namespace CarbonEmissionTool.Models
{
    public class DataCapture : IDataCapture
    {
        private readonly string _userRoamingFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

        private string _settingsFileDirectory => $"{this._userRoamingFolder}\\Autodesk\\Revit\\Addins\\HBERT";

        /// <summary>
        /// Uploads the form data input by the user to the Google Drive. 
        /// </summary>
        public void Upload(IProjectDetails projectDetails, CarbonDataCache carbonDataCache)
        {
            DateTime dateTime = DateTime.Now;
            string date = $"{dateTime.Year}-{dateTime.Month}-{dateTime.Day}";
            string time = dateTime.TimeOfDay.ToString();

            string jsonName = $"{NameUtils.ValidateFileName(projectDetails.Name)} {date}_{dateTime.Hour}-{dateTime.Minute}.json";

            string jsonFilePath = $"{_settingsFileDirectory}\\{jsonName}";

            if(!Directory.Exists(_settingsFileDirectory))
                Directory.CreateDirectory(_settingsFileDirectory);

            // Create a new JSON data file.
            DataWriter.WriteJson(projectDetails, carbonDataCache, jsonFilePath, date, time);
        }
    }
}
