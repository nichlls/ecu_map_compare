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

            CompareMaps(mapList);

            void CompareMaps(List<Map> maps)
            {
                // Assuming all maps are compared to the first map (as a baseline)
                var baseMap = maps.FirstOrDefault();
                if (baseMap == null || baseMap.EcuSettingsItem == null)
                {
                    Console.WriteLine("No maps or base map has no ECU items.");
                    return;
                }

                for (int i = 1; i < maps.Count; i++)
                {
                    var currentMap = maps[i];
                    if (currentMap.EcuSettingsItem == null)
                        continue;

                    Console.WriteLine($"Comparing '{baseMap.name}' with '{currentMap.name}':");

                    foreach (var baseItem in baseMap.EcuSettingsItem)
                    {
                        var matchingItem = currentMap.EcuSettingsItem.FirstOrDefault(item =>
                            item.name == baseItem.name
                        );

                        if (matchingItem == null)
                        {
                            Console.WriteLine(
                                $"- '{baseItem.name}' missing in '{currentMap.name}'"
                            );
                        }
                        else if (baseItem.value != matchingItem.value)
                        {
                            Console.WriteLine(
                                $"- Difference in '{baseItem.name}': '{baseMap.name}' = {baseItem.value}, '{currentMap.name}' = {matchingItem.value}"
                            );
                        }
                    }
                }
            }
        }
    }
}
