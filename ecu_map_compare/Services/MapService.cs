using System.Text;
using System.Xml.Linq;
using ecu_map_compare.Models;

namespace ecu_map_compare.Services
{
    /// <summary>
    /// Service class responsible for loading and parsing ECU map files.
    /// Handles XML parsing, file discovery, and conversion to Map objects.
    /// </summary>
    public class MapService : IMapService
    {
        /// <summary>
        /// Loads and parses an ECU map file from the specified path.
        /// Handles the specific encoding requirements of MaxxECU files.
        /// </summary>
        /// <param name="filename">Path to the ECU map file</param>
        /// <returns>Parsed XDocument containing the map data</returns>
        public XDocument LoadMap(string filename)
        {
            // MaxxECU specifies utf-16 in the XML declaration but actually saves files as utf-8
            // This method handles the encoding discrepancy by reading as UTF-8
            string xmlContent = File.ReadAllText(filename, Encoding.UTF8);
            return XDocument.Parse(xmlContent);
        }

        /// <summary>
        /// Discovers and initializes all ECU map files in the maps directory.
        /// Creates the maps directory if it doesn't exist and returns all .MaxxECU-save files.
        /// </summary>
        /// <returns>Array of file paths to discovered map files</returns>
        /// <exception cref="Exception">Thrown when unable to determine the project base directory</exception>
        public string[] InitialiseMaps()
        {
            // Navigate up from the current directory to find the project root
            // Current directory is typically in bin/Debug/net8.0, so we go up 3 levels
            string? baseDirectory =
                (Directory.GetParent(Environment.CurrentDirectory)?.Parent?.Parent?.FullName)
                ?? throw new Exception("Failed to get current base directory.");

            string mapsDirectory = Path.Combine(baseDirectory, "maps");

            // Create the maps directory if it doesn't exist
            if (!Directory.Exists(mapsDirectory))
            {
                System.IO.Directory.CreateDirectory(mapsDirectory);
            }

            // Find all ECU map files with the .MaxxECU-save extension
            string[] mapFiles = Directory.GetFiles(mapsDirectory, "*.MaxxECU-save");
            return mapFiles;
        }

        /// <summary>
        /// Converts XML documents to Map objects by extracting ECU settings.
        /// Parses the XML structure to find ECUSettingsItem elements and creates corresponding objects.
        /// </summary>
        /// <param name="maps">List of tuples containing map names and their corresponding XML documents</param>
        /// <returns>List of Map objects with populated ECU settings</returns>
        public List<Map> LoadEcuSettingsItems(List<(string Name, XDocument xmlDoc)> maps)
        {
            List<Map> tempList = [];

            foreach (var (name, xmlDoc) in maps)
            {
                // Extract all ECUSettingsItem elements from the XML document
                var settings = xmlDoc
                    .Descendants("ECUSettingsItem")
                    .Select(x => new EcuSettingsItem
                    {
                        name = x.Attribute("name")?.Value ?? string.Empty,
                        value = x.Value,
                    })
                    .ToList();

                // Create a Map object with the extracted settings
                tempList.Add(new Map { name = name, EcuSettingsItem = settings });
            }

            return tempList;
        }
    }
}
