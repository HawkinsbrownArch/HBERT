using Autodesk.Revit.DB;
using CarbonEmissionTool.Services;
using System;

namespace CarbonEmissionTool.Models.Annotations
{
    public class TextNoteCreator
    {
        TextNoteTypeCreator _textNoteTypeCreator = new TextNoteTypeCreator();

        /// <summary>
        /// Creates a new <see cref="TextNote"/> from the <paramref name="heading"/> object.
        /// </summary>
        public TextNote Create(IHeading heading)
        {
            var color = heading.Color;

            var textNoteType = _textNoteTypeCreator.GetBySizeAndColor(heading.FontSize, color);

            var options = new TextNoteOptions
            {
                TypeId = textNoteType.Id,
                Rotation = heading.Vertical ? Math.PI / 2 : 0.0,
                HorizontalAlignment = heading.HorizontalAlignment
            };

            var textNoteWidth = heading.TextNoteWidth.ToDecimalFeet();

            var textNote = TextNote.Create(ApplicationServices.Document, heading.PlacementView.Id, heading.Origin, textNoteWidth, heading.Title, options);

            heading.BoldFormatter.Set(textNote);

            return textNote;
        }
    }
}
