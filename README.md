# CMS2021 Multiplayer Foundation

An extensible, **unofficial** multiplayer foundation for the PC version of *Car Mechanic Simulator 2021*.

## Scope of this first foundation

- host-authoritative session model;
- lobby/player registry;
- JSON message contracts and validation;
- transport abstraction (the networking backend can be replaced);
- a standalone TCP session host for LAN/Tailscale testing;
- player-presence snapshots (name, position and rotation) for remote avatar rendering;
- Unity/BepInEx adapter seam kept separate from network-domain code;

This is a technical foundation, not a complete online-co-op conversion. CMS2021's game-specific save, vehicle and gameplay hooks require testing against the owner's installed game build and its IL2CPP assemblies.

## LAN host test

Run the standalone host on the machine that will host the session:

```bash
dotnet run --project src/Cms2021Multiplayer.Server -- 31337
```

It listens on TCP port `31337`. On the same LAN, clients will use the host computer's local IP. Across the internet, prefer Tailscale first; do not expose this development protocol publicly yet.

## Planned mod layout

The release will contain a BepInEx IL2CPP plugin DLL. Install it in:

`Car Mechanic Simulator 2021/BepInEx/plugins/`

## Development

```bash
dotnet test
```

The core library is deliberately independent from Unity and BepInEx, so networking rules can be tested outside the game.

## License

MIT
