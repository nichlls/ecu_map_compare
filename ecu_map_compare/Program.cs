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
        public string value = "0";
    }

    public class DynamicTables { }

    private static void Main(string[] args)
    {
        string[] mapFiles = InitialiseMaps();

        // Load maps
        var maps = new List<(string Name, XDocument xmlDoc)>();
        try
        {
            foreach (var map in mapFiles)
            {
                var xmlDoc = LoadMap(map);
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

    // Collect all maps in /maps directory to a string array
    private static string[] InitialiseMaps()
    {
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
}
