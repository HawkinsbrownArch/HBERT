using Autodesk.Revit.DB;
using CarbonEmissionTool.Models.Annotations;
using Color = System.Windows.Media.Color;

namespace CarbonEmissionTool.Models.Headings
{
    public class TreeChartHeading : IHeading
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
        /// Constructs a new <see cref="TreeChartHeading"/> object.
        /// </summary>
        public TreeChartHeading(Autodesk.Revit.DB.View view, XYZ origin, string title, double height, double width)
        {
            this.PlacementView = view;

            this.Origin = origin;

            this.Title = title;

            this.TextNoteWidth = 35.0;

            this.FontSize = FontSizeProcessor.FindBestPointSize(height, width);

            this.Color = HeadingColors.White;

            this.BoldFormatter = new BoldFormatter(0, 0);

            this.Vertical = false;
        }
    }
}
