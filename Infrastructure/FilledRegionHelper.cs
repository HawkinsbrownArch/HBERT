using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using HBERT_UI.Infrastructure;

namespace HBERT.Infrastructure
{
    class FilledRegionHelper
    {
        internal static Dictionary<string, FilledRegionType> FilledRegionDictionary(Document doc)
        {
            List<FilledRegionType> filledRegionTypes = new FilteredElementCollector(doc).OfClass(typeof(FilledRegionType)).Cast<FilledRegionType>().ToList();

            Dictionary<string, FilledRegionType> filledRegionDict = new Dictionary<string, FilledRegionType>();
            foreach(FilledRegionType f in filledRegionTypes)
            {
                filledRegionDict[f.Name] = f;
            }

            return filledRegionDict;
        }

        internal List<CurveLoop> GenerateCurveLoop(Dictionary<string, object> rectangle, double width, double height, double smallCurveTolerance, out XYZ ptTopLeft)
        {
            XYZ origin = new XYZ((double)rectangle["x"], (double)rectangle["y"], 0.0);

            Transform transformX = Transform.CreateTranslation(new XYZ(width, 0.0, 0.0));
            Transform transformY = Transform.CreateTranslation(new XYZ(0.0, height, 0.0));

            XYZ ptBottomRight = transformX.OfPoint(origin);
            XYZ ptTopRight = transformY.OfPoint(ptBottomRight);
            ptTopLeft = new XYZ(origin.X, ptTopRight.Y, 0.0);

            List<XYZ> cornerPoints = new List<XYZ> { origin, ptBottomRight, ptTopRight, ptTopLeft };

            if(origin.DistanceTo(ptTopLeft) < smallCurveTolerance | origin.DistanceTo(ptBottomRight) < smallCurveTolerance)
            {
                return null;
            }
            else
            {
                CurveLoop curveLoop = new CurveLoop();
                for (int i = 0; i < cornerPoints.Count; i++)
                {
                    XYZ ptStart = cornerPoints[i];
                    XYZ ptEnd = cornerPoints[i + 1 == cornerPoints.Count ? 0 : i + 1];

                    Line lnEdge = Line.CreateBound(ptStart, ptEnd);

                    curveLoop.Append(lnEdge);
                }

                return new List<CurveLoop> { curveLoop };
            }
        }

        internal FilledRegion GenerateCurveLoop(Document doc, List<XYZ> cornerPoints, XYZ chartOrigin, Dictionary<string, FilledRegionType> filledRegionTypeDictionary, ElementId newDrawingId, ElementId invisibleLinesId, string matertialKey)
        {
            CurveLoop curveLoop = new CurveLoop();
            for (int i = 0; i < cornerPoints.Count; i++)
            {
                XYZ ptStart = cornerPoints[i];
                XYZ ptEnd = cornerPoints[i + 1 == cornerPoints.Count ? 0 : i + 1];
                
                Line lnEdge = Line.CreateBound(ptStart, ptEnd);

                curveLoop.Append(lnEdge);
            }

            ElementId typeId = GetTypeId(doc, matertialKey, filledRegionTypeDictionary);

            FilledRegion filledRegion = FilledRegion.Create(doc, typeId, newDrawingId, new List<CurveLoop> { curveLoop } );
            filledRegion.SetLineStyleId(invisibleLinesId);

            return filledRegion;
        }

        internal static FillPatternElement GetSolidFillPattern(Document doc)
        {
            List<FillPatternElement> filledRegionType = new FilteredElementCollector(doc).OfClass(typeof(FillPatternElement)).Cast<FillPatternElement>().ToList();

            for (int i = 0; i < filledRegionType.Count; i++)
            {
                FillPatternElement currentFillPattern = filledRegionType[i];
                if (currentFillPattern.GetFillPattern().IsSolidFill)
                {
                    return currentFillPattern;
                }
            }
            return filledRegionType.First();
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

        //Generate a dictionary of all the filled region types in the document
        internal static Dictionary<string, FilledRegionType> CreateTypeDictionary(Document doc)
        {
            List<FilledRegionType> filledRegionType = new FilteredElementCollector(doc).OfClass(typeof(FilledRegionType)).Cast<FilledRegionType>().ToList();

            Dictionary<string, FilledRegionType> filledRegionDict = new Dictionary<string, FilledRegionType>();
            for(int i = 0; i < filledRegionType.Count; i++)
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
                Color newColor = new Color(byte.Parse(colourRGB[0].ToString()), byte.Parse(colourRGB[1].ToString()), byte.Parse(colourRGB[2].ToString()));

                newFilledRegionType.Color = newColor;
                newFilledRegionType.FillPatternId = new ElementId(3);

                filledRegionTypeDictionary[validatedMaterialKey] = newFilledRegionType;
            }

            return filledRegionTypeDictionary[validatedMaterialKey].Id;

        }

        //Returns the invisible line style element ID so charts have no boarders
        internal static ElementId GetInvisibleLineStyleId(Document doc)
        {
            List<GraphicsStyle> lineStyles = new FilteredElementCollector(doc).OfClass(typeof(GraphicsStyle)).Cast<GraphicsStyle>().ToList();

            GraphicsStyle invisibleGraphicStyle = null;
            foreach (GraphicsStyle lStyle in lineStyles)
            {
                if (lStyle.Name == "<Invisible lines>")
                {
                    invisibleGraphicStyle = lStyle;
                    break;
                }
            }
            return invisibleGraphicStyle.Id;
        }
    }
}
