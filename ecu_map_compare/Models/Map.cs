namespace ecu_map_compare.Models
{
    /// <summary>
    /// Represents an ECU map containing a collection of ECU settings.
    /// Each map has a name and a list of ECU settings items.
    /// </summary>
    public class Map
    {
        /// <summary>
        /// The name of the ECU map, typically derived from the filename.
        /// </summary>
        public string name = string.Empty;

        /// <summary>
        /// Collection of ECU settings items contained in this map.
        /// Can be null if the map has no settings or failed to load.
        /// </summary>
        public List<EcuSettingsItem>? EcuSettingsItem;
    }
}
