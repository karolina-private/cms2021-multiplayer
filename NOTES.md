# cms2021-multiplayer

Goal: Extensible multiplayer-mod foundation for Car Mechanic Simulator 2021 (PC, Unity IL2CPP).
Owner asked: 2026-09-18.
Status: Standalone TCP host, BepInEx IL2CPP plugin project and tested network-domain foundation are implemented; game adapter is next.
Next: Test the plugin against the owner's specific CMS2021 + BepInEx installation, then implement actual scene/game hooks and remote-avatar rendering.
Build: `./build.sh` (requires .NET 8 SDK; plugin targets .NET 6).
Sandbox: sb-cms2021-multiplayer (CT 104); .NET SDK 8.0.425 at `/opt/dotnet/dotnet`.
Decisions:
- Use BepInEx IL2CPP as the target loader because current CMS2021 mod pages support it.
- Keep deterministic multiplayer domain logic independent of Unity/game binaries.
- The transport is an interface, so a later LiteNetLib/Steamworks backend does not leak into game code.
Log:
- 2026-09-18: Red→green tests for joining/leaving players and JSON envelopes (3 passed).
- 2026-09-18: Built BepInEx IL2CPP plugin DLL successfully. Package restore reports NU1603 from BepInEx's upstream bleeding-edge dependency feed; no compiler errors.
- 2026-09-18: Added standalone TCP session host and tested HELLO → WELCOME handshake using a real local TCP client (6 tests passed).
