using Autodesk.Revit.DB;
using CarbonEmissionTool.Model.Annotations.Formatting;
using CarbonEmissionTool.Model.Enums;
using CarbonEmissionTool.Model.Extensions;
using CarbonEmissionTool.Model.Graphics;
using CarbonEmissionTool.Model.Interfaces;
using Color = System.Windows.Media.Color;

namespace CarbonEmissionTool.Model.Charts.BarCharts.Headings
{
    class BarChartSubheadingAnnotation : IAnnotation
    {
        private double _xCoordinate = -27.0;
        private double _yCoordinate = 10.0;

        public Autodesk.Revit.DB.View PlacementView { get; }

        public XYZ Origin { get; }

        public FontSize FontSize { get; }

        public Color Color { get; }

        public double TextNoteWidth { get; }

        public string Title { get; }

        public BoldFormatter BoldFormatter { get; }

        public bool Vertical { get; }

        public BarChartSubheadingAnnotation(Autodesk.Revit.DB.View view, string projectName)
        {
            this.PlacementView = view;

            this.Origin = new XYZ(_xCoordinate.ToDecimalFeet(), _yCoordinate.ToDecimalFeet(), 0.0);

            this.FontSize = FontSize.Eleven;

            this.Color = AnnotationColors.Red;

            this.TextNoteWidth = 25.0;

            this.Title = projectName;

            this.BoldFormatter = new BoldFormatter(0, 0);

            this.Vertical = false;
        }
    }
}
