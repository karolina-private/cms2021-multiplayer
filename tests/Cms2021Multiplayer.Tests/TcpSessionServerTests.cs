using System.Net.Sockets;
using System.Text.Json;
using Cms2021Multiplayer.Networking;
using Cms2021Multiplayer.Server;
using Xunit;

namespace Cms2021Multiplayer.Tests;

public sealed class TcpSessionServerTests
{
    [Fact]
    public async Task Start_assigns_an_ephemeral_listening_port()
    {
        await using var server = new TcpSessionServer();

        await server.StartAsync(0);

        Assert.InRange(server.Port, 1, 65535);
        await server.StopAsync();
    }

    [Fact]
    public async Task Hello_receives_a_welcome_with_assigned_player_id()
    {
        await using var server = new TcpSessionServer();
        await server.StartAsync(0);

        using var client = new TcpClient();
        await client.ConnectAsync("127.0.0.1", server.Port);
        using var stream = client.GetStream();
        using var writer = new StreamWriter(stream, leaveOpen: true) { AutoFlush = true };
        using var reader = new StreamReader(stream, leaveOpen: true);
        var hello = new NetworkEnvelope(
            MessageKind.Hello,
            Guid.Empty,
            JsonSerializer.Serialize(new HelloPayload("Alice")));

        await writer.WriteLineAsync(hello.Serialize());
        var response = await reader.ReadLineAsync();

        Assert.NotNull(response);
        var welcome = NetworkEnvelope.Deserialize(response);
        Assert.Equal(MessageKind.Welcome, welcome.Kind);
        Assert.NotEqual(
            Guid.Empty,
            JsonSerializer.Deserialize<WelcomePayload>(welcome.Payload, new JsonSerializerOptions(JsonSerializerDefaults.Web))!.PlayerId);
    }
}
