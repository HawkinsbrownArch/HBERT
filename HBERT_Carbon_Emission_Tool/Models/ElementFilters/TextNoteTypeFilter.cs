using Autodesk.Revit.DB;
using CarbonEmissionTool.Services;
using System.Collections.Generic;
using System.Linq;

namespace CarbonEmissionTool.Models
{
    class TextNoteTypeFilter
    {
        /// <summary>
        /// Returns a list of all the <see cref="TextNoteType"/> in the active document.
        /// </summary>
        public static List<TextNoteType> GetAll()
        {
            var textNoteTypes = new FilteredElementCollector(ApplicationServices.Document).OfClass(typeof(TextNoteType))
                .WhereElementIsElementType().Cast<TextNoteType>().ToList();

            return textNoteTypes;
        }
    }
}
