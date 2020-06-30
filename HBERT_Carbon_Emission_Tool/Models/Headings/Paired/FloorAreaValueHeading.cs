using Autodesk.Revit.DB;
using CarbonEmissionTool.Models.Annotations;
using Color = System.Windows.Media.Color;

namespace CarbonEmissionTool.Models.Headings
{
    /// <summary>
    /// The floor area title value heading of the scheme input by the user.
    /// </summary>
    class FloorAreaValueHeading : IHeading
    {
        private double _xCoordinate = 29.2;
        private double _yCoordinate = 227.4;

        public Autodesk.Revit.DB.View PlacementView { get; }

        public XYZ Origin { get; }

        public FontSize FontSize { get; }

        public BoldFormatter BoldFormatter { get; }

        public Color Color { get; }

        public double TextNoteWidth { get; }

        public string Title { get; }

        public bool Vertical { get; }

        /// <summary>
        /// Constructs a new <see cref="FloorAreaValueHeading"/> object.
        /// </summary>
        public FloorAreaValueHeading(Autodesk.Revit.DB.View view, IProjectDetails projectDetails)
        {
            this.PlacementView = view;

            this.Origin = new XYZ(_xCoordinate.ToDecimalFeet(), _yCoordinate.ToDecimalFeet(), 0.0);

            this.FontSize = FontSize.Ten;

            this.Color = HeadingColors.Black;

            this.TextNoteWidth = 110.0;

            this.Title = $"{projectDetails.FloorArea} m²";

            this.BoldFormatter = new BoldFormatter(0, 0);

            this.Vertical = false;
        }
    }
}
