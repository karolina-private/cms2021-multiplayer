namespace Cms2021Multiplayer.Networking;

public sealed record ConnectedPlayer(Guid PlayerId, string DisplayName);

public sealed class SessionRegistry
{
    private readonly List<ConnectedPlayer> _players = new();

    public IReadOnlyList<ConnectedPlayer> Players => _players;

    public ConnectedPlayer Join(string displayName)
    {
        if (string.IsNullOrWhiteSpace(displayName))
        {
            throw new ArgumentException("A player name is required.", nameof(displayName));
        }

        var player = new ConnectedPlayer(Guid.NewGuid(), displayName.Trim());
        _players.Add(player);
        return player;
    }

    public bool Leave(Guid playerId)
    {
        var player = _players.Find(candidate => candidate.PlayerId == playerId);
        return player is not null && _players.Remove(player);
    }
}
