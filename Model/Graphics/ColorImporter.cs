using System.IO;
using System.Reflection;
using Newtonsoft.Json;

namespace CarbonEmissionTool.Model.Graphics
{
    class ColorImporter
    {
        private readonly string _assemblyPath = Assembly.GetExecutingAssembly().Location;

        internal dynamic JsonArray { get; set; }

        public void LoadJson()
        {
            string jsonConfigFile = Path.GetDirectoryName(_assemblyPath) + @"\HB_Material_ColourScheme.json";
            using (StreamReader r = new StreamReader(jsonConfigFile))
            {
                string jsonFileContents = r.ReadToEnd();
                JsonArray = JsonConvert.DeserializeObject(jsonFileContents);
            }
        }
    }
}
