using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.DB;
using CarbonEmissionTool.Services.Caches;

namespace CarbonEmissionTool.Model.Utilities
{
    class FilledRegionUtils
    {
        internal FilledRegion FromPoints(Document doc, List<XYZ> cornerPoints, Dictionary<string, FilledRegionType> filledRegionTypeDictionary, ElementId newDrawingId, ElementId invisibleLinesId, string materialKey)
        {
            CurveLoop curveLoop = new CurveLoop();
            for (int i = 0; i < cornerPoints.Count; i++)
            {
                XYZ ptStart = cornerPoints[i];
                XYZ ptEnd = cornerPoints[i + 1 == cornerPoints.Count ? 0 : i + 1];
                
                Line lnEdge = Line.CreateBound(ptStart, ptEnd);

                curveLoop.Append(lnEdge);
            }

            ElementId typeId = FilledRegionCache.GetTypeId(doc, materialKey, filledRegionTypeDictionary);

            FilledRegion filledRegion = FilledRegion.Create(doc, typeId, newDrawingId, new List<CurveLoop> { curveLoop } );
            filledRegion.SetLineStyleId(invisibleLinesId);

            return filledRegion;
        }

        //Generates all filled regions from the JSON file
        internal static void CreateAll(Document doc)
        {
            FillPatternElement fillPattern = GetSolidFillPattern(doc);
            Dictionary<string, FilledRegionType> filledRegionTypes = FilledRegionDictionary(doc);

            JSONColor.LoadJson(); //Load the JSON colour file

            foreach (dynamic colourList in JSONColor.JsonArray)
            {
                string materialName = colourList.Name;
                List<int> colourRGB = colourList.Value.ToObject<List<int>>();

                try
                {
                    FilledRegionType newFilledRegionType = (FilledRegionType)filledRegionTypes.First().Value.Duplicate(materialName);

                    Color newColor = new Color(byte.Parse(colourRGB[0].ToString()), byte.Parse(colourRGB[1].ToString()), byte.Parse(colourRGB[2].ToString()));

                    newFilledRegionType.Color = newColor;
                    newFilledRegionType.FillPatternId = fillPattern.Id;
                }
                catch
                {
                    //do nothing if the creation fails
                }
            }
        }
    }
}
