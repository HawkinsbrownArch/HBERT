using System.Collections;
using System.Collections.Generic;
using CarbonEmissionTool.Services;

namespace CarbonEmissionTool.Models.Charts
{
    public class TreeChartSquares : IEnumerable<Dictionary<string, object>>
    {
        //The dimensions of the tree chart
        private double _width = 164.0;
        private double _height = 112.0;

        private double xAxisLength = 246.0;
        private double yAxisLength = 66.0;

        private List<Dictionary<string, object>> RectangleDictionary { get; }

        /// <summary>
        /// Constructs a new <see cref="TreeChartSquares"/>.
        /// </summary>
        public TreeChartSquares(CarbonDataCache carbonDataCache)
        {
            var widthFt = _width.ToDecimalFeet();
            var heightFt = _height.ToDecimalFeet();

            var ecValuesNormalized = TreeChartSquaresProcessor.NormalizeSizes(carbonDataCache, widthFt, heightFt);

            this.RectangleDictionary = TreeChartSquaresProcessor.Padded(ecValuesNormalized, xAxisLength.ToDecimalFeet(), yAxisLength.ToDecimalFeet(), widthFt, heightFt);
        }

        public IEnumerator<Dictionary<string, object>> GetEnumerator()
        {
            foreach (var rectangle in RectangleDictionary)
            {
                yield return rectangle;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
