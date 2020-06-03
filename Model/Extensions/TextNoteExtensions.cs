using System;
using Autodesk.Revit.DB;
using CarbonEmissionTool.Model.Annotations;
using CarbonEmissionTool.Model.Enums;
using CarbonEmissionTool.Services;

namespace CarbonEmissionTool.Model.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    static class TextNoteExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        public static void MakeTextNoteAllBold(this TextNote textNote)
        {
            FormattedText formattedText = textNote.GetFormattedText();
            formattedText.SetBoldStatus(true);

            textNote.SetFormattedText(formattedText);
        }

        /// <summary>
        /// Sets the characters within a text string to bold.
        /// </summary>
        public static void SetBoldCharacters(this TextNote textNote, int startChar, int boldCharCount)
        {
            FormattedText formattedText = textNote.GetFormattedText();

            //Create a new text range
            TextRange textRange = new TextRange(startChar, boldCharCount);

            formattedText.SetBoldStatus(textRange, true);
            //Set the text notes chars to bold
            textNote.SetFormattedText(formattedText);
        }
    }
}