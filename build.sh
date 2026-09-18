#!/usr/bin/env sh
set -eu

dotnet test tests/Cms2021Multiplayer.Tests/Cms2021Multiplayer.Tests.csproj --nologo
dotnet build src/Cms2021Multiplayer.Mod/Cms2021Multiplayer.Mod.csproj --nologo
