namespace Cms2021Multiplayer.Presence;

public sealed record PlayerPresenceSnapshot(
    Guid PlayerId,
    string DisplayName,
    float PositionX,
    float PositionY,
    float PositionZ,
    float YawDegrees);

public sealed class PlayerPresenceTracker
{
    private readonly Dictionary<Guid, PlayerPresenceSnapshot> _remotePlayers = new();

    public IReadOnlyCollection<PlayerPresenceSnapshot> RemotePlayers => _remotePlayers.Values;

    public void Update(PlayerPresenceSnapshot snapshot)
    {
        ArgumentNullException.ThrowIfNull(snapshot);
        _remotePlayers[snapshot.PlayerId] = snapshot;
    }

    public bool Remove(Guid playerId) => _remotePlayers.Remove(playerId);
}
