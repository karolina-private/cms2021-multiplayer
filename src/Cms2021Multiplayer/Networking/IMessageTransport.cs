namespace Cms2021Multiplayer.Networking;

public interface IMessageTransport : IAsyncDisposable
{
    event Func<NetworkEnvelope, Task>? MessageReceived;

    Task StartHostAsync(int port, CancellationToken cancellationToken = default);
    Task ConnectAsync(string host, int port, CancellationToken cancellationToken = default);
    Task SendAsync(NetworkEnvelope message, CancellationToken cancellationToken = default);
    Task StopAsync();
}
