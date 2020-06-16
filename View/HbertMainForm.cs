using Autodesk.Revit.DB;
using System;
using System.Diagnostics;
using System.Windows.Forms;
using CarbonEmissionTool.Model.BuildingProject;
using CarbonEmissionTool.Model.Enums;
using CarbonEmissionTool.Model.Utilities;
using Form = System.Windows.Forms.Form;

namespace HBERT_UI
{
    public partial class HbertMainForm : Form
    {
        /// <summary>Stores details about the project.</summary>
        internal ProjectDetails ActiveProjectDetails { get; set; }
        /// <summary>The export status of the project</summary>
        internal CarbonExportStatus ExportStatus { get; set; } = CarbonExportStatus.None;

        private System.Drawing.Color FormRedColour = System.Drawing.Color.FromArgb(232, 70, 16);

        private bool BuildingElementsSelected => checkBoxStructuralFrame.Checked | checkBoxFacade.Checked | checkBoxExternalWorks.Checked | checkBoxRoof.Checked | checkBoxFittingsFurnishingsEquipment.Checked |
                                                   checkBoxFoundations.Checked | checkBoxWindowsExternalDoors.Checked | checkBoxInternalWalls.Checked | checkBoxInternalFinishes.Checked | checkBoxOther.Checked;

        private bool SectorSelected => checkBoxEducation.Checked | checkBoxWorkplace.Checked | checkBoxInfrastructureTransport.Checked | checkBoxResidential.Checked | checkBoxCivicCommunityCulture.Checked;

        private bool TextBoxesInput => textBoxProjectVersion.Text.Length > 0 & textBoxProjectAddress.Text.Length > 0 & numericUpDownFloorArea.Value > 0 & comboBoxRIBAWorkstage.Text.Length > 0 & textBoxProjectName.Text.Length > 0;


        public HbertMainForm()
        {
            InitializeComponent();

            ActiveProjectDetails = new ProjectDetails();
            Show();
        }

        //Launches the sheet input form and updates the ActiveProjectDetails object to capture any user inputs or changes to the project address or name
        private void SubmitForm(CarbonExportStatus exportStatus)
        {
            if (!TextBoxesInput)
            {
                if (textBoxProjectName.Text.Length == 0)
                    labelRequiredProjectName.Visible = true;
                else if (textBoxProjectVersion.Text.Length == 0)
                    labelRequiredProjectVersion.Visible = true;
                else if (textBoxProjectAddress.Text.Length == 0)
                    labelRequiredAddress.Visible = true;
                else if (numericUpDownFloorArea.Value == 0)
                    labelRequiredFloorArea.Visible = true;
                else if (comboBoxRIBAWorkstage.Text.Length == 0)
                    labelRequiredRIBA.Visible = true;
            }
            else if (!BuildingElementsSelected) //If not building elements are selected
            {
                labelRequiredBuildingElements.Visible = true;
            }
            else if (buttonNewBuild.BackColor != FormRedColour & buttonRefurbishment.BackColor != FormRedColour)
            {
                labelRequiredNewBuildRefurb.Visible = true;
            }
            else if (!SectorSelected) //If no sector is selected
            {
                labelRequiredSector.Visible = true;
            }
            else if (!checkBoxTermsOfService.Checked)
            {
                labelRequiredTerms.Visible = true;
            }
            else
            {
                ExportStatus = exportStatus;
                ActiveProjectDetails.ExportStatus = exportStatus;

                //Update the ActiveProjectDetails in case the user has made changes
                ActiveProjectDetails.Name = textBoxProjectName.Text;
                ActiveProjectDetails.Version = textBoxProjectVersion.Text;
                ActiveProjectDetails.Address = textBoxProjectAddress.Text;
                ActiveProjectDetails.FloorArea = Convert.ToDouble(numericUpDownFloorArea.Value);

                ActiveProjectDetails.RibaWorkstage = comboBoxRIBAWorkstage.Text;
                ActiveProjectDetails.NewBuild = buttonRefurbishment.BackColor == System.Drawing.Color.DimGray;

                Close();
            }
        }

        private void SetBuildingElementsRequired(object sender)
        {
            CheckBox checkBox = (CheckBox)sender;

            if (checkBox.Checked)
                labelRequiredBuildingElements.Visible = false;

            ActiveProjectDetails.BuildElements[checkBox.Tag.ToString()] = checkBox.Checked;
        }

        private void SetSectorRequired(CheckBox checkBox)
        {
            if (checkBox.Checked)
                labelRequiredSector.Visible = false;

            ActiveProjectDetails.Sectors[checkBox.Tag.ToString()] = checkBox.Checked;
        }

        private void buttonFinal_Click(object sender, EventArgs e)
        {
            SubmitForm(CarbonExportStatus.Final);
        }

        private void buttonDraft_Click(object sender, EventArgs e)
        {
            SubmitForm(CarbonExportStatus.Draft);
        }

