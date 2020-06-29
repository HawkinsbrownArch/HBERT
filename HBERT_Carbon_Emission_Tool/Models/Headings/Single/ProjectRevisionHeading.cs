using Autodesk.Revit.DB;
using CarbonEmissionTool.Models.Annotations;
using CarbonEmissionTool.Settings;
using Color = System.Windows.Media.Color;

namespace CarbonEmissionTool.Models.Headings
{
    class ProjectRevisionHeading : IHeading
    {
        private double _xCoordinate = 10.0;
        private double _yCoordinate = 255.0;

        public Autodesk.Revit.DB.View PlacementView { get; }
        public XYZ Origin { get; }

        public FontSize FontSize { get; }

        public Color Color { get; }

        public double TextNoteWidth { get; }

        public string Title { get; }

        public BoldFormatter BoldFormatter { get; }

        public bool Vertical { get; }

        /// <summary>
        /// Constructs a new <see cref="ProjectRevisionHeading"/> object.
        /// </summary>
        public ProjectRevisionHeading(Autodesk.Revit.DB.View view, IProjectDetails projectDetails)
        {
            this.PlacementView = view;

            this.Origin = new XYZ(_xCoordinate.ToDecimalFeet(), _yCoordinate.ToDecimalFeet(), 0.0);

            this.FontSize = FontSize.Thirty;

            this.Color = HeadingColors.Red;

            this.TextNoteWidth = ApplicationSettings.MaxTextNoteWidth;

            this.Title = projectDetails.Revision;

            this.BoldFormatter = new BoldFormatter(0, 0);

            this.Vertical = false;
        }
    }
}
