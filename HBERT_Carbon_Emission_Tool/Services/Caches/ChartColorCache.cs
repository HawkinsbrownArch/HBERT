using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows.Media;
using CarbonEmissionTool.Models;
using CarbonEmissionTool.Settings;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CarbonEmissionTool.Services
{
    public class ChartColorCache : IEnumerable<ChartColor>
    {
        private readonly string _assemblyPath = Assembly.GetExecutingAssembly().Location;

        private List<ChartColor> Colors { get; }

        /// <summary>
        /// Constructs a new <see cref="ChartColorCache"/>.
        /// </summary>
        public ChartColorCache()
        {
            this.Colors = new List<ChartColor>();

            var jsonConfigFile = $"{Path.GetDirectoryName(_assemblyPath)}\\{ApplicationSettings.MaterialColorJsonFileName}";

            using (StreamReader r = new StreamReader(jsonConfigFile))
            {
                string jsonFileContents = r.ReadToEnd();
                var colorDictionary = JsonConvert.DeserializeObject<Dictionary<string, JArray>>(jsonFileContents);

                foreach (var graphicsValuePair in colorDictionary)
                {
                    var materialName = graphicsValuePair.Key;

                    var rgbValues = graphicsValuePair.Value;

                    var color = new Color()
                    {
                        A = 255,
                        R = rgbValues[0].ToObject<byte>(),
                        G = rgbValues[1].ToObject<byte>(),
                        B = rgbValues[2].ToObject<byte>(),
                    };

                    var chartColor = new ChartColor(materialName, color);

                    this.Colors.Add(chartColor);
                }
            }
        }

        /// <summary>
        /// Returns a <see cref="ChartColor"/> by name. If the name doesn't exist returns null.
        /// </summary>
        public ChartColor GetByName(string textStyleName)
        {
            return this.Colors.Find(c => c.Name == textStyleName);
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
