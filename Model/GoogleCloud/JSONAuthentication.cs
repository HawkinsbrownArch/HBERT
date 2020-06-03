using System.IO;
using System.Reflection;
using Newtonsoft.Json;

namespace CarbonEmissionTool.Model.GoogleCloud
{
    class JSONAuthentication
    {
        internal static string thisAssemblyPath = Assembly.GetExecutingAssembly().Location;
        internal static string JsonArray { get; set; }

        public static string LoadJson()
        {
            string jsonConfigFile = Path.GetDirectoryName(thisAssemblyPath) + @"\hbert-json-storage-fc1413b06760.json";
            using (StreamReader r = new StreamReader(jsonConfigFile))
            {
                string jsonFileContents = r.ReadToEnd();
                JsonArray = JsonConvert.DeserializeObject(jsonFileContents).ToString();
                return JsonArray;
            }
        }
    }
}
