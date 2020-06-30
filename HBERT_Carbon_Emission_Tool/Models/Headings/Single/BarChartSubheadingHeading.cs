using Autodesk.Revit.DB;
using CarbonEmissionTool.Models.Annotations;

namespace CarbonEmissionTool.Models.Headings
{
    class BarChartSubheadingHeading : IHeading
    {
        private double _xCoordinate = -27.0;
        private double _yCoordinate = 10.0;

        public View PlacementView { get; }

        public XYZ Origin { get; }

        public FontSize FontSize { get; }

        public ColorData Color { get; }

        public HorizontalTextAlignment HorizontalAlignment { get; }

        public double TextNoteWidth { get; }

        public string Title { get; }

        public BoldFormatter BoldFormatter { get; }

        public bool Vertical { get; }

        public BarChartSubheadingHeading(View view, string projectName)
        {
            this.PlacementView = view;

            this.Origin = new XYZ(_xCoordinate.ToDecimalFeet(), _yCoordinate.ToDecimalFeet(), 0.0);

            this.FontSize = FontSize.Eight;

            this.Color = HeadingColors.Red;

            this.TextNoteWidth = 25.0;

            this.Title = projectName;

            this.BoldFormatter = new BoldFormatter(0, 0);

            this.Vertical = false;

            this.HorizontalAlignment = HorizontalTextAlignment.Left;
        }
    }
}
