using Autodesk.Revit.DB;
using CarbonEmissionTool.Model.Collectors;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CarbonEmissionTool.Services.Caches
{
    public class FilledRegionCache
    {
        private Dictionary<string, FilledRegionType> FilledRegionDictionary { get; }

        /// <summary>
        /// Creates a new <see cref="FilledRegionCache"/>.
        /// </summary>
        public FilledRegionCache()
        {
            // Create all the filled regions from the JSON graphics file.
            this.CreateAll();

            this.PopulateCache();
        }
        
        /// <summary>
        /// Populate the cache with all the filled region types in the document.
        /// </summary>
        public void PopulateCache()
        {
            var filledRegionTypes = new FilteredElementCollector(ApplicationServices.Document)
                .OfClass(typeof(FilledRegionType)).WhereElementIsElementType();

            foreach (FilledRegionType filledRegionType in filledRegionTypes)
            {
                this.FilledRegionDictionary[filledRegionType.Name] = filledRegionType;
            }
        }

        /// <summary>
        /// Generates all filled regions from the JSON graphics file.
        /// </summary>
        public void CreateAll()
        {
            FillPatternElement fillPattern = FillPatternFilter.GetSolidFillPattern();

            JSONColor.LoadJson(); //Load the JSON colour file

            foreach (dynamic colourList in JSONColor.JsonArray)
            {
                var materialName = colourList.Name;
                var colourRGB = colourList.Value.ToObject<List<int>>();

                try
                {
                    var newFilledRegionType = (FilledRegionType)filledRegionTypes.First().Value.Duplicate(materialName);

                    var newColor = new Color(byte.Parse(colourRGB[0].ToString()), byte.Parse(colourRGB[1].ToString()), byte.Parse(colourRGB[2].ToString()));

                    newFilledRegionType.Color = newColor;
                    newFilledRegionType.FillPatternId = fillPattern.Id;
                }
                catch
                {
                    //do nothing if the creation fails
                }
            }
        }

        public FilledRegionType GetByName(string materialName)
        {
            var validatedMaterialKey = materialName;

            //If the materials key (the name of the filled region) doesn't exist in the document, then create it
            if (!this.FilledRegionDictionary.ContainsKey(validatedMaterialKey))
            {
                var filledRegion = this.FilledRegionDictionary.First().Value;

                dynamic colourList = JSONColor.JsonArray[materialKey];

                //If the key cant be found then its a new material or not a HB material so default to the Not_Found material
                if (colourList == null)
                {
                    colourList = JSONColor.JsonArray["Not_Found"];
                    validatedMaterialKey = "Not_Found";

                    if (this.FilledRegionDictionary.ContainsKey(validatedMaterialKey))
                        return this.FilledRegionDictionary[validatedMaterialKey];
                }

                var colourRGB = colourList.ToObject<List<int>>();

                var newFilledRegionType = (FilledRegionType)filledRegion.Duplicate(validatedMaterialKey);
                var newColor = new Color(Byte.Parse(colourRGB[0].ToString()), Byte.Parse(colourRGB[1].ToString()), Byte.Parse(colourRGB[2].ToString()));

                newFilledRegionType.Color = newColor;
                newFilledRegionType.FillPatternId = new ElementId(3);

                this.FilledRegionDictionary[validatedMaterialKey] = newFilledRegionType;
            }

            return this.FilledRegionDictionary[validatedMaterialKey];
        }
    }
}
