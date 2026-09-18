using Cms2021Multiplayer.Networking;
using Xunit;

namespace Cms2021Multiplayer.Tests;

public sealed class NetworkEnvelopeTests
{
    [Fact]
    public void Round_trip_preserves_message_metadata_and_payload()
    {
        var sent = new NetworkEnvelope(
            MessageKind.PlayerJoined,
            Guid.Parse("b90fc4aa-1015-4444-9999-0123456789ab"),
            "{\"displayName\":\"Alice\"}");

        var received = NetworkEnvelope.Deserialize(sent.Serialize());

        Assert.Equal(sent, received);
    }
}
