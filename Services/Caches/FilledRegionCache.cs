using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;

namespace CarbonEmissionTool.Services.Caches
{
    class FilledRegionCache
    {
        internal static Dictionary<string, FilledRegionType> FilledRegionDictionary(Document doc)
        {
            List<FilledRegionType> filledRegionTypes = new FilteredElementCollector(doc).OfClass(typeof(FilledRegionType)).Cast<FilledRegionType>().ToList();

            Dictionary<string, FilledRegionType> filledRegionDict = new Dictionary<string, FilledRegionType>();
            foreach (FilledRegionType f in filledRegionTypes)
            {
                filledRegionDict[f.Name] = f;
            }

            return filledRegionDict;
        }


        //Generate a dictionary of all the filled region types in the document
        internal static Dictionary<string, FilledRegionType> CreateTypeDictionary(Document doc)
        {
            List<FilledRegionType> filledRegionType = new FilteredElementCollector(doc).OfClass(typeof(FilledRegionType)).Cast<FilledRegionType>().ToList();

            Dictionary<string, FilledRegionType> filledRegionDict = new Dictionary<string, FilledRegionType>();
            for (int i = 0; i < filledRegionType.Count; i++)
            {
                FilledRegionType filledRegion = filledRegionType[i];
                filledRegionDict[filledRegion.Name] = filledRegion;
            }

            return filledRegionDict;
        }

        internal static ElementId GetTypeId(Document doc, string materialKey, Dictionary<string, FilledRegionType> filledRegionTypeDictionary)
        {
            string validatedMaterialKey = materialKey;
            //If the materials key (the name of the filled region) doesn't exist in the document, then create it
            if (!filledRegionTypeDictionary.ContainsKey(validatedMaterialKey))
            {
                JSONColor.LoadJson(); //Load the JSON colour file

                FilledRegionType filledRegion = filledRegionTypeDictionary.First().Value;

                dynamic colourList = JSONColor.JsonArray[materialKey];

                //If the key cant be found then its a new material or not a HB material so default to the Not_Found material
                if (colourList == null)
                {
                    colourList = JSONColor.JsonArray["Not_Found"];
                    validatedMaterialKey = "Not_Found";

                    if (filledRegionTypeDictionary.ContainsKey(validatedMaterialKey))
                        return filledRegionTypeDictionary[validatedMaterialKey].Id;
                }

                List<int> colourRGB = colourList.ToObject<List<int>>();

                FilledRegionType newFilledRegionType = (FilledRegionType)filledRegion.Duplicate(validatedMaterialKey);
                Color newColor = new Color(Byte.Parse(colourRGB[0].ToString()), Byte.Parse(colourRGB[1].ToString()), Byte.Parse(colourRGB[2].ToString()));

                newFilledRegionType.Color = newColor;
                newFilledRegionType.FillPatternId = new ElementId(3);

                filledRegionTypeDictionary[validatedMaterialKey] = newFilledRegionType;
            }

            return filledRegionTypeDictionary[validatedMaterialKey].Id;
        }
    }
}
