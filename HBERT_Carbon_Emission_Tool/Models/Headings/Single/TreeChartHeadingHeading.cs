using Autodesk.Revit.DB;
using CarbonEmissionTool.Models.Annotations;
using CarbonEmissionTool.Settings;
using Color = System.Windows.Media.Color;

namespace CarbonEmissionTool.Models.Headings
{
    class TreeChartHeadingHeading : IHeading
    {
        private double _xCoordinate = 246.0;
        private double _yCoordinate = 158.0;

        public Autodesk.Revit.DB.View PlacementView { get; }
        public XYZ Origin { get; }

        public FontSize FontSize { get; }

        public Color Color { get; }

        public double TextNoteWidth { get; }

        public string Title { get; }

        public BoldFormatter BoldFormatter { get; }

        public bool Vertical { get; }

        public TreeChartHeadingHeading(Autodesk.Revit.DB.View view)
        {
            this.PlacementView = view;

            this.Origin = new XYZ(_xCoordinate.ToDecimalFeet(), _yCoordinate.ToDecimalFeet(), 0.0);

            this.FontSize = FontSize.Sixteen;

            this.Color = HeadingColors.Red;

            this.TextNoteWidth = ApplicationSettings.MaxTextNoteWidth;

            this.Title = "Embodied Carbon per Material";

            this.BoldFormatter = new BoldFormatter(0, this.Title.Length);

            this.Vertical = false;
        }
    }
}
