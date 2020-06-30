using Autodesk.Revit.DB;
using CarbonEmissionTool.Models.Annotations;
using CarbonEmissionTool.Settings;

namespace CarbonEmissionTool.Models.Headings
{
    class CarbonCalculationDisclaimerHeading : IHeading
    {
        private double _xCoordinate = 128.0;
        private double _yCoordinate = 60.0;

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
        /// Constructs a new <see cref="ProjectNameHeading"/> object.
        /// </summary>
        public CarbonCalculationDisclaimerHeading(View view)
        {
            this.PlacementView = view;

            this.Origin = new XYZ(_xCoordinate.ToDecimalFeet(), _yCoordinate.ToDecimalFeet(), 0.0);

            this.FontSize = FontSize.Eight;

            this.Color = HeadingColors.LightGrey;

            this.TextNoteWidth = 215.0;

            this.Title = ApplicationSettings.CarbonCalculationDisclaimer;

            this.BoldFormatter = new BoldFormatter(0, 0);

            this.Vertical = false;

            this.HorizontalAlignment = HorizontalTextAlignment.Center;
        }
    }
}
