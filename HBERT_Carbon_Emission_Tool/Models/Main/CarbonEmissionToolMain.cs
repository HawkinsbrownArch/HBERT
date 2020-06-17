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

            using (var transaction = new Transaction(ApplicationServices.Document, "Carbon Emission Tool Main"))
            {
                transaction.Start();

                if (!projectDetails.CarbonDataCache.IsEmpty)
                {
                    var newSheet = SheetUtils.CreateECSheet(projectDetails.TitleBlock);

                    var treeChart = new TreeChart(projectDetails, newSheet);
                    var stackedBarChart = new StackedBarChart(projectDetails, newSheet, treeChart);

                    var axonometricView = new AxonometricView(projectDetails, newSheet);

                    AnnotationGenerator.Create(projectDetails, treeChart, stackedBarChart, newSheet);

                    CloudPublisher.Upload(projectDetails);
                }

                transaction.Commit();
            };
        }
    }
}
