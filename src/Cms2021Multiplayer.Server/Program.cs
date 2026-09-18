using Cms2021Multiplayer.Server;

var port = args.Length == 1 && int.TryParse(args[0], out var requestedPort)
    ? requestedPort
    : 31337;

using var cancellation = new CancellationTokenSource();
Console.CancelKeyPress += (_, eventArgs) =>
{
    eventArgs.Cancel = true;
    cancellation.Cancel();
};

await using var server = new TcpSessionServer();
await server.StartAsync(port, cancellation.Token);
Console.WriteLine($"CMS2021 multiplayer host is listening on TCP port {server.Port}. Press Ctrl+C to stop.");

try
{
    await Task.Delay(Timeout.InfiniteTimeSpan, cancellation.Token);
}
catch (OperationCanceledException)
{
    await server.StopAsync();
}
