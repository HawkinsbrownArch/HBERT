using Autodesk.Revit.DB;
using CarbonEmissionTool.Models.Annotations;
using Color = System.Windows.Media.Color;

namespace CarbonEmissionTool.Models.Headings
{
    /// <summary>
    /// The Sector value title heading of the scheme input by the user.
    /// </summary>
    class SectorValueHeading : IHeading
    {
        private double _xCoordinate = 23.4;
        private double _yCoordinate = 240.0;

        public Autodesk.Revit.DB.View PlacementView { get; }

        public XYZ Origin { get; }

        public FontSize FontSize { get; }

        public BoldFormatter BoldFormatter { get; }

        public Color Color { get; }

        public double TextNoteWidth { get; }

        public string Title { get; }

        public bool Vertical { get; }

        /// <summary>
        /// Constructs a new <see cref="SectorValueHeading"/> object.
        /// </summary>
        public SectorValueHeading(Autodesk.Revit.DB.View view, IProjectDetails projectDetails)
        {
            this.PlacementView = view;

            this.Origin = new XYZ(_xCoordinate.ToDecimalFeet(), _yCoordinate.ToDecimalFeet(), 0.0);

            this.FontSize = FontSize.Ten;

            this.Color = HeadingColors.Black;

            this.TextNoteWidth = 110.0;

            this.Title = StringUtils.GenerateSectorString(projectDetails);

            this.BoldFormatter = new BoldFormatter(0, 0);

            this.Vertical = false;
        }
    }
}