        private void textBoxProjectName_TextChanged(object sender, EventArgs e)
        {
            labelRequiredProjectName.Visible = textBoxProjectName.Text.Length < 1;
        }

        private void checkBoxEducation_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkBox = (CheckBox)sender;
            ActiveProjectDetails.Education = checkBox.Checked ? "Education" : "";

            SetSectorRequired(checkBox);
        }

        private void checkBoxWorkplace_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkBox = (CheckBox)sender;
            ActiveProjectDetails.Workplace = checkBox.Checked ? "Workplace" : "";

            SetSectorRequired(checkBox);
        }

        private void checkBoxInfrastructureTransport_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkBox = (CheckBox)sender;
            ActiveProjectDetails.Infrastructure = checkBox.Checked ? "Infrastructure and Transport" : "";

            SetSectorRequired(checkBox);
        }

        private void checkBoxResidential_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkBox = (CheckBox)sender;
            ActiveProjectDetails.Residential = checkBox.Checked ? "Residential" : "";

            SetSectorRequired(checkBox);
        }

        private void checkBoxCivicCommunityCulture_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkBox = (CheckBox)sender;
            ActiveProjectDetails.Civic = checkBox.Checked ? "Civic, Community and Culture" : "";

            SetSectorRequired(checkBox);
        }

        private void checkBoxTermsOfService_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkBox = (CheckBox)sender;
            if(checkBox.Checked)
                labelRequiredTerms.Visible = false;
        }

        private void HbertMainForm_Load(object sender, EventArgs e)
        {
            textBoxProjectName.Text = ActiveProjectDetails.Name;
            textBoxProjectAddress.Text = ActiveProjectDetails.Address;
        }

        private void labelBimorph_Click(object sender, EventArgs e)
        {
            //ProcessStartInfo sInfo = new ProcessStartInfo("https://bimorph.com/");
            //Process.Start(sInfo);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            ProcessStartInfo sInfo = new ProcessStartInfo("https://www.hawkinsbrown.com/");
            Process.Start(sInfo);
        }

        private void buttonNewBuild_Click(object sender, EventArgs e)
        {
            buttonRefurbishment.BackColor = System.Drawing.Color.DimGray;
            buttonNewBuild.BackColor = FormRedColour;

            labelRequiredNewBuildRefurb.Visible = false;
        }

        private void buttonRefurbishment_Click(object sender, EventArgs e)
        {
            buttonNewBuild.BackColor = System.Drawing.Color.DimGray;
            buttonRefurbishment.BackColor = FormRedColour;

            labelRequiredNewBuildRefurb.Visible = false;
        }

        private void labelFloorAreaTotal_Click(object sender, EventArgs e)
        {

        }

        private void textBoxProjectVersion_TextChanged(object sender, EventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            labelRequiredProjectVersion.Visible = textBox.Text.Length < 1;
        }

        private void textBoxProjectAddress_TextChanged(object sender, EventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            labelRequiredAddress.Visible = textBox.Text.Length < 1;
        }

        private void textBoxTotalFloorArea_TextChanged(object sender, EventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            labelRequiredFloorArea.Visible = textBox.Text.Length < 1;
        }

        private void comboBoxRIBAWorkstage_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            labelRequiredRIBA.Visible = comboBox.Text.Length < 1;
        }

        private void checkBoxStructuralFrame_CheckedChanged(object sender, EventArgs e)
        {
            SetBuildingElementsRequired(sender);
        }

        private void checkBoxFacade_CheckedChanged(object sender, EventArgs e)
        {
            SetBuildingElementsRequired(sender);
        }

        private void checkBoxExternalWorks_CheckedChanged(object sender, EventArgs e)
        {
            SetBuildingElementsRequired(sender);
        }

        private void checkBoxRoof_CheckedChanged(object sender, EventArgs e)
        {
            SetBuildingElementsRequired(sender);
        }

        private void checkBoxFittingsFurnishingsEquipment_CheckedChanged(object sender, EventArgs e)
        {
            SetBuildingElementsRequired(sender);
        }

        private void checkBoxFoundations_CheckedChanged(object sender, EventArgs e)
        {
            SetBuildingElementsRequired(sender);
        }

        private void checkBoxWindowsExternalDoors_CheckedChanged(object sender, EventArgs e)
        {
            SetBuildingElementsRequired(sender);
        }

        private void checkBoxInternalWalls_CheckedChanged(object sender, EventArgs e)
        {
            SetBuildingElementsRequired(sender);
        }

        private void checkBoxInternalFinishes_CheckedChanged(object sender, EventArgs e)
        {
            SetBuildingElementsRequired(sender);
        }

        private void checkBoxOther_CheckedChanged(object sender, EventArgs e)
        {
            SetBuildingElementsRequired(sender);
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown numericUpDown = (NumericUpDown)sender;
            labelRequiredFloorArea.Visible = numericUpDown.Value == 0;
        }
    }
}
