using System;
using System.Linq;
using Autodesk.Revit.DB;
using CarbonEmissionTool.Model.Annotations.Formatting;
using CarbonEmissionTool.Model.Enums;
using CarbonEmissionTool.Model.Extensions;
using CarbonEmissionTool.Model.Graphics;
using CarbonEmissionTool.Model.Interfaces;
using CarbonEmissionTool.Services.Caches;
using Color = System.Windows.Media.Color;

namespace CarbonEmissionTool.Model.Charts.BarCharts.HeadingPairs
{
    class TotalEmbodiedCarbonValueAnnotation : IAnnotation
    {
        private double _xCoordinate = 7.0;
        private double _yCoordinate = -8.5;

        public Autodesk.Revit.DB.View PlacementView { get; }

        public XYZ Origin { get; }

        public FontSize FontSize { get; }

        public Color Color { get; }

        public double TextNoteWidth { get; }

        public string Title { get; }

        public BoldFormatter BoldFormatter { get; }

        public bool Vertical { get; }

        /// <summary>
        /// Constructs a new <see cref="TotalEmbodiedCarbonValueAnnotation"/> object.
        /// </summary>
        public TotalEmbodiedCarbonValueAnnotation(Autodesk.Revit.DB.View view, CarbonDataCache carbonDataCache)
        {
            this.PlacementView = view;

            this.Origin = new XYZ(_xCoordinate.ToDecimalFeet(), _yCoordinate.ToDecimalFeet(), 0.0);

            this.FontSize = FontSize.Thirty;

            this.Color = AnnotationColors.Black;

            this.TextNoteWidth = 120.0;

            double total = carbonDataCache.Sum(v => v.EmbodiedCarbon);
            string totalTitle = Math.Round(total, 0).ToString();

            this.Title = $"{totalTitle} ton CO₂e";

            this.BoldFormatter = new BoldFormatter(0, totalTitle.Length);

            this.Vertical = false;
        }
    }
}
