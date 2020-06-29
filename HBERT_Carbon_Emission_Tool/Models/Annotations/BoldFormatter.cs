using Autodesk.Revit.DB;

namespace CarbonEmissionTool.Models.Annotations
{
    /// <summary>
    /// An object used to convert characters in a Text note text to bold.
    /// </summary>
    public class BoldFormatter
    {
        /// <summary>
        /// The 0 indexed start character to being converting to bold.
        /// </summary>
        public int StartCharacter { get; }

        /// <summary>
        /// The number of characters in a text note to convert to bold starting from the <see cref="StartCharacter"/>.
        /// </summary>
        public int BoldCharacterCount { get; }

        /// <summary>
        /// Constructs a new <see cref="BoldFormatter"/>.
        /// </summary>
        public BoldFormatter(int startCharacter, int boldCharacterCount)
        {
            this.StartCharacter = startCharacter;
            this.BoldCharacterCount = boldCharacterCount;
        }

        /// <summary>
        /// Currently not used.
        /// </summary>
        public void MakeTextNoteAllBold(TextNote textNote)
        {
            FormattedText formattedText = textNote.GetFormattedText();
            formattedText.SetBoldStatus(true);

            textNote.SetFormattedText(formattedText);
        }

        /// <summary>
        /// Sets the characters within a text string to bold.
        /// </summary>
        public void Set(TextNote textNote)
        {
            if (this.BoldCharacterCount > 0)
            {
                FormattedText formattedText = textNote.GetFormattedText();

                //Create a new text range
                TextRange textRange = new TextRange(this.StartCharacter, this.BoldCharacterCount);

                formattedText.SetBoldStatus(textRange, true);

                //Set the text notes chars to bold
                textNote.SetFormattedText(formattedText);
            }
        }
    }
}
