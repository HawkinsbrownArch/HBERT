using Autodesk.Revit.DB;
using CarbonEmissionTool.Models.Annotations;
using CarbonEmissionTool.Settings;

namespace CarbonEmissionTool.Models.Headings
{
    /// <summary>
    /// The floor area title value heading of the scheme input by the user.
    /// </summary>
    class FloorAreaValueHeading : IHeading
    {
        private double _xCoordinate = ApplicationSettings.HeadingOffsetFromLeftSide + 22.2;
        private double _yCoordinate = 227.2;

        public View PlacementView { get; }

        public XYZ Origin { get; }

        public FontSize FontSize { get; }

        public BoldFormatter BoldFormatter { get; }

        public ColorData Color { get; }

        public HorizontalTextAlignment HorizontalAlignment { get; }

        public double TextNoteWidth { get; }

        public string Title { get; }

        public bool Vertical { get; }

        /// <summary>
        /// Constructs a new <see cref="FloorAreaValueHeading"/> object.
        /// </summary>
        public FloorAreaValueHeading(View view, IProjectDetails projectDetails)
        {
            this.PlacementView = view;

            this.Origin = new XYZ(_xCoordinate.ToDecimalFeet(), _yCoordinate.ToDecimalFeet(), 0.0);

            this.FontSize = FontSize.Eight;

            this.Color = HeadingColors.Black;

            this.TextNoteWidth = 110.0;

            this.Title = $"{projectDetails.FloorArea} m²";

            this.BoldFormatter = new BoldFormatter(0, 0);

            this.Vertical = false;

            this.HorizontalAlignment = HorizontalTextAlignment.Left;
        }
    }
}
