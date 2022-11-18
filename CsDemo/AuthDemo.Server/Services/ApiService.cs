using AuthDemo.Grpc;
using AuthDemo.Server.Implements;
using Grpc.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using GrpcServer = Grpc.Core.Server;

namespace AuthDemo.Server.Services;

public class ApiService : IHostedService
{
    private int port;
    private string host;
    private GrpcServer server;
    private ILogger<ApiService> logger;

    public ApiService(IConfiguration config, ILogger<ApiService> logger, AuthDemoImplement adi)
    {
        this.logger = logger;
        port = config.GetValue<int>("Tcp:Port");
        host = config.GetValue<string>("Tcp:Host")!;
        server = new GrpcServer
        {
            Services = {
                AuthDemoRpc.BindService(adi),
            },
            Ports = {
                new ServerPort(host, port, ServerCredentials.Insecure),
            },
        };
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        server.Start();
        await Task.Yield();
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await server.ShutdownAsync();
    }
}
