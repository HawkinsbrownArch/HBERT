using Autodesk.Revit.DB;
using CarbonEmissionTool.Models.Charts;
using CarbonEmissionTool.Models.Headings;
using CarbonEmissionTool.Services;

namespace CarbonEmissionTool.Models
{
    public class CarbonEmissionToolMain
    {
        /// <summary>
        /// Computes the embodied carbon of the scheme using the Embodied Carbon Schedule
        /// </summary>
        public static void ComputeEmbodiedCarbon(IProjectDetails projectDetails, IPublishDetails publishDetails)
        {
            var carbonDataCache = ApplicationServices.CarbonDataCache;

            var filledRegionCache = new FilledRegionCache();
            
            using (var transaction = new Transaction(ApplicationServices.Document, "Carbon Emission Tool Main"))
            {
                transaction.Start();

                if (!carbonDataCache.IsEmpty)
                {
                    var newSheet = SheetUtils.CreateECSheet(publishDetails);

                    var treeChart = new TreeChart(carbonDataCache, filledRegionCache, newSheet);
                    var stackedBarChart = new StackedBarChart(projectDetails, carbonDataCache, filledRegionCache, newSheet, treeChart);

                    var axonometricView = new AxonometricView(publishDetails, newSheet);

                    HeadingGenerator.Create(projectDetails, treeChart, stackedBarChart, newSheet);

                    projectDetails.DataCapture.Upload(projectDetails);
                }

                transaction.Commit();
            };
        }
    }
}
