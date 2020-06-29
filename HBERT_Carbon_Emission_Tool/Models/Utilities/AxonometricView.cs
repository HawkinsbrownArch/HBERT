using Autodesk.Revit.DB;
using CarbonEmissionTool.Services;

namespace CarbonEmissionTool.Models
{
    class AxonometricView
    {
        private double _viewportXOrigin = 126.0;
        private double _viewportYOrigin = 150.0;

        /// <summary>
        /// Creates the Axonomteric view and places it in a viewport on the <paramref name="sheet"/>
        /// </summary>
        public AxonometricView(IPublishDetails publishDetails, ViewSheet sheet)
        {
            View3D axoView = publishDetails.AxoView;

            var origin = new XYZ(_viewportXOrigin.ToDecimalFeet(), _viewportYOrigin.ToDecimalFeet(), 0.0);
            Viewport viewport3D = Viewport.Create(ApplicationServices.Document, sheet.Id, axoView.Id, origin);

            ViewportUtils.SetViewportNoTitle(viewport3D);
        }
    }
}
