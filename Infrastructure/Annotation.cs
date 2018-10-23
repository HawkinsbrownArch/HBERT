using System;
using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.DB;
using HBERT.Infrastructure;
using Autodesk.Revit.UI;

namespace HBERT
{
    class Annotation
    {
        //Int 0 = color, int 1 = font size, int 2 = bold
        internal static Dictionary<string, ElementType> TextStyleDictionary { get; set; }

        internal List<XYZ> OriginPoints { get; set; }
        internal List<FontSize> TextPointSize { get; set; }
        internal List<string> TextValues { get; set; }

        internal enum FontSize
        {
            None = 0,
            Six = 6,
            Ten = 10,
            Eleven = 11,
            Sixteen = 16,
            Thirty = 30,
            FourtyFive = 45
        }

        //Default constructor
        internal Annotation()
        {
            OriginPoints = new List<XYZ>();
            TextPointSize = new List<FontSize>();
            TextValues = new List<string>();
        }

        internal static void GenerateStyleDictionary(Document doc, double convertToFt, double convertPointToMm, int redHB)
        {
            TextNoteType defaultType = new FilteredElementCollector(doc).OfClass(typeof(TextNoteType)).Cast<TextNoteType>().First();
            List<TextNoteType> allTextNoteTypes = new FilteredElementCollector(doc).OfClass(typeof(TextNoteType)).Cast<TextNoteType>().ToList();
            ProjectInfo projectInfo = doc.ProjectInformation;

            TextStyleDictionary = new Dictionary<string, ElementType>();

            string hbaFont = "HelveticaNeueLT Std";
            string fontDefault = "Arial";

            string name = "HBA_";
            FontSize[] fontSizes = new FontSize[] { FontSize.FourtyFive, FontSize.Thirty, FontSize.Sixteen, FontSize.Eleven, FontSize.Ten, FontSize.Six };

            //Colours = red, black and white
            int[] colors = new[] { redHB, Utilities.ConvertColourToInt(0, 0, 0), Utilities.ConvertColourToInt(254, 254, 254) };

            //Iterate the font sizes
            for (int i = 0; i < fontSizes.Length; i++)
            {
                double fontSize = (Convert.ToDouble(fontSizes[i]) / convertPointToMm) / convertToFt;

                //Iterate the list of colours
                for (int c = 0; c < colors.Length; c++)
                {
                    int color = colors[c];
                    string newName = name + fontSizes[i].ToString() + "_" + color.ToString();

                    ElementType elementTypeExists = allTextNoteTypes.Find(tn => tn.Name == newName);
                    //Check to see if the text note style already exists
                    if (elementTypeExists != null)
                    {
                        TextStyleDictionary[newName] = elementTypeExists;
                    }
                    else //Create the new note style if it doesn't exist
                    {
                        ElementType newType = defaultType.Duplicate(newName);

                        doc.Regenerate();

                        newType.get_Parameter(BuiltInParameter.LINE_COLOR).Set(c);
                            
                        List<BuiltInParameter> parameters = new List<BuiltInParameter> { BuiltInParameter.LINE_COLOR, BuiltInParameter.TEXT_SIZE, BuiltInParameter.TEXT_BACKGROUND };
                        List<dynamic> values = new List<dynamic> { color, fontSize, 1 };

                        Utilities.SetParameters(newType, parameters, values);

                        BuiltInParameter fontType = BuiltInParameter.TEXT_FONT;
                        try //Try setting the font to the default HBA font
                        {
                            newType.get_Parameter(fontType).Set(hbaFont);
                        }
                        catch //If it fails then default to arial (probably because the font isn't installed)
                        {
                            newType.get_Parameter(fontType).Set(fontDefault);
                        }

                        TextStyleDictionary[newName] = newType;
                    }
                }
            }
        }

        internal TextNote CreateTextNote(Document doc, ElementId viewId, XYZ origin, FontSize fontSize, double color, double width, string title, bool bold, bool vertical)
        {
            string fontName = "HBA_" + fontSize.ToString() + "_" + color.ToString();

            ElementType textNoteType = TextStyleDictionary[fontName];
            TextNoteOptions options = new TextNoteOptions();
            options.TypeId = textNoteType.Id;
            options.Rotation = vertical ? Math.PI / 2 : 0.0;

            TextNote newTextNote = TextNote.Create(doc, viewId, origin, width, title, options);

            if (bold)
                MakeTextNoteAllBold(newTextNote);

            return newTextNote;
        }

        internal List<TextNote> ProjectInfo(Document doc, ElementId viewId, FontSize fontSize, double xCoord, double yCoord, double spacingAlongY, double colour, double convertToFt, string[] projectData)
        {
            string[] titles = new string[6] { "Date:", "RIBA Workstage:", "Location:", "Floor Area:", "Type:", "Sector:"};
            double[] dataOffsets = new double[6] { 8.58, 28.34, 15.28, 17.6, 8.23, 11.84 };

            double spacing = 1.6 / convertToFt; //The spacing between the title and its data

            List<TextNote> titleTextNotes = new List<TextNote>();
            for (int i = 0; i < titles.Length; i++)
            {
                double xCoordInFt = xCoord / convertToFt;
                double yCoordInFt = (yCoord + (i * spacingAlongY)) / convertToFt;

                //Create the title text note
                XYZ origin = new XYZ(xCoordInFt, yCoordInFt, 0.0);

                TextNote textNoteTitle = CreateTextNote(doc, viewId, origin, fontSize, colour, 110.0 / convertToFt, titles[i], true, false);
                titleTextNotes.Add(textNoteTitle);

                //Create the data text note
                origin = new XYZ((dataOffsets[i] / convertToFt) + xCoordInFt + spacing, yCoordInFt, 0.0);

                TextNote textNoteData = CreateTextNote(doc, viewId, origin, fontSize, 0, 110.0 / convertToFt, projectData[i], false, false);
                titleTextNotes.Add(textNoteData);
            }

            return titleTextNotes;
        }

        internal List<TextNote> AnnotateGraph(Document doc, ElementId viewId, Annotation annotation, double textNoteWidth, double convertToFt, int colour, bool vertical)
        {
            List<TextNote> textNotes = new List<TextNote>();
            for (int i = 0; i < annotation.TextValues.Count; i++)
            {
                TextNote newTextNote = CreateTextNote(doc, viewId, annotation.OriginPoints[i], annotation.TextPointSize[i], colour, textNoteWidth / convertToFt, annotation.TextValues[i], true, vertical);

                textNotes.Add(newTextNote);
            }
            return textNotes;
        }

        internal static void MakeTextNoteAllBold(TextNote textNote)
        {
            FormattedText formattedText = textNote.GetFormattedText();
            formattedText.SetBoldStatus(true);

            textNote.SetFormattedText(formattedText);
        }

        //Finds the best size of font in points based on a given height
        internal static FontSize FindBestTextPointSize(double height, double width, double convertToFt)
        {
            double heightInMM = height * convertToFt;
            double widthInMM = width * convertToFt;
            //Sizes shown in mm (by multiplying convertToFt)
            if (heightInMM < 4.0 | widthInMM < 4.0)
            {
                return FontSize.Six;
            }
            else if (heightInMM < 15.0 | widthInMM < 15.0)
            {
                return FontSize.Eleven;
            }
            else if (heightInMM < 40.0 | widthInMM < 40.0)
            {
                return FontSize.Sixteen;
            }
            else
            {
                return FontSize.Thirty;
            }
        }

        /// <summary>
        /// Sets the characters within a text string to bold
        /// </summary>
        internal static void SetBoldCharacters(TextNote textNote, int startChar, int boldCharCount)
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
