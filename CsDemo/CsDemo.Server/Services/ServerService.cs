using Grpc.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using CsDemo.Server.Implements;

namespace CsDemo.Server.Services;

public class ServerService : IHostedService
{
    private int port;
    private string host;
    private Grpc.Core.Server server;
    private ILogger<ServerService> logger;

    public ServerService(
        IConfiguration config,
        ILogger<ServerService> logger,
        CsDemoImplement csdemoi,
        BookImplement booki,
        SkiaDemoImplement skiai,
        ImageSharpDemoImplement imagesharpi
    ) {
        port = config.GetValue<int>("Tcp:Port");
        host = config.GetValue<string>("Tcp:Host");
        server = new Grpc.Core.Server
        {
            Services = {
                Greeter.BindService(csdemoi),
                Book.BindService(booki),
                SkiaDemo.BindService(skiai),
                ImageSharpDemo.BindService(imagesharpi),
            },
            Ports = {
                new ServerPort(host, port, ServerCredentials.Insecure),
            },
        };
        this.logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        server.Start();
        logger.LogInformation("Greeter server listening on port {}", port);
        await Task.Yield();
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await server.ShutdownAsync();
    }
}
