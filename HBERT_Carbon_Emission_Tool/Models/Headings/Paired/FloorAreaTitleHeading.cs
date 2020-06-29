using Autodesk.Revit.DB;
using CarbonEmissionTool.Models.Annotations;
using Color = System.Windows.Media.Color;

namespace CarbonEmissionTool.Models.Headings
{
    /// <summary>
    /// The Floor Area heading title heading. 
    /// </summary>
    class FloorAreaTitleHeading : IHeading
    {
        private double _xCoordinate = 10.0;
        private double _yCoordinate = 259.0;

        public Autodesk.Revit.DB.View PlacementView { get; }

        public XYZ Origin { get; }

        public FontSize FontSize { get; }

        public BoldFormatter BoldFormatter { get; }

        public Color Color { get; }

        public double TextNoteWidth { get; }

        public string Title { get; }

        public bool Vertical { get; }

        /// <summary>
        /// Constructs a new <see cref="FloorAreaTitleHeading"/> object.
        /// </summary>
        public FloorAreaTitleHeading(Autodesk.Revit.DB.View view)
        {
            this.PlacementView = view;

            this.Origin = new XYZ(_xCoordinate.ToDecimalFeet(), _yCoordinate.ToDecimalFeet(), 0.0);

            this.FontSize = FontSize.Ten;

            this.Color = HeadingColors.Red;

            this.TextNoteWidth = 110.0;

            this.Title = "Floor Area:";

            this.BoldFormatter = new BoldFormatter(0, this.Title.Length);

            this.Vertical = false;
        }
    }
}
