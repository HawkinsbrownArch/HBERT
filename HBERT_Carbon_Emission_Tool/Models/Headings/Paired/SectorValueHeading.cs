using Autodesk.Revit.DB;
using CarbonEmissionTool.Models.Annotations;
using CarbonEmissionTool.Settings;

namespace CarbonEmissionTool.Models.Headings
{
    /// <summary>
    /// The Sector value title heading of the scheme input by the user.
    /// </summary>
    class SectorValueHeading : IHeading
    {
        private double _xCoordinate = ApplicationSettings.HeadingOffsetFromLeftSide + 16.0;
        private double _yCoordinate = 239.8;

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
        /// Constructs a new <see cref="SectorValueHeading"/> object.
        /// </summary>
        public SectorValueHeading(View view, IProjectDetails projectDetails)
        {
            this.PlacementView = view;

            this.Origin = new XYZ(_xCoordinate.ToDecimalFeet(), _yCoordinate.ToDecimalFeet(), 0.0);

            this.FontSize = FontSize.Eight;

            this.Color = HeadingColors.Black;

            this.TextNoteWidth = ApplicationSettings.MaxTextNoteWidth;

            this.Title = StringUtils.GenerateSectorString(projectDetails);

            this.BoldFormatter = new BoldFormatter(0, 0);

            this.Vertical = false;

            this.HorizontalAlignment = HorizontalTextAlignment.Left;
        }
    }
}
