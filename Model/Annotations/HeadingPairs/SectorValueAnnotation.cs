using Autodesk.Revit.DB;
using CarbonEmissionTool.Model.Annotations.Formatting;
using CarbonEmissionTool.Model.Enums;
using CarbonEmissionTool.Model.Extensions;
using CarbonEmissionTool.Model.Graphics;
using CarbonEmissionTool.Model.Interfaces;
using CarbonEmissionTool.Model.Utilities;
using Color = System.Windows.Media.Color;

namespace CarbonEmissionTool.Model.Annotations.HeadingPairs
{
    /// <summary>
    /// The Sector value title annotation of the scheme input by the user.
    /// </summary>
    class SectorValueAnnotation : IAnnotation
    {
        private double _xCoordinate = 23.4;
        private double _yCoordinate = 271.5;

        public Autodesk.Revit.DB.View PlacementView { get; }

        public XYZ Origin { get; }

        public FontSize FontSize { get; }

        public BoldFormatter BoldFormatter { get; }

        public Color Color { get; }

        public double TextNoteWidth { get; }

        public string Title { get; }

        public bool Vertical { get; }

        /// <summary>
        /// Constructs a new <see cref="SectorValueAnnotation"/> object.
        /// </summary>
        public SectorValueAnnotation(Autodesk.Revit.DB.View view, IProjectDetails projectDetails)
        {
            this.PlacementView = view;

            this.Origin = new XYZ(_xCoordinate.ToDecimalFeet(), _yCoordinate.ToDecimalFeet(), 0.0);

            this.FontSize = FontSize.Ten;

            this.Color = AnnotationColors.Black;

            this.TextNoteWidth = 110.0;

            this.Title = StringUtils.GenerateSectorString(projectDetails);

            this.BoldFormatter = new BoldFormatter(0, 0);

            this.Vertical = false;
        }
    }
}
