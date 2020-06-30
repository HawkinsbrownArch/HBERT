using Autodesk.Revit.DB;
using CarbonEmissionTool.Services;
using System.Collections.Generic;
using System.Linq;
using Color = System.Windows.Media.Color;

namespace CarbonEmissionTool.Models.Annotations
{
    /// <summary>
    /// Stores and creates <see cref="TextNoteType"/> for the headings on the HBERT sheet.
    /// </summary>
    public class TextNoteTypeCreator
    {
        private List<TextNoteType> TypeList { get; }

        /// <summary>
        /// Constructs a new <see cref="TextNoteTypeCreator"/>.
        /// </summary>
        public TextNoteTypeCreator()
        {
            this.TypeList = TextNoteTypeFilter.GetAll();
        }

        /// <summary>
        /// Returns a <see cref="Autodesk.Revit.DB.ElementType"/> by matching <see cref="FontSize"/> and
        /// <see cref="Color"/>. If the style cant be found, it is created and returned.
        /// </summary>
        public TextNoteType GetBySizeAndColor(FontSize fontSize, ColorData colorData)
        {
            string fontTypeName = NameUtils.GenerateTextStyleName(colorData, fontSize);

            var style = this.TypeList.Find(s => s.Name == fontTypeName);
            
            // If no match was found, create a new style with the name.
            if (style == null)
            {
                var newStyle = this.Create(fontSize, colorData, fontTypeName);

                return newStyle;
            }

            return style;
        }

        /// <summary>
        /// Creates a new text note style if it cant be found in the active document and adds
        /// it to the <see cref="TypeList"/>.
        /// </summary>
        private TextNoteType Create(FontSize fontSize, ColorData colorData, string newFontTypeName)
        {
            var textNoteData = this.TypeList.First();
            
            var textNoteType = textNoteData.Duplicate(newFontTypeName) as TextNoteType;

            ApplicationServices.Document.Regenerate();

            ParameterUtils.SetTextNoteTypeParameters(textNoteType, fontSize, colorData);

            this.TypeList.Add(textNoteType);

            return textNoteType;
        }
    }
}