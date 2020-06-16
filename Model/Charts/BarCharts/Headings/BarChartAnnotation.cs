using Autodesk.Revit.DB;
using CarbonEmissionTool.Model.Annotations.Formatting;
using CarbonEmissionTool.Model.Charts.TreeCharts.Headings;
using CarbonEmissionTool.Model.Enums;
using CarbonEmissionTool.Model.Graphics;
using CarbonEmissionTool.Model.Interfaces;
using Color = System.Windows.Media.Color;

namespace CarbonEmissionTool.Model.Charts.BarCharts.Headings
{
    class BarChartAnnotation : IAnnotation
    {
        public Autodesk.Revit.DB.View PlacementView { get; }

        public XYZ Origin { get; }

        public FontSize FontSize { get; }

        public Color Color { get; }

        public double TextNoteWidth { get; }

        public string Title { get; }

        public BoldFormatter BoldFormatter { get; }

        public bool Vertical { get; }

        /// <summary>
        /// Constructs a new <see cref="TreeChartAnnotation"/> object.
        /// </summary>
        public BarChartAnnotation(Autodesk.Revit.DB.View view, XYZ origin, string title)
        {
            this.PlacementView = view;

            this.Origin = origin;

            this.Title = title;

            this.TextNoteWidth = 35.0;

            this.FontSize = FontSize.Six;

            this.Color = AnnotationColors.Red;

            this.BoldFormatter = new BoldFormatter(0, 0);

            this.Vertical = true;
        }
    }
}
