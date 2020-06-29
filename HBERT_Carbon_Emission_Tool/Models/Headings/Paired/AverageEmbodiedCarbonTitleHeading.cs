using Autodesk.Revit.DB;
using CarbonEmissionTool.Models.Annotations;
using Color = System.Windows.Media.Color;

namespace CarbonEmissionTool.Models.Headings
{
    class AverageEmbodiedCarbonTitleHeading : IHeading
    {
        private double _xCoordinate = -27.0;
        private double _yCoordinate = -27.0;

        public Autodesk.Revit.DB.View PlacementView { get; }
        public XYZ Origin { get; }

        public FontSize FontSize { get; }

        public Color Color { get; }

        public double TextNoteWidth { get; }

        public string Title { get; }

        public BoldFormatter BoldFormatter { get; }

        public bool Vertical { get; }

        /// <summary>
        /// Constructs a new <see cref="AverageEmbodiedCarbonValueHeading"/> object.
        /// </summary>
        public AverageEmbodiedCarbonTitleHeading(Autodesk.Revit.DB.View view)
        {
            this.PlacementView = view;

            this.Origin = new XYZ(_xCoordinate.ToDecimalFeet(), _yCoordinate.ToDecimalFeet(), 0.0);

            this.FontSize = FontSize.Eleven;

            this.Color = HeadingColors.Red;

            this.TextNoteWidth = 25.0;

            this.Title = "Average per m² of Floor Area";

            this.BoldFormatter = new BoldFormatter(0, 0);

            this.Vertical = false;
        }
    }
}
