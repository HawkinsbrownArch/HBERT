using System;
using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.DB;
using CarbonEmissionTool.Model.Enums;
using CarbonEmissionTool.Model.Extensions;
using CarbonEmissionTool.Model.Graphics;
using CarbonEmissionTool.Model.Utilities;

namespace CarbonEmissionTool.Model.Annotations
{
    class Annotation
    {
        private readonly TextNoteExtensions _textNote;
        private readonly LabelGraph _labelGraph;
        private readonly ProjectInfo _projectInfo;

        //Int 0 = color, int 1 = font size, int 2 = bold
        public static Dictionary<string, ElementType> TextStyleDictionary { get; set; }

        public List<XYZ> OriginPoints { get; set; }

        public List<FontSize> TextPointSize { get; set; }

        public List<string> TextValues { get; set; }

        public TextNote TextNote
        {
            get { return _textNote; }
        }

        public LabelGraph LabelGraph
        {
            get { return _labelGraph; }
        }

        public ProjectInfo ProjectInfo1
        {
            get { return _projectInfo; }
        }

        public Annotation()
        {
            OriginPoints = new List<XYZ>();
            TextPointSize = new List<FontSize>();
            TextValues = new List<string>();
            _textNote = new TextNote();
            _labelGraph = new LabelGraph(this);
            _projectInfo = new ProjectInfo();
        }

        public static void GenerateStyleDictionary(Document doc, double convertToFt, double convertPointToMm, int redHB)
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
            int[] colors = new[] { redHB, ColorUtils.ConvertColourToInt(0, 0, 0), ColorUtils.ConvertColourToInt(254, 254, 254) };

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

                        ParameterUtils.SetParameters(newType, parameters, values);

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
    }
}
