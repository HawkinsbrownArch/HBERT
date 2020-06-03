using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using CarbonEmissionTool.Model.Enums;
using CarbonEmissionTool.Model.Extensions;
using CarbonEmissionTool.Services;

namespace CarbonEmissionTool.Model.Annotations
{
    class TextNoteFactory
    {
        /// <summary>
        /// Creates a new text note with the <see cref="FontSize"/>. Input <paramref name="width"/> in mm.
        /// </summary>
        public static TextNote Create(ElementId viewId, XYZ origin, FontSize fontSize, double color, double width, string title, bool bold, bool vertical)
        {
            var fontName = $"HBA_{fontSize}_{color}";

            var textNoteType = Annotation.TextStyleDictionary[fontName];
            var options = new TextNoteOptions
            {
                TypeId = textNoteType.Id,
                Rotation = vertical ? Math.PI / 2 : 0.0
            };

            var textNote = TextNote.Create(ApplicationServices.Document, viewId, origin, width.ToDecimalFeet(), title, options);

            if (bold)
                textNote.MakeTextNoteAllBold();

            return textNote;
        }
    }
}
