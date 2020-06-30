﻿using System;
using Autodesk.Revit.DB;
using CarbonEmissionTool.Models.Annotations;
using Color = System.Windows.Media.Color;

namespace CarbonEmissionTool.Models.Headings
{
    /// <summary>
    /// The date value heading.
    /// </summary>
    class DateValueHeading : IHeading
    {
        private double _xCoordinate = 20.2;
        private double _yCoordinate = 208.5;

        public Autodesk.Revit.DB.View PlacementView { get; }

        public XYZ Origin { get; }

        public FontSize FontSize { get; }

        public BoldFormatter BoldFormatter { get; }

        public Color Color { get; }

        public double TextNoteWidth { get; }

        public string Title { get; }

        public bool Vertical { get; }

        /// <summary>
        /// Constructs a new <see cref="DateValueHeading"/> object.
        /// </summary>
        public DateValueHeading(Autodesk.Revit.DB.View view)
        {
            this.PlacementView = view;

            this.Origin = new XYZ(_xCoordinate.ToDecimalFeet(), _yCoordinate.ToDecimalFeet(), 0.0);

            this.FontSize = FontSize.Ten;

            this.Color = HeadingColors.Black;

            this.TextNoteWidth = 110.0;

            DateTime now = DateTime.Now;

            this.Title = $"{now.Day.ToString()}.{now.Month.ToString()}.{now.Year.ToString()}";

            this.BoldFormatter = new BoldFormatter(0, 0);

            this.Vertical = false;
        }
    }
}
