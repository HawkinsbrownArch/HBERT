using System;
using System.Linq;
using Autodesk.Revit.DB;
using CarbonEmissionTool.Models.Annotations;
using CarbonEmissionTool.Services;

namespace CarbonEmissionTool.Models.Headings
{
    class TotalEmbodiedCarbonValueHeading : IHeading
    {
        private double _xCoordinate = 7.0;
        private double _yCoordinate = -8.5;

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
        /// Constructs a new <see cref="TotalEmbodiedCarbonValueHeading"/> object.
        /// </summary>
        public TotalEmbodiedCarbonValueHeading(View view, CarbonDataCache carbonDataCache)
        {
            this.PlacementView = view;

            this.Origin = new XYZ(_xCoordinate.ToDecimalFeet(), _yCoordinate.ToDecimalFeet(), 0.0);

            this.FontSize = FontSize.TwentyTwo;

            this.Color = HeadingColors.Black;

            this.TextNoteWidth = 120.0;

            double total = carbonDataCache.Sum(v => v.EmbodiedCarbon);
            string totalTitle = Math.Round(total, 0).ToString();

            this.Title = $"{totalTitle} ton CO₂e";

            this.BoldFormatter = new BoldFormatter(0, totalTitle.Length);

            this.Vertical = false;

            this.HorizontalAlignment = HorizontalTextAlignment.Left;
        }
    }
}
