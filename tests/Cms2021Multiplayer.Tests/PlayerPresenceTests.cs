using Cms2021Multiplayer.Presence;
using Xunit;

namespace Cms2021Multiplayer.Tests;

public sealed class PlayerPresenceTests
{
    [Fact]
    public void Update_applies_latest_remote_position_and_rotation()
    {
        var playerId = Guid.Parse("ce8b9406-ae31-46cd-8d78-0e5f97a6a642");
        var tracker = new PlayerPresenceTracker();

        tracker.Update(new PlayerPresenceSnapshot(playerId, "Alice", 12.5f, 0f, -4.25f, 90f));

        var presence = Assert.Single(tracker.RemotePlayers);
        Assert.Equal(playerId, presence.PlayerId);
        Assert.Equal("Alice", presence.DisplayName);
        Assert.Equal(12.5f, presence.PositionX);
        Assert.Equal(-4.25f, presence.PositionZ);
        Assert.Equal(90f, presence.YawDegrees);
    }
}
