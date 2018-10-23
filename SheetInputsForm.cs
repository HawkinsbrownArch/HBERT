﻿using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Events;
using HBERT.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using Form = System.Windows.Forms.Form;

namespace HBERT_UI
{
    public partial class SheetInputsForm : Form
    {
        private Dictionary<string, FamilySymbol> TitleBlockDict { get; set; }
        private ViewSchedule ViewShedule { get; set; }
        private Dictionary<string, View3D> AxoViewDict { get; set; }
        internal HBFormUtils.CarbonExportStatus ExportStatus { get; set; } = HBFormUtils.CarbonExportStatus.None;

        private bool AllSelectionsSatisfied => comboBoxTitleBlock.Text.Length > 0 & comboBoxAxoView.Text.Length > 0;

        private ProjectDetails ActiveProjectDetails { get; set; }
        
        public SheetInputsForm(ProjectDetails projectDetails, string sheetNumber, string sheetName)
        {
            InitializeComponent();

            TitleBlockDict = HBFormUtils.GetTitleBlocks(projectDetails.ActiveDocument);

            ViewShedule = Utilities.GetCarbonSchedule(projectDetails.ActiveDocument);
            AxoViewDict = HBFormUtils.Get3DViews(projectDetails.ActiveDocument, sheetNumber, sheetName);

            ActiveProjectDetails = projectDetails;
        }

        private void buttonDecarbonise_Click(object sender, EventArgs e)
        {
            ActiveProjectDetails.TitleBlock = TitleBlockDict[comboBoxTitleBlock.Text];
            ActiveProjectDetails.CarbonSchedule = ViewShedule;
            ActiveProjectDetails.AxoView = AxoViewDict[comboBoxAxoView.Text];

            ActiveProjectDetails.FormShutDown = true;
            ExportStatus = HBFormUtils.CarbonExportStatus.Final; //Default to any but None (it doesnt matter what this setting is as the first form sets the value. Instead, this field is used to hangle what happens if the user cancels the form)

            Close();
        }

        private void comboBoxTitleBlock_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (AllSelectionsSatisfied)
                buttonPublish.Enabled = true;
            else if (buttonPublish.Enabled)
                buttonPublish.Enabled = false;
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
            if (AllSelectionsSatisfied)
                buttonPublish.Enabled = true;
            else if (buttonPublish.Enabled)
                buttonPublish.Enabled = false;
        }

        private void comboBoxAxoView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (AllSelectionsSatisfied)
                buttonPublish.Enabled = true;
            else if (buttonPublish.Enabled)
                buttonPublish.Enabled = false;
        }

        private void labelImportSchedule_Click(object sender, EventArgs e)
        {

        }

        private void buttonImportSchedule_Click(object sender, EventArgs e)
        {
            buttonImportSchedule.Visible = false;
            labelCarbonRatingSchedule.Visible = false;

            Utilities.ImportECScedule(ActiveProjectDetails.ActiveDocument);
            
            ViewShedule = Utilities.GetCarbonSchedule(ActiveProjectDetails.ActiveDocument);

            buttonImportSchedule.Visible = false;
        }

        private void textBoxImportSchedule_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
