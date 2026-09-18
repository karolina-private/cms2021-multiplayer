using Cms2021Multiplayer.Networking;
using Xunit;

namespace Cms2021Multiplayer.Tests;

public sealed class SessionRegistryTests
{
    [Fact]
    public void Join_assigns_unique_player_ids_and_tracks_connected_players()
    {
        var registry = new SessionRegistry();

        var first = registry.Join("Alice");
        var second = registry.Join("Bob");

        Assert.NotEqual(first.PlayerId, second.PlayerId);
        Assert.Equal(2, registry.Players.Count);
        Assert.Contains(registry.Players, player => player.DisplayName == "Alice");
        Assert.Contains(registry.Players, player => player.DisplayName == "Bob");
    }

    [Fact]
    public void Leave_removes_only_the_requested_player()
    {
        var registry = new SessionRegistry();
        var first = registry.Join("Alice");
        var second = registry.Join("Bob");

        var removed = registry.Leave(first.PlayerId);

        Assert.True(removed);
        Assert.Single(registry.Players);
        Assert.Equal(second.PlayerId, registry.Players[0].PlayerId);
    }
}
