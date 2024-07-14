# Version Check Mod for 7 Days to Die

This mod for 7 Days to Die checks for version mismatches between the game and the mod, and provides optional features to enhance the game experience.

## Features

- Version mismatch detection: Compares the game version with the mod version and displays a warning if they don't match.
- Configurable error messages: Customize the title and description of the version mismatch warning.
- Option to disable the news screen: Skip the news screen on game startup.

## Installation

1. Download the latest release of the mod from the [Releases](https://github.com/yourusername/versioncheckmod/releases) page.
2. Extract the contents of the zip file into your Mod's folder (not the Mod's folder, but the folder your mod files are in)
3. Ensure that the `versioncheck.xml` file is present in the folder and configured.

## Configuration

The mod is configured using the `versioncheck.xml` file. You can customize the following settings:

- `ModVersion`: Set this to the version of the game that the mod is designed for.
- `DisableNewsScreen`: Set to `true` to skip the news screen on game startup.
- `ErrorMessage`:
    - `Title`: Customize the title of the version mismatch warning.
    - `Description`: Customize the description of the version mismatch warning.

Example configuration:

```xml
<?xml version="1.0" encoding="UTF-8"?>
<xml>
    <!-- Configuration for the Version Check -->
    <Settings>
        <!-- Set this to the version of the game you're modding -->
        <!-- This should match the version in the game's build version which is visible in the main menu (top right) -->
        <!-- Example: b316 would be <ModVersion>1.0.316</ModVersion> -->
        <ModVersion>1.0.316</ModVersion>

        <!-- Set to true to disable the news screen on game start -->
        <!-- Set to false to keep the default behavior -->
        <DisableNewsScreen>false</DisableNewsScreen>

        <!-- Customize the version mismatch error message -->
        <ErrorMessage>
            <!-- Title of the error message box -->
            <Title>Version Mismatch Detected</Title>
            <!-- Description of the error. Use {0} for game version and {1} for mod version -->
            <Description>The game version ({0}) does not match the mod version ({1}). This may cause issues.</Description>
        </ErrorMessage>
    </Settings>
</xml>
```

## Usage

1. Start 7 Days to Die with the mod installed.
2. If there's a version mismatch between the game and the mod, you'll see a warning message when entering the main menu.
3. The news screen will be skipped if you've set `DisableNewsScreen` to `true` in the configuration.

## Compatibility

This mod is designed to work with 7 Days to Die version 1.0. It may work with other versions, but compatibility is not guaranteed.
You're free to modify this to work with your mod. It should require little if any changes between game versions.


# TODO

Localization

XML conditional version
