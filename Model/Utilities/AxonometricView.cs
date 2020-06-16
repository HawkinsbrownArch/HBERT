using Autodesk.Revit.DB;
using CarbonEmissionTool.Model.Extensions;
using CarbonEmissionTool.Services;

namespace CarbonEmissionTool.Model.Utilities
{
    class AxonometricView
    {
        private double _viewportXOrigin = 126.0;
        private double _viewportYOrigin = 150.0;

        /// <summary>
        /// Creates the Axonomteric view and places it in a viewport on the <paramref name="sheet"/>
        /// </summary>
        public AxonometricView(ViewSheet sheet)
        {
            View3D axoView = ApplicationServices.AxoView;

            var origin = new XYZ(_viewportXOrigin.ToDecimalFeet(), _viewportYOrigin.ToDecimalFeet(), 0.0);
            Viewport viewport3D = Viewport.Create(ApplicationServices.Document, sheet.Id, axoView.Id, origin);

            ViewportUtils.SetViewportNoTitle(viewport3D);
        }
    }
}
