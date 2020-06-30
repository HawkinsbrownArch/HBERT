using Autodesk.Revit.DB;
using CarbonEmissionTool.Models.Annotations;
using CarbonEmissionTool.Settings;

namespace CarbonEmissionTool.Models.Headings
{
    class ProjectRevisionHeading : IHeading
    {
        private double _xCoordinate = ApplicationSettings.HeadingOffsetFromLeftSide;
        private double _yCoordinate = 254.0;

        public View PlacementView { get; }

        public XYZ Origin { get; }

        public FontSize FontSize { get; }

        public ColorData Color { get; }

        public HorizontalTextAlignment HorizontalAlignment { get; }

        public double TextNoteWidth { get; }

        public string Title { get; }

        public BoldFormatter BoldFormatter { get; }

        public bool Vertical { get; }

        /// <summary>
        /// Constructs a new <see cref="ProjectRevisionHeading"/> object.
        /// </summary>
        public ProjectRevisionHeading(View view, IProjectDetails projectDetails)
        {
            this.PlacementView = view;

            this.Origin = new XYZ(_xCoordinate.ToDecimalFeet(), _yCoordinate.ToDecimalFeet(), 0.0);

            this.FontSize = FontSize.TwentyTwo;

            this.Color = HeadingColors.Red;

            this.TextNoteWidth = ApplicationSettings.MaxTextNoteWidth;

            this.Title = projectDetails.Revision;

            this.BoldFormatter = new BoldFormatter(0, 0);

            this.Vertical = false;

            this.HorizontalAlignment = HorizontalTextAlignment.Left;
        }
    }
}
