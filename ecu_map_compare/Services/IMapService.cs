using System.Xml.Linq;
using ecu_map_compare.Models;

namespace ecu_map_compare.Services
{
    /// <summary>
    /// Interface defining the contract for ECU map file operations.
    /// Provides methods for loading, discovering, and parsing ECU map files.
    /// </summary>
    public interface IMapService
    {
        /// <summary>
        /// Loads and parses an ECU map file from the specified path.
        /// </summary>
        /// <param name="filename">Path to the ECU map file</param>
        /// <returns>Parsed XDocument containing the map data</returns>
        public XDocument LoadMap(string filename);

        /// <summary>
        /// Discovers and initializes all ECU map files in the maps directory.
        /// </summary>
        /// <returns>Array of file paths to discovered map files</returns>
        string[] InitialiseMaps();

        /// <summary>
        /// Converts XML documents to Map objects by extracting ECU settings.
        /// </summary>
        /// <param name="maps">List of tuples containing map names and their corresponding XML documents</param>
        /// <returns>List of Map objects with populated ECU settings</returns>
        List<Map> LoadEcuSettingsItems(List<(string Name, XDocument xmlDoc)> maps);
    }
}
