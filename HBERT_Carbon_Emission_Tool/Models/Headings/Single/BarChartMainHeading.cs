using Autodesk.Revit.DB;
using CarbonEmissionTool.Models.Annotations;
using CarbonEmissionTool.Settings;

namespace CarbonEmissionTool.Models.Headings
{
    class BarChartMainHeading : IHeading
    {
        private double _xCoordinate = 246.0;
        private double _yCoordinate = 265.0;

        public View PlacementView { get; }

        public XYZ Origin { get; }

        public FontSize FontSize { get; }

        public ColorData Color { get; }

        public HorizontalTextAlignment HorizontalAlignment { get; }

        public double TextNoteWidth { get; }

        public string Title { get; }

        public BoldFormatter BoldFormatter { get; }

        public bool Vertical { get; }

        public BarChartMainHeading(View view)
        {
            this.PlacementView = view;

            this.Origin = new XYZ(_xCoordinate.ToDecimalFeet(), _yCoordinate.ToDecimalFeet(), 0.0);

            this.FontSize = FontSize.Fourteen;

            this.Color = HeadingColors.Red;

            this.TextNoteWidth = ApplicationSettings.MaxTextNoteWidth;

            this.Title = "Total Embodied Carbon";

            this.BoldFormatter = new BoldFormatter(0, this.Title.Length);

            this.Vertical = false;

            this.HorizontalAlignment = HorizontalTextAlignment.Left;
        }
    }
}
