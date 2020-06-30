using Autodesk.Revit.DB;
using CarbonEmissionTool.Models.Annotations;
using Color = System.Windows.Media.Color;

namespace CarbonEmissionTool.Models.Headings
{
    /// <summary>
    /// The RIBA Workstage value title heading of the scheme input by the user.
    /// </summary>
    class RibaWorkstageValueHeading : IHeading
    {
        private double _xCoordinate = 40.0;
        private double _yCoordinate = 214.8;

        public Autodesk.Revit.DB.View PlacementView { get; }

        public XYZ Origin { get; }

        public FontSize FontSize { get; }

        public BoldFormatter BoldFormatter { get; }

        public Color Color { get; }

        public double TextNoteWidth { get; }

        public string Title { get; }

        public bool Vertical { get; }

        /// <summary>
        /// Constructs a new <see cref="RibaWorkstageValueHeading"/> object.
        /// </summary>
        public RibaWorkstageValueHeading(Autodesk.Revit.DB.View view, IProjectDetails projectDetails)
        {
            this.PlacementView = view;

            this.Origin = new XYZ(_xCoordinate.ToDecimalFeet(), _yCoordinate.ToDecimalFeet(), 0.0);

            this.FontSize = FontSize.Ten;

            this.Color = HeadingColors.Black;

            this.TextNoteWidth = 110.0;

            this.Title = projectDetails.RibaWorkstage.ToString();

            this.BoldFormatter = new BoldFormatter(0, 0);

            this.Vertical = false;
        }
    }
}
