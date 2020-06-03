using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarbonEmissionTool.Model.Graphics;

namespace CarbonEmissionTool.Services.Caches
{
    class ChartColorCache : IEnumerable<ChartColor>
    {
        private List<ChartColor> Colors { get; }

        public ChartColorCache()
        {
            // todo: instantiate colours from JSON
        }

        public IEnumerator<ChartColor> GetEnumerator()
        {
            foreach (var chartColor in this.Colors)
            {
                yield return chartColor;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
