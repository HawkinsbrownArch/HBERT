using Autodesk.Revit.DB;
using CarbonEmissionTool.Model.Enums;
using CarbonEmissionTool.Model.Extensions;
using CarbonEmissionTool.Model.Graphics;
using CarbonEmissionTool.Model.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using CarbonEmissionTool.Settings;

namespace CarbonEmissionTool.Services.Caches
{
    public class TextStyleCache
    {
        private Dictionary<string, ElementType> TextStyleDictionary { get; }

        /// <summary>
        /// Constructs a new <see cref="TextStyleCache"/>.
        /// </summary>
        public TextStyleCache()
        {
            this.TextStyleDictionary = new Dictionary<string, ElementType>();
            
            this.PopulateStyleDictionary();
        }

        /// <summary>
        /// Returns a <see cref="ElementType"/> by name. If the name doesn't exist returns null.
        /// </summary>
        public ElementType GetByName(string textStyleName)
        {
            if (this.TextStyleDictionary.ContainsKey(textStyleName))
                return this.TextStyleDictionary[textStyleName];

            return null;
        }

        /// <summary>
        /// Populates the <see cref="TextStyleDictionary"/>.
        /// </summary>
        private void PopulateStyleDictionary()
        {
            var doc = ApplicationServices.Document;

            var defaultTextNoteType = new FilteredElementCollector(doc).OfClass(typeof(TextNoteType)).Cast<TextNoteType>().First();

            var allTextNoteTypes = new FilteredElementCollector(doc).OfClass(typeof(TextNoteType))
                .WhereElementIsElementType().Cast<TextNoteType>().ToList();

            string name = "HBA_";
            var fontSizes = new[] { FontSize.FortyFive, FontSize.Thirty, FontSize.Sixteen, FontSize.Eleven, FontSize.Ten, FontSize.Six };

            var colors = new[] { AnnotationColors.Red, AnnotationColors.Black, AnnotationColors.White };

            foreach (var fontSize in fontSizes)
            {
                double fontSizeFt = (Convert.ToDouble(fontSize) / ApplicationSettings.ConvertPointToMm).ToDecimalFeet();

                for (var c = 0; c < colors.Length; c++)
                {
                    var color = colors[c];
                    string newName = $"{name}{fontSize.ToString()}_{color.ToString()}";

                    var elementTypeExists = allTextNoteTypes.Find(tn => tn.Name == newName);

                    //Check to see if the text note style already exists
                    if (elementTypeExists != null)
                    {
                        this.TextStyleDictionary[newName] = elementTypeExists;
                    }
                    else //Create the new note style if it doesn't exist
                    {
                        var newType = defaultTextNoteType.Duplicate(newName);

                        ApplicationServices.Document.Regenerate();

                        newType.get_Parameter(BuiltInParameter.LINE_COLOR).Set(c);
                            
                        var parameters = new List<BuiltInParameter> { BuiltInParameter.LINE_COLOR, BuiltInParameter.TEXT_SIZE, BuiltInParameter.TEXT_BACKGROUND };
                        List<dynamic> values = new List<dynamic> { color, fontSizeFt, 1 };

                        ParameterUtils.SetParameters(newType, parameters, values);

                        BuiltInParameter fontType = BuiltInParameter.TEXT_FONT;
                        try 
                        {
                            // Try setting the font to the default HBA font.
                            newType.get_Parameter(fontType).Set(ApplicationSettings.HawkinsBrownFont);
                        }
                        catch 
                        {
                            // If it fails then default to arial (probably because the font isn't installed).
                            newType.get_Parameter(fontType).Set(ApplicationSettings.FontDefault);
                        }

                        this.TextStyleDictionary[newName] = newType;
                    }
                }
            }
        }
    }
}