using Autodesk.Revit.DB;
using CarbonEmissionTool.Models.Annotations;
using CarbonEmissionTool.Settings;

namespace CarbonEmissionTool.Models.Headings
{
    /// <summary>
    /// The RIBA Workstage heading title heading. 
    /// </summary>
    class RibaWorkstageTitleHeading : IHeading
    {
        private double _xCoordinate = ApplicationSettings.HeadingOffsetFromLeftSide;
        private double _yCoordinate = 214.8;

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
        /// Constructs a new <see cref="RibaWorkstageTitleHeading"/> object.
        /// </summary>
        public RibaWorkstageTitleHeading(View view)
        {
            this.PlacementView = view;

            this.Origin = new XYZ(_xCoordinate.ToDecimalFeet(), _yCoordinate.ToDecimalFeet(), 0.0);

            this.FontSize = FontSize.Eight;

            this.Color = HeadingColors.Red;

            this.TextNoteWidth = 110.0;

            this.Title = "RIBA Workstage:";

            this.BoldFormatter = new BoldFormatter(0, this.Title.Length);

            this.Vertical = false;

            this.HorizontalAlignment = HorizontalTextAlignment.Left;
        }
    }
}
