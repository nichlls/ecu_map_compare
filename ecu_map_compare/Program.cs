using System.Text;
using System.Xml.Linq;
using ecu_map_compare.Models;
using ecu_map_compare.Services;

namespace ecu_map_compare
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var mapService = new MapService();

            string[] mapFiles = mapService.InitialiseMaps();

            // Load maps
            var maps = new List<(string Name, XDocument xmlDoc)>();
            try
            {
                foreach (var map in mapFiles)
                {
                    var xmlDoc = mapService.LoadMap(map);
                    var mapName = Path.GetFileNameWithoutExtension(map);
                    maps.Add((mapName, xmlDoc));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading maps: {ex.Message}");
                return;
            }

            // Store Map objects in list
            var mapList = new List<Map>();
            try
            {
                mapList = mapService.LoadEcuSettingsItems(maps);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading EcuSettingsItems: {ex.Message}");
                return;
            }
        }
    }
}
