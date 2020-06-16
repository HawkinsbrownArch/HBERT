using Autodesk.Revit.DB;
using CarbonEmissionTool.Model.Annotations;
using CarbonEmissionTool.Model.Charts.BarCharts;
using CarbonEmissionTool.Model.Charts.TreeCharts;
using CarbonEmissionTool.Model.GoogleCloud;
using CarbonEmissionTool.Model.Interfaces;
using CarbonEmissionTool.Model.Utilities;
using CarbonEmissionTool.Services;

namespace CarbonEmissionTool.Model.Main
{
    public class CarbonEmissionToolMain
    {
        /// <summary>
        /// Computes the embodied carbon of the scheme using the Embodied Carbon Schedule
        /// </summary>
        public static void ComputeEmbodiedCarbon(IProjectDetails projectDetails)
        {
            //HbertMainForm hbertMainForm = new HbertMainForm();
            //System.Windows.Forms.Application.Run(hbertMainForm);

            //bool executeStepOne = WarningDialog.UserClosedForm(hbertMainForm);

            if (executeStepOne)
            {
                //ProjectDetails projectDetails = hbertMainForm.ActiveProjectDetails;

                //SheetInputsForm sheetInputsForm;

                /*
                //<<<<----START THE TRANSACTION---->>>>
                using (Transaction transaction = new Transaction(doc, "Carbon Emission Tool Main"))
                {
                    transaction.Start();

                    sheetInputsForm = new SheetInputsForm(projectDetails);
                    System.Windows.Forms.Application.Run(sheetInputsForm);

                    transaction.Commit();
                }
                */

                //bool executeStepTwo = WarningDialog.UserClosedForm(sheetInputsForm);

                if (executeStepTwo)
                {
                    //FamilySymbol titleBlock = ApplicationServices.TitleBlock;
                    //ViewSchedule embodiedCarbonSchedule = ApplicationServices.CarbonSchedule;

                    // TODO: Viewmodel
                    //string projectName = projectDetails.Name, location = projectDetails.Address, floorArea = $"{projectDetails.FloorArea} m²", ribaWorkstage = projectDetails.RibaWorkstage, projectVersion = projectDetails.Version, sector = projectDetails.Sector;
                    //bool newBuild = projectDetails.ProjectType;
                    //string projectType = newBuild ? "New Build" : "Refurbishment";

                    //int redColour = ColorUtils.ConvertRBGToInt(232, 70, 16);

                    /*
                     TODO: move to button click
                    ViewSheet oldECSheet = SheetUtils.GetECSheet();
                    bool sheetIsActive = SheetUtils.ExistingECSheetActive(doc, oldECSheet);

                    if (sheetIsActive)
                    {
                        WarningDialog.SheetIsActive();
                        return;
                    }
                    */

                    //<<<<----START THE TRANSACTION---->>>>
                    using (Transaction transaction = new Transaction(ApplicationServices.Document, "Carbon Emission Tool Main"))
                    {
                        transaction.Start();

                        if (!projectDetails.CarbonDataCache.IsEmpty)
                        {
                            ViewSheet newSheet = SheetUtils.CreateECSheet();

                            var treeChart = new TreeChart(projectDetails, newSheet);
                            var stackedBarChart = new StackedBarChart(projectDetails, newSheet, treeChart);

                            var axonometricView = new AxonometricView(newSheet);

                            AnnotationGenerator.Create(treeChart, stackedBarChart, projectDetails, newSheet);

                            CloudPublisher.Upload(projectDetails);
                        }

                        //<<<<----END THE TRANSACTION---->>>>
                        transaction.Commit();
                    };
                }
            }
        }
    }
}
