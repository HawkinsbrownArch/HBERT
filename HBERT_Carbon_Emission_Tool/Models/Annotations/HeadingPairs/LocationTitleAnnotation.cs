using Autodesk.Revit.DB;
using CarbonEmissionTool.Model.Annotations.Formatting;
using CarbonEmissionTool.Model.Enums;
using CarbonEmissionTool.Model.Extensions;
using CarbonEmissionTool.Model.Graphics;
using CarbonEmissionTool.Model.Interfaces;
using Color = System.Windows.Media.Color;

namespace CarbonEmissionTool.Model.Annotations.HeadingPairs
{
    /// <summary>
    /// The Location heading title annotation. 
    /// </summary>
    class LocationTitleAnnotation : IAnnotation
    {
        private double _xCoordinate = 10.0;
        private double _yCoordinate = 252.6;

        public Autodesk.Revit.DB.View PlacementView { get; }

        public XYZ Origin { get; }

        public FontSize FontSize { get; }

        public BoldFormatter BoldFormatter { get; }

        public Color Color { get; }

        public double TextNoteWidth { get; }

        public string Title { get; }

        public bool Vertical { get; }

        /// <summary>
        /// Constructs a new <see cref="LocationTitleAnnotation"/> object.
        /// </summary>
        public LocationTitleAnnotation(Autodesk.Revit.DB.View view)
        {
            this.PlacementView = view;

            this.Origin = new XYZ(_xCoordinate.ToDecimalFeet(), _yCoordinate.ToDecimalFeet(), 0.0);

            this.FontSize = FontSize.Ten;

            this.Color = AnnotationColors.Red;

            this.TextNoteWidth = 110.0;

            this.Title = "Location:";

            this.BoldFormatter = new BoldFormatter(0, this.Title.Length);

            this.Vertical = false;
        }
    }
}
