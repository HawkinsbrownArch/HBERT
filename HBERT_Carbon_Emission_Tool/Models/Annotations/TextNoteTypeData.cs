using Autodesk.Revit.DB;

namespace CarbonEmissionTool.Models.Annotations
{
    /// <summary>
    /// Stores Revit TestStyle and name required by the titles on the HBERT sheet.
    /// </summary>
    public class TextNoteTypeData
    {
        /// <summary>
        /// The name of this text style.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The Revit text style.
        /// </summary>
        public TextNoteType TextType { get; }

        /// <summary>
        /// Constructs a new <see cref="TextNoteTypeData"/>.
        /// </summary>
        public TextNoteTypeData(TextNoteType textType, string name)
        {
            this.Name = name;

            this.TextType = textType;
        }
    }
}
