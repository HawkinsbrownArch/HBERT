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
    class AverageEmbodiedCarbonValueAnnotation : IAnnotation
    {
        private double _xCoordinate = 7.0;
        private double _yCoordinate = -24.5;

        public Autodesk.Revit.DB.View PlacementView { get; }
        public XYZ Origin { get; }

        public FontSize FontSize { get; }

        public Color Color { get; }

        public double TextNoteWidth { get; }

        public string Title { get; }

        public BoldFormatter BoldFormatter { get; }

        public bool Vertical { get; }

        /// <summary>
        /// Constructs a new <see cref="AverageEmbodiedCarbonValueAnnotation"/> object.
        /// </summary>
        public AverageEmbodiedCarbonValueAnnotation(Autodesk.Revit.DB.View view, IProjectDetails projectDetails, CarbonDataCache carbonDataCache)
        {
            this.PlacementView = view;

            this.Origin = new XYZ(_xCoordinate.ToDecimalFeet(), _yCoordinate.ToDecimalFeet(), 0.0);

            this.FontSize = FontSize.Thirty;

            this.Color = AnnotationColors.Black;

            this.TextNoteWidth = 120.0;

            double totalEmbodiedCarbon = carbonDataCache.Sum(v => v.EmbodiedCarbon);
            double average = totalEmbodiedCarbon / projectDetails.FloorArea;

            int decimalPlaces = average < 1 ? 2 : 0;
            string averageText = Math.Round(average * 1000.0, decimalPlaces).ToString(); // Converted to kg from tons, so multiply by 1000.
            
            this.Title = $"{averageText} kg CO₂e/m²";

            this.BoldFormatter = new BoldFormatter(0, averageText.Length);

            this.Vertical = false;
        }
    }
}
