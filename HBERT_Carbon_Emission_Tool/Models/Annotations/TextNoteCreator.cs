using Autodesk.Revit.DB;
using CarbonEmissionTool.Services;
using System;
using CarbonEmissionTool.Settings;

namespace CarbonEmissionTool.Models.Annotations
{
    public class TextNoteCreator
    {
        private TextStyleCache _textStyleCache = new TextStyleCache();

        /// <summary>
        /// Creates a new text note from the <paramref name="heading"/> object.
        /// </summary>
        public TextNote Create(IHeading heading)
        {
            int color = ColorUtils.ConvertColorToInt(heading.Color);

            var fontName = $"{ApplicationSettings.TextStyleNamePrefix}{heading.FontSize}_{color}";

            var textNoteTypeData = _textStyleCache.GetByName(fontName);
            var options = new TextNoteOptions
            {
                TypeId = textNoteTypeData.TextType.Id,
                Rotation = heading.Vertical ? Math.PI / 2 : 0.0
            };

            var textNoteWidth = heading.TextNoteWidth.ToDecimalFeet();

            var textNote = TextNote.Create(ApplicationServices.Document, heading.PlacementView.Id, heading.Origin, textNoteWidth, heading.Title, options);

            heading.BoldFormatter.Set(textNote);

            return textNote;
        }
    }
}
