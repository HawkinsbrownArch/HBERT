﻿using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using CarbonEmissionTool.Model.BuildingProject;
using CarbonEmissionTool.Model.Collectors;
using CarbonEmissionTool.Model.Enums;
using CarbonEmissionTool.Model.Utilities;
using CarbonEmissionTool.Services;
using Form = System.Windows.Forms.Form;

namespace HBERT_UI
{
    public partial class SheetInputsForm : Form
    {
        private Dictionary<string, FamilySymbol> TitleBlockDict { get; set; }
        private ViewSchedule ViewShedule { get; set; }
        private Dictionary<string, View3D> AxoViewDict { get; set; }
        internal CarbonExportStatus ExportStatus { get; set; } = CarbonExportStatus.None;

        private bool AllSelectionsSatisfied => comboBoxTitleBlock.Text.Length > 0 & comboBoxAxoView.Text.Length > 0 & !buttonImportSchedule.Visible;

        private ProjectDetails ActiveProjectDetails { get; set; }
        
        public SheetInputsForm(ProjectDetails projectDetails)
        {
            InitializeComponent();

            var document = ApplicationServices.Document;

            TitleBlockDict = TitleBlockFilter.GetAll(document);

            ViewShedule = ScheduleUtils.GetCarbonSchedule(document);
            AxoViewDict = RevitViewFilter.Get3DViews();

            ActiveProjectDetails = projectDetails;
        }

        /// <summary>
        /// Sets the <see cref="buttonPublish"/> to active once the user has satisfied the <see cref="SheetInputsForm"/> inputs.
        /// </summary>
        private void ActivatePublishButton()
        {
            if (AllSelectionsSatisfied)
                buttonPublish.Enabled = true;
            else if (buttonPublish.Enabled)
                buttonPublish.Enabled = false;
        }

        private void buttonDecarbonise_Click(object sender, EventArgs e)
        {
            ActiveProjectDetails.TitleBlock = TitleBlockDict[comboBoxTitleBlock.Text];
            ActiveProjectDetails.CarbonSchedule = ViewShedule;
            ActiveProjectDetails.AxoView = AxoViewDict[comboBoxAxoView.Text];

            ActiveProjectDetails.FormShutDown = true;
            ExportStatus = CarbonExportStatus.Final; //Default to any but None (it doesnt matter what this setting is as the first form sets the value. Instead, this field is used to hangle what happens if the user cancels the form)

            Close();
        }

        private void comboBoxTitleBlock_SelectedIndexChanged(object sender, EventArgs e)
        {
            ActivatePublishButton();
        }

        private void SheetInputsForm_Load(object sender, EventArgs e)
        {
            comboBoxTitleBlock.Items.AddRange(TitleBlockDict.Keys.ToArray());
            comboBoxAxoView.Items.AddRange(AxoViewDict.Keys.ToArray());

            if(ViewShedule == null)
            {
                labelCarbonRatingSchedule.Visible = true;
                buttonImportSchedule.Visible = true;
                
                buttonImportSchedule.Enabled = true;
                buttonImportSchedule.BackColor = System.Drawing.Color.FromArgb(232, 70, 16);
            }
        }

        private void comboBoxMaterialCarbonSchedule_SelectedIndexChanged(object sender, EventArgs e)
        {
            ActivatePublishButton();
        }

        private void comboBoxAxoView_SelectedIndexChanged(object sender, EventArgs e)
        {
            ActivatePublishButton();
        }

        private void labelImportSchedule_Click(object sender, EventArgs e)
        {

        }

        private void buttonImportSchedule_Click(object sender, EventArgs e)
        {
            var document = ApplicationServices.Document;

            buttonImportSchedule.Visible = false;
            labelCarbonRatingSchedule.Visible = false;

            ScheduleUtils.ImportECScedule(document);
            
            ViewShedule = ScheduleUtils.GetCarbonSchedule(document);

            buttonImportSchedule.Visible = false;

            ActivatePublishButton();
        }

        private void textBoxImportSchedule_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
