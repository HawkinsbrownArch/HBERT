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

        public List<TextNote> CreateMainHeadings(ElementId viewId, FontSize fontSize, double xCoord, double yCoord, double spacingAlongY, double colour, string[] projectData)
        {
            string[] titles = { "Date:", "RIBA Workstage:", "Location:", "Floor Area:", "Type:", "Sector:" };
            double[] dataOffsets = { 8.58, 28.34, 15.28, 17.6, 8.23, 11.84 };

            double spacing = 1.6.ToDecimalFeet(); //The spacing between the title and its data

            List<TextNote> titleTextNotes = new List<TextNote>();
            for (int i = 0; i < titles.Length; i++)
            {
                double xCoordInFt = xCoord.ToDecimalFeet();
                double yCoordInFt = (yCoord + i * spacingAlongY).ToDecimalFeet();

                //Create the title text note
                XYZ origin = new XYZ(xCoordInFt, yCoordInFt, 0.0);

                TextNote textNoteTitle = TextNoteFactory.Create(viewId, origin, fontSize, colour, 110.0, titles[i], true, false);
                titleTextNotes.Add(textNoteTitle);

                //Create the data text note
                origin = new XYZ(dataOffsets[i].ToDecimalFeet() + xCoordInFt + spacing, yCoordInFt, 0.0);

                TextNote textNoteData = TextNoteFactory.Create(viewId, origin, fontSize, 0, 110.0, projectData[i], false, false);
                titleTextNotes.Add(textNoteData);
            }

            return titleTextNotes;
        }
    }
}
