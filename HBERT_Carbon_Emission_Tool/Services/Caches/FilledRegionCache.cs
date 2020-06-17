using Autodesk.Revit.DB;
using CarbonEmissionTool.Model.Collectors;
using System;
using System.Collections.Generic;
using System.Linq;
using CarbonEmissionTool.Settings;

namespace CarbonEmissionTool.Services.Caches
{
    public class FilledRegionCache
    {
        private ChartColorCache _colorCache;

        private Dictionary<string, FilledRegionType> FilledRegionDictionary { get; }

        /// <summary>
        /// Creates a new <see cref="FilledRegionCache"/>.
        /// </summary>
        public FilledRegionCache(ChartColorCache colorCache)
        {
            this.PopulateCache();

            this.CreateChartTypes(colorCache);

            _colorCache = colorCache;
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
        /// Generates all filled regions from the <see cref="ChartColorCache"/> and adds them to the
        /// <see cref="FilledRegionDictionary"/>.
        /// </summary>
        public void CreateChartTypes(ChartColorCache chartColorCache)
        {
            FillPatternElement fillPattern = FillPatternFilter.GetSolidFillPattern();
            
            foreach (var chartColor in chartColorCache)
            {
                if (!this.FilledRegionDictionary.ContainsKey(chartColor.Name))
                {
                    var newFilledRegionType = (FilledRegionType)this.FilledRegionDictionary.First().Value.Duplicate(chartColor.Name);

                    var color = chartColor.Color;

                    var newColor = new Color(color.R, color.G, color.B);

                    newFilledRegionType.Color = newColor;
                    newFilledRegionType.FillPatternId = fillPattern.Id;

                    this.FilledRegionDictionary[chartColor.Name] = newFilledRegionType;
                }
            }
        }

        /// <summary>
        /// Returns a <see cref="FilledRegionType"/> by name.
        /// </summary>
        public FilledRegionType GetByName(string materialName)
        {
            // If the key cant be found then its a new material or not a HB material so default to the Not_Found material.
            if (!_colorCache.Any(c => c.Name == materialName))
                return this.FilledRegionDictionary[ApplicationSettings.NotFoundColorName];

            return this.FilledRegionDictionary[materialName];
        }
    }
}
