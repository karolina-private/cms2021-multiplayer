using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Text.Json;
using Cms2021Multiplayer.Networking;

namespace Cms2021Multiplayer.Server;

public sealed record HelloPayload(string DisplayName);
public sealed record WelcomePayload(Guid PlayerId);

public sealed class TcpSessionServer : IAsyncDisposable
{
    private readonly SessionRegistry _sessions = new();
    private readonly ConcurrentDictionary<Guid, TcpClient> _clients = new();
    private readonly JsonSerializerOptions _jsonOptions = new(JsonSerializerDefaults.Web);
    private TcpListener? _listener;
    private CancellationTokenSource? _cancellation;
    private Task? _acceptLoop;

    public int Port { get; private set; }

    public Task StartAsync(int port, CancellationToken cancellationToken = default)
    {
        if (_listener is not null)
        {
            throw new InvalidOperationException("The server is already running.");
        }

        _listener = new TcpListener(IPAddress.Any, port);
        _listener.Start();
        Port = ((IPEndPoint)_listener.LocalEndpoint).Port;
        _cancellation = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        _acceptLoop = AcceptLoopAsync(_cancellation.Token);
        return Task.CompletedTask;
    }

    public async Task StopAsync()
    {
        _cancellation?.Cancel();
        _listener?.Stop();
        _listener = null;

        if (_acceptLoop is not null)
        {
            try { await _acceptLoop.ConfigureAwait(false); }
            catch (OperationCanceledException) { }
        }

        foreach (var client in _clients.Values)
        {
            client.Dispose();
        }

        _clients.Clear();
        _cancellation?.Dispose();
        _cancellation = null;
        _acceptLoop = null;
        Port = 0;
    }

    private async Task AcceptLoopAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested && _listener is not null)
        {
            try
            {
                var client = await _listener.AcceptTcpClientAsync().ConfigureAwait(false);
                _ = HandleClientAsync(client, cancellationToken);
            }
            catch (ObjectDisposedException) when (cancellationToken.IsCancellationRequested) { }
            catch (SocketException) when (cancellationToken.IsCancellationRequested) { }
        }
    }

    private async Task HandleClientAsync(TcpClient client, CancellationToken cancellationToken)
    {
        Guid? playerId = null;
        try
        {
            using (client)
            using (var stream = client.GetStream())
            using (var reader = new StreamReader(stream, leaveOpen: true))
            using (var writer = new StreamWriter(stream, leaveOpen: true) { AutoFlush = true })
            {
                var line = await reader.ReadLineAsync().ConfigureAwait(false);
                if (line is null) return;

                var hello = NetworkEnvelope.Deserialize(line);
                if (hello.Kind != MessageKind.Hello) return;

                var payload = JsonSerializer.Deserialize<HelloPayload>(hello.Payload, _jsonOptions);
                if (payload is null || string.IsNullOrWhiteSpace(payload.DisplayName)) return;

                var player = _sessions.Join(payload.DisplayName);
                playerId = player.PlayerId;
                _clients[player.PlayerId] = client;

                await SendAsync(writer, new NetworkEnvelope(
                    MessageKind.Welcome,
                    Guid.Empty,
                    JsonSerializer.Serialize(new WelcomePayload(player.PlayerId), _jsonOptions))).ConfigureAwait(false);

                await BroadcastAsync(new NetworkEnvelope(
                    MessageKind.PlayerJoined,
                    player.PlayerId,
                    JsonSerializer.Serialize(player, _jsonOptions)), player.PlayerId).ConfigureAwait(false);

                while (!cancellationToken.IsCancellationRequested && (line = await reader.ReadLineAsync().ConfigureAwait(false)) is not null)
                {
                    var message = NetworkEnvelope.Deserialize(line);
                    if (message.SenderId == player.PlayerId)
                    {
                        await BroadcastAsync(message, player.PlayerId).ConfigureAwait(false);
                    }
                }
            }
        }
        finally
        {
            if (playerId is Guid id)
            {
                _clients.TryRemove(id, out _);
                if (_sessions.Leave(id))
                {
                    await BroadcastAsync(new NetworkEnvelope(MessageKind.PlayerLeft, id, "{}"), id).ConfigureAwait(false);
                }
            }
        }
    }

    private static Task SendAsync(StreamWriter writer, NetworkEnvelope message) =>
        writer.WriteLineAsync(message.Serialize());

    private async Task BroadcastAsync(NetworkEnvelope message, Guid excludedPlayerId)
    {
        foreach (var pair in _clients.Where(pair => pair.Key != excludedPlayerId))
        {
            try
            {
                using var writer = new StreamWriter(pair.Value.GetStream(), leaveOpen: true) { AutoFlush = true };
                await SendAsync(writer, message).ConfigureAwait(false);
            }
            catch (IOException) { }
            catch (ObjectDisposedException) { }
        }
    }

    public async ValueTask DisposeAsync() => await StopAsync().ConfigureAwait(false);
}
