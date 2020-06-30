using System;
using Autodesk.Revit.DB;
using CarbonEmissionTool.Models.Annotations;
using CarbonEmissionTool.Settings;

namespace CarbonEmissionTool.Models.Headings
{
    /// <summary>
    /// The date value heading.
    /// </summary>
    class DateValueHeading : IHeading
    {
        private double _xCoordinate = ApplicationSettings.HeadingOffsetFromLeftSide + 12.2;
        private double _yCoordinate = 208.3;

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
        /// Constructs a new <see cref="DateValueHeading"/> object.
        /// </summary>
        public DateValueHeading(View view)
        {
            this.PlacementView = view;

            this.Origin = new XYZ(_xCoordinate.ToDecimalFeet(), _yCoordinate.ToDecimalFeet(), 0.0);

            this.FontSize = FontSize.Eight;

            this.Color = HeadingColors.Black;

            this.TextNoteWidth = 110.0;

            DateTime now = DateTime.Now;

            this.Title = $"{now.Day.ToString()}.{now.Month.ToString()}.{now.Year.ToString()}";

            this.BoldFormatter = new BoldFormatter(0, 0);

            this.Vertical = false;

            this.HorizontalAlignment = HorizontalTextAlignment.Left;
        }
    }
}
