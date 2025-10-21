# ECU Map Compare

A simple console application for comparing MaxxECU map files and identifying differences between them.

## Overview

This application loads ECU map files from the `maps` directory and compares them to identify differences in ECU settings. It uses the first map as a baseline and compares all other maps against it.

## Features

- Loads ECU map files with `.MaxxECU-save` extension
- Compares multiple maps against a baseline map
- Displays differences in a formatted table
- Handles missing settings and value differences
- Sorts results alphabetically for consistent output

## Usage

1. Place your MaxxECU map files in the `maps` directory
2. Run the application
3. View the comparison results in the console

## File Structure

```
ecu_map_compare/
├── maps/                   # Directory for ECU map files
├── Models/                 # Data models
│   ├── Map.cs
│   └── EcuSettingsItems.cs
├── Services/               # Business logic
│   ├── IMapService.cs
│   └── MapService.cs
├── Utils/                  # Utility classes
│   └── OutputHandler.cs
└── Program.cs              # Main application entry point
```

## Requirements

- .NET 8.0
- MaxxECU map files with `.MaxxECU-save` extension (non-encrypted - clean XML)

## How It Works

1. The application looks in the `maps` directory for ECU map files
2. Loads and parses the XML content of each map file
3. Extracts ECU settings from the XML structure
4. Compares all maps against the first map (baseline)
5. Displays differences in a formatted table showing:
   - Setting name
   - Baseline value
   - Comparison map value
   - Missing settings (marked as "missing")

## Output Format

The application displays results in a table format:

```
ECU Setting Name          | Baseline Map | Comparison Map
------------------------- | ------------ | -------------
Setting Name              | Value        | Value
Another Setting           | Value        | missing
```

## Error Handling

The application handles common errors:

- Missing maps directory (creates it automatically)
- Invalid XML files
- Missing ECU settings
- File access issues

## Todo

- Handle tables (currently only ECUSettingsItem nodes are checked)
