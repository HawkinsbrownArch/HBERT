using Autodesk.Revit.DB;
using CarbonEmissionTool.Model.Annotations.Formatting;
using CarbonEmissionTool.Model.Enums;
using CarbonEmissionTool.Model.Extensions;
using CarbonEmissionTool.Model.Graphics;
using CarbonEmissionTool.Model.Interfaces;
using CarbonEmissionTool.Services;
using CarbonEmissionTool.Settings;
using Color = System.Windows.Media.Color;

namespace CarbonEmissionTool.Model.Annotations.Headings
{
    class ProjectNameAnnotation : IAnnotation
    {
        private double _xCoordinate = 10.0;
        private double _yCoordinate = 267.0;

        public Autodesk.Revit.DB.View PlacementView { get; }

        public XYZ Origin { get; }

        public FontSize FontSize { get; }

        public Color Color { get; }

        public double TextNoteWidth { get; }

        public string Title { get; }

        public BoldFormatter BoldFormatter { get; }

        public bool Vertical { get; }

        /// <summary>
        /// Constructs a new <see cref="ProjectNameAnnotation"/> object.
        /// </summary>
        public ProjectNameAnnotation(Autodesk.Revit.DB.View view, IProjectDetails projectDetails)
        {
            this.PlacementView = view;

            this.Origin = new XYZ(_xCoordinate.ToDecimalFeet(), _yCoordinate.ToDecimalFeet(), 0.0);

            this.FontSize = FontSize.Thirty;

            this.Color = AnnotationColors.Red;

            this.TextNoteWidth = ApplicationSettings.MaxTextNoteWidth;

            this.Title = projectDetails.Name;

            this.BoldFormatter = new BoldFormatter(0, this.Title.Length);

            this.Vertical = false;
        }
    }
}
