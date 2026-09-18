using BepInEx;
using BepInEx.Unity.IL2CPP;
using Cms2021Multiplayer.Networking;

namespace Cms2021Multiplayer.Mod;

[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
public sealed class Cms2021MultiplayerPlugin : BasePlugin
{
    public const string PluginGuid = "com.karolina.cms2021multiplayer";
    public const string PluginName = "CMS2021 Multiplayer Foundation";
    public const string PluginVersion = "0.1.0";

    internal static SessionRegistry Sessions { get; } = new();

    public override void Load()
    {
        Log.LogInfo("CMS2021 multiplayer foundation loaded. No game state is synchronized yet.");
    }
}
