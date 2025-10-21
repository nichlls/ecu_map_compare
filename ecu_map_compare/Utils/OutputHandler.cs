namespace ecu_map_compare.Utils
{
    public static class OutputHandler
    {
        public static void PrintError(string message)
        {
            Console.WriteLine(message);
        }

        public static void PrintComparingHeader(string baseName, string currentName)
        {
            Console.WriteLine($"Comparing '{baseName}' with '{currentName}':\n");
        }

        public static void PrintNoDifferences()
        {
            Console.WriteLine("No differences found.");
            Console.WriteLine();
        }

        public static void PrintComparisonTable(
            string baseName,
            string currentName,
            List<(string setting, string baseVal, string currVal)> diffs
        )
        {
            // Calculate column widths
            int settingWidth = Math.Max(
                "ECU Setting Name".Length,
                diffs.Max(d => d.setting.Length)
            );
            int baseWidth = Math.Max(baseName.Length, diffs.Max(d => d.baseVal.Length));
            int currWidth = Math.Max(currentName.Length, diffs.Max(d => d.currVal.Length));

            // Print header
            Console.WriteLine(
                PadRight("ECU Setting Name", settingWidth)
                    + " | "
                    + PadRight(baseName, baseWidth)
                    + " | "
                    + PadRight(currentName, currWidth)
            );

            // Print separator
            Console.WriteLine(
                new string('-', settingWidth)
                    + " | "
                    + new string('-', baseWidth)
                    + " | "
                    + new string('-', currWidth)
            );

            // Print rows
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

        private static string PadRight(string s, int width) => s.PadRight(width);
    }
}
