using Autodesk.Revit.DB;
using CarbonEmissionTool.Model.Extensions;
using CarbonEmissionTool.Model.Graphics;
using CarbonEmissionTool.Model.Interfaces;
using CarbonEmissionTool.Services;
using CarbonEmissionTool.Services.Caches;
using System;

namespace CarbonEmissionTool.Model.Annotations
{
    public class TextNoteCreator
    {
        /// <summary>
        /// Creates a new text note from the <paramref name="annotation"/> object.
        /// </summary>
        public static TextNote Create(IAnnotation annotation)
        {
            int color = ColorUtils.ConvertColorToInt(annotation.Color);

            var fontName = $"HBA_{annotation.FontSize}_{color}";

            var textNoteType = TextStyleCache[fontName];
            var options = new TextNoteOptions
            {
                TypeId = textNoteType.Id,
                Rotation = annotation.Vertical ? Math.PI / 2 : 0.0
            };

            var textNoteWidth = annotation.TextNoteWidth.ToDecimalFeet();

            var textNote = TextNote.Create(ApplicationServices.Document, annotation.PlacementView.Id, annotation.Origin, textNoteWidth, annotation.Title, options);

            annotation.BoldFormatter.Set(textNote);

            return textNote;
        }
    }
}
