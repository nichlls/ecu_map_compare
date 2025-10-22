using System.Xml.Linq;
using ecu_map_compare.Models;
using ecu_map_compare.Services;
using ecu_map_compare.Utils;

namespace ecu_map_compare
{
    /// <summary>
    /// Main entry point for the ECU Map Comparison application.
    /// This application compares ECU map files and displays differences between them.
    /// </summary>
    internal static class Program
    {
        /// <summary>
        /// Main entry point of the application.
        /// Loads ECU map files from the maps directory and compares them to identify differences.
        /// </summary>
        /// <param name="args">Command line arguments (currently unused)</param>
        private static void Main(string[] args)
        {
            var mapService = new MapService();

            // Initialize and discover map files in the maps directory
            string[] mapFiles = mapService.InitialiseMaps();

            // Load and parse XML documents for each map file
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

            // Convert XML documents to Map objects with ECU settings
            var mapList = new List<Map>();
            try
            {
                //mapList = mapService.LoadEcuSettingsItems(maps);
                mapService.LoadXmlNodes(maps);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading EcuSettingsItems: {ex.Message}");
                return;
            }

            // Perform comparison between all maps
            CompareMaps(mapList);

            /// <summary>
            /// Compares all maps against the first map (baseline) and displays differences.
            /// Uses the first map as the reference point and compares all subsequent maps against it.
            /// </summary>
            /// <param name="maps">List of Map objects to compare</param>
            void CompareMaps(List<Map> maps)
            {
                // Use the first map as the baseline for all comparisons
                var baseMap = maps.FirstOrDefault();
                if (baseMap == null || baseMap.EcuSettingsItem == null)
                {
                    OutputHandler.PrintError("No maps or base map has no ECU items.");
                    return;
                }

                // Compare each subsequent map against the baseline
                for (int i = 1; i < maps.Count; i++)
                {
                    var currentMap = maps[i];
                    if (currentMap.EcuSettingsItem == null)
                        continue;

                    // Display comparison header
                    OutputHandler.PrintComparingHeader(baseMap.name, currentMap.name);

                    // Collect all differences between baseline and current map
                    var diffs = new List<(string setting, string baseVal, string currVal)>();
                    foreach (var baseItem in baseMap.EcuSettingsItem)
                    {
                        // Find matching setting in current map
                        var matchingItem = currentMap.EcuSettingsItem.FirstOrDefault(item =>
                            item.name == baseItem.name
                        );

                        string baseValStr = baseItem.value?.ToString() ?? "";
                        if (matchingItem == null)
                        {
                            // Setting exists in baseline but not in current map
                            diffs.Add((baseItem.name, baseValStr, "missing"));
                        }
                        else if (baseItem.value != matchingItem.value)
                        {
                            // Setting exists in both but has different values
                            string currValStr = matchingItem.value?.ToString() ?? "";
                            diffs.Add((baseItem.name, baseValStr, currValStr));
                        }
                    }

                    // Display results
                    if (!diffs.Any())
                    {
                        OutputHandler.PrintNoDifferences();
                        continue;
                    }

                    // Sort differences alphabetically by setting name
                    diffs = diffs.OrderBy(d => d.setting).ToList();

                    // Display formatted comparison table
                    OutputHandler.PrintComparisonTable(baseMap.name, currentMap.name, diffs);
                }
            }
        }
    }
}
