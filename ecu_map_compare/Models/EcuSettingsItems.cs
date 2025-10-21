namespace ecu_map_compare.Models
{
    /// <summary>
    /// Represents a single ECU setting item with a name and value.
    /// Each setting corresponds to a specific ECU parameter or configuration.
    /// </summary>
    public class EcuSettingsItem
    {
        /// <summary>
        /// The name of the ECU setting, which identifies the specific parameter.
        /// </summary>
        public string name = string.Empty;

        /// <summary>
        /// The value of the ECU setting, stored as a string.
        /// Defaults to "0" if no value is specified.
        /// </summary>
        public string value = "0";
    }
}
