using Autodesk.Revit.DB;
using CarbonEmissionTool.Models.Charts;
using CarbonEmissionTool.Models.Headings;
using CarbonEmissionTool.Services;

namespace CarbonEmissionTool.Models
{
    public class CarbonEmissionToolMain
    {
        /// <summary>
        /// Facade method which computes the embodied carbon of the scheme, creates
        /// the sheet, and charts and places the axonometric view on the sheet before publishing
        /// the results to a JSON file stored in the users roaming folder.
        /// </summary>
        public static void ComputeEmbodiedCarbon(IProjectDetails projectDetails, IPublishDetails publishDetails)
        {
            var carbonDataCache = ApplicationServices.CarbonDataCache;

            var filledRegionCache = new FilledRegionCache();

            if (!carbonDataCache.IsEmpty)
            {
                using (var transaction = new Transaction(ApplicationServices.Document, "Carbon Emission Tool Main"))
                {
                    transaction.Start();

                    var newSheet = SheetUtils.CreateECSheet(publishDetails);

                    var treeChart = new TreeChart(carbonDataCache, filledRegionCache, newSheet);
                    var stackedBarChart = new StackedBarChart(projectDetails, carbonDataCache, filledRegionCache, newSheet, treeChart);

                    var axonometricView = new AxonometricView(publishDetails, newSheet);

                    HeadingGenerator.Create(projectDetails, treeChart, stackedBarChart, newSheet);

                    ApplicationServices.DataCapture.Upload(projectDetails, carbonDataCache);

                    UserInputMonitor.RegisterUserInputs(projectDetails);

                    transaction.Commit();
                }

                HelpDialog.HbertSuccessfullyRun();
            }
        }
    }
}
