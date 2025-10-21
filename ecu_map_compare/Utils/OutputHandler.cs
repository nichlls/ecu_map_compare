namespace ecu_map_compare.Utils
{
    /// <summary>
    /// Utility class for handling console output formatting and display.
    /// Provides methods for displaying comparison results in a consistent, readable format.
    /// </summary>
    public static class OutputHandler
    {
        /// <summary>
        /// Displays an error message to the console.
        /// </summary>
        /// <param name="message">The error message to display</param>
        public static void PrintError(string message)
        {
            Console.WriteLine(message);
        }

        /// <summary>
        /// Displays a header indicating which maps are being compared.
        /// </summary>
        /// <param name="baseName">Name of the baseline map</param>
        /// <param name="currentName">Name of the map being compared against the baseline</param>
        public static void PrintComparingHeader(string baseName, string currentName)
        {
            Console.WriteLine($"Comparing '{baseName}' with '{currentName}':\n");
        }

        /// <summary>
        /// Displays a message indicating no differences were found between maps.
        /// </summary>
        public static void PrintNoDifferences()
        {
            Console.WriteLine("No differences found.");
            Console.WriteLine();
        }

        /// <summary>
        /// Displays a formatted comparison table showing differences between two maps.
        /// The table includes setting names and their values in both maps, with proper column alignment.
        /// </summary>
        /// <param name="baseName">Name of the baseline map</param>
        /// <param name="currentName">Name of the map being compared</param>
        /// <param name="diffs">List of differences containing setting name, baseline value, and current value</param>
        public static void PrintComparisonTable(
            string baseName,
            string currentName,
            List<(string setting, string baseVal, string currVal)> diffs
        )
        {
            // Calculate optimal column widths based on content
            int settingWidth = Math.Max(
                "ECU Setting Name".Length,
                diffs.Max(d => d.setting.Length)
            );
            int baseWidth = Math.Max(baseName.Length, diffs.Max(d => d.baseVal.Length));
            int currWidth = Math.Max(currentName.Length, diffs.Max(d => d.currVal.Length));

            // Print table header with column names
            Console.WriteLine(
                PadRight("ECU Setting Name", settingWidth)
                    + " | "
                    + PadRight(baseName, baseWidth)
                    + " | "
                    + PadRight(currentName, currWidth)
            );

            // Print separator line for visual clarity
            Console.WriteLine(
                new string('-', settingWidth)
                    + " | "
                    + new string('-', baseWidth)
                    + " | "
                    + new string('-', currWidth)
            );

            // Print each difference as a table row
            foreach (var diff in diffs)
            {
                Console.WriteLine(
                    PadRight(diff.setting, settingWidth)
                        + " | "
                        + PadRight(diff.baseVal, baseWidth)
                        + " | "
                        + PadRight(diff.currVal, currWidth)
                );
            }

            Console.WriteLine();
        }

        /// <summary>
        /// Helper method to pad a string to the right with spaces to achieve consistent column width.
        /// </summary>
        /// <param name="s">The string to pad</param>
        /// <param name="width">The target width</param>
        /// <returns>The padded string</returns>
        private static string PadRight(string s, int width) => s.PadRight(width);
    }
}
