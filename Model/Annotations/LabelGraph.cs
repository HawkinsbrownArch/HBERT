using System.Collections.Generic;
using Autodesk.Revit.DB;
using CarbonEmissionTool.Model.Extensions;

namespace CarbonEmissionTool.Model.Annotations
{
    class LabelGraph
    {
        private Annotation _annotation;

        public LabelGraph(Annotation annotation)
        {
            _annotation = annotation;
        }

        public List<TextNote> AnnotateGraph(Document doc, ElementId viewId, Annotation annotation, double textNoteWidth, double convertToFt, int colour, bool vertical)
        {
            List<TextNote> textNotes = new List<TextNote>();
            for (int i = 0; i < annotation.TextValues.Count; i++)
            {
                TextNote newTextNote = TextNoteExtensions.CreateTextNote(doc, viewId, annotation.OriginPoints[i], annotation.TextPointSize[i], colour, textNoteWidth / convertToFt, annotation.TextValues[i], true, vertical);

                textNotes.Add(newTextNote);
            }
            return textNotes;
        }
    }
}