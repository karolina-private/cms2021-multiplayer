# First Windows test

This package checks that the CMS2021 installation can load the BepInEx IL2CPP plugin. It does **not** yet create a multiplayer lobby or a visible remote player.

## 1. Install the mod loader

1. Close Car Mechanic Simulator 2021 and Steam.
2. Download the current **BepInEx 6 Unity IL2CPP Windows x64** build from the official BepInEx installation guide.
3. Extract its contents into the game folder — the folder containing `Car Mechanic Simulator 2021.exe`.
4. Start the game once, wait at the main menu, then exit. BepInEx should create its folders and log files.

Official guide: <https://docs.bepinex.dev/master/articles/user_guide/installation/unity_il2cpp.html>

## 2. Install the test plugin

Copy both DLL files from this archive into:

`<CMS2021 game folder>/BepInEx/plugins/Cms2021Multiplayer/`

Do not copy the archive itself into `plugins`.

## 3. Verify

Start the game and exit after the main menu appears. Open:

`<CMS2021 game folder>/BepInEx/LogOutput.log`

Search for:

`CMS2021 multiplayer foundation loaded`

Send that matching line plus roughly 30 lines before and after it. If the game crashes, send the last 100 lines of the log instead.

## Safety

Use a copy of a save for any future multiplayer experiment. The current test plugin does not alter save data.
