using ecu_map_compare.Models;

namespace ecu_map_compare.tests
{
    public class EcuSettingsItemModelTests
    {
        [Fact]
        public void EcuSettingsItem_Constructor_InitializesWithDefaultValues()
        {
            // Arrange & Act
            var item = new EcuSettingsItem();

            // Assert
            Assert.Equal(string.Empty, item.name);
            Assert.Equal("0", item.value);
        }

        [Fact]
        public void EcuSettingsItem_WithCustomValues_PropertiesSetCorrectly()
        {
            // Arrange & Act
            var item = new EcuSettingsItem { name = "TestSetting", value = "TestValue" };

            // Assert
            Assert.Equal("TestSetting", item.name);
            Assert.Equal("TestValue", item.value);
        }
    }
}
