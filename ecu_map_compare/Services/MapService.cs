using System.Text;
using System.Xml.Linq;
using ecu_map_compare.Models;

namespace ecu_map_compare.Services
{
    public class MapService : IMapService
    {
        public XDocument LoadMap(string filename)
        {
            // MaxxECU specifies utf-16, but saves as utf-8
            string xmlContent = File.ReadAllText(filename, Encoding.UTF8);
            return XDocument.Parse(xmlContent);
        }

        // Collect all maps in /maps directory to a string array
        public string[] InitialiseMaps()
        {
            // Get project directory
            string? baseDirectory =
                (Directory.GetParent(Environment.CurrentDirectory)?.Parent?.Parent?.FullName)
                ?? throw new Exception("Failed to get current base directory.");

            string mapsDirectory = Path.Combine(baseDirectory, "maps");

            // If maps folder doesn't exist
            if (!Directory.Exists(mapsDirectory))
            {
                System.IO.Directory.CreateDirectory(mapsDirectory);
            }

            // Get all map files in directory
            string[] mapFiles = Directory.GetFiles(mapsDirectory, "*.MaxxECU-save");
            return mapFiles;
        }

        public List<Map> LoadEcuSettingsItems(List<(string Name, XDocument xmlDoc)> maps)
        {
            List<Map> tempList = [];

            foreach (var (name, xmlDoc) in maps)
            {
                var settings = xmlDoc
                    .Descendants("ECUSettingsItem")
                    .Select(x => new EcuSettingsItem
                    {
                        name = x.Attribute("name")?.Value ?? string.Empty,
                        value = x.Value,
                    })
                    .ToList();

                tempList.Add(new Map { name = name, EcuSettingsItem = settings });
            }

            return tempList;
        }
    }
}
