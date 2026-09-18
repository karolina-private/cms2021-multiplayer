using System.Text.Json;

namespace Cms2021Multiplayer.Networking;

public enum MessageKind
{
    Hello,
    Welcome,
    PlayerJoined,
    PlayerLeft,
    PlayerTransform,
    GarageSnapshot,
    VehicleSnapshot,
    JobUpdated,
    Error
}

public sealed record NetworkEnvelope(MessageKind Kind, Guid SenderId, string Payload)
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    public string Serialize() => JsonSerializer.Serialize(this, JsonOptions);

    public static NetworkEnvelope Deserialize(string json) =>
        JsonSerializer.Deserialize<NetworkEnvelope>(json, JsonOptions)
        ?? throw new InvalidOperationException("Network message was empty.");
}
