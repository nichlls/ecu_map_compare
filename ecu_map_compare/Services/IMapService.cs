using System.Xml.Linq;
using ecu_map_compare.Models;

namespace ecu_map_compare.Services
{
    public interface IMapService
    {
        public XDocument LoadMap(string filename);
        string[] InitialiseMaps();

        List<Map> LoadEcuSettingsItems(List<(string Name, XDocument xmlDoc)> maps);
    }
}
