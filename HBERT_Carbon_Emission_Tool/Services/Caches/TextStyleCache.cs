using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.DB;
using CarbonEmissionTool.Models;
using CarbonEmissionTool.Models.Annotations;

namespace CarbonEmissionTool.Services
{
    public class TextStyleCache
    {
        private List<TextNoteTypeData> TextStyleList { get; }

        /// <summary>
        /// Constructs a new <see cref="TextStyleCache"/>.
        /// </summary>
        public TextStyleCache()
        {
            this.TextStyleList = new List<TextNoteTypeData>();

            this.PopulateStyleDictionary();
        }

        /// <summary>
        /// Returns a <see cref="ElementType"/> by name. If the name doesn't exist returns null.
        /// </summary>
        public TextNoteTypeData GetByName(string textStyleName)
        {
            var style = this.TextStyleList.Find(s => s.Name == textStyleName);

            return style;
        }

        /// <summary>
        /// Populates the <see cref="TextStyleList"/>.
        /// </summary>
        private void PopulateStyleDictionary()
        {
            var doc = ApplicationServices.Document;

            var defaultTextNoteType = new FilteredElementCollector(doc).OfClass(typeof(TextNoteType)).Cast<TextNoteType>().First();

            var allTextNoteTypes = new FilteredElementCollector(doc).OfClass(typeof(TextNoteType))
                .WhereElementIsElementType().Cast<TextNoteType>().ToList();

            var fontSizes = new[] { FontSize.FortyFive, FontSize.Thirty, FontSize.Sixteen, FontSize.Eleven, FontSize.Ten, FontSize.Six };

            var colors = new[] { HeadingColors.Red, HeadingColors.Black, HeadingColors.White };

            foreach (var fontSize in fontSizes)
            {
                foreach (var color in colors)
                {
                    string newName = NameUtils.GenerateTextStyleName(color, fontSize);

                    var elementType = allTextNoteTypes.Find(tn => tn.Name == newName);

                    // Create the new note style if it doesn't exist.
                    if (elementType == null)
                    {
                        elementType = defaultTextNoteType.Duplicate(newName) as TextNoteType;

                        ApplicationServices.Document.Regenerate();

                        ParameterUtils.SetTextNoteTypeParameters(elementType, fontSize, color);
                    }

                    var textNoteTypeData = new TextNoteTypeData(elementType, newName);

                    this.TextStyleList.Add(textNoteTypeData);
                }
            }
        }
    }
}