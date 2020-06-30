using CarbonEmissionTool.Models;
using System.Collections;
using System.Collections.Generic;

namespace CarbonEmissionTool.Services
{
    public class CarbonDataCache : IEnumerable<CarbonData>
    {
        private List<CarbonData> CarbonDataList { get; }

        public bool IsEmpty => CarbonDataList.Count == 0;

        /// <summary>
        /// Constructs a new <see cref="CarbonDataCache"/>.
        /// </summary>
        public CarbonDataCache()
        {
            this.CarbonDataList = CarbonDataProcessor.Process();
        }

        public IEnumerator<CarbonData> GetEnumerator()
        {
            foreach (var carbonData in CarbonDataList)
            {
                yield return carbonData;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
