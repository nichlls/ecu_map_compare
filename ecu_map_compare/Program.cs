using System.Text;
using System.Xml.Linq;

internal class Program
{
    public class Map
    {
        public string name = string.Empty;
        public List<EcuSettingsItem>? EcuSettingsItem;
    }

    public class EcuSettingsItem
    {
        public string name = string.Empty;
        public string? value;
    }

    public class DynamicTables { }

    private static void Main(string[] args)
    {
        string map1Filename =
            @"C:\Users\Matthew\source\repos\ecu_map_compare\ecu_map_compare\map1.MaxxECU-save";
        string map2Filename =
            @"C:\Users\Matthew\source\repos\ecu_map_compare\ecu_map_compare\map2.MaxxECU-save";

        // Load maps
        var maps = new List<(string Name, XDocument xmlDoc)>();
        try
        {
            maps.Add(("map1", LoadMap(map1Filename)));
            maps.Add(("map2", LoadMap(map2Filename)));
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
            mapList = LoadEcuSettingsItems(maps);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading EcuSettingsItems: {ex.Message}");
            return;
        }

        static XDocument LoadMap(string filename)
        {
            // MaxxECU specifies utf-16, but saves as utf-8
            string xmlContent = File.ReadAllText(filename, Encoding.UTF8);
            return XDocument.Parse(xmlContent);
        }

        static List<Map> LoadEcuSettingsItems(List<(string Name, XDocument xmlDoc)> maps)
        {
            var tempList = new List<Map>();

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
