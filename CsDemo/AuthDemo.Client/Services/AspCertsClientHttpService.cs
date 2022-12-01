using AspCertsDemo;
using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthDemo.Client.Services;

public class AspCertsClientHttpService : BackgroundService
{
    private int port;
    private string host;
    private GrpcChannel channel;
    private Greeter.GreeterClient client;
    private ILogger<AspCertsClientHttpService> logger;

    public AspCertsClientHttpService(IConfiguration config, ILogger<AspCertsClientHttpService> logger)
    {
        this.logger = logger;
        port = config.GetValue<int>("AspCertsHttp:Port");
        host = config.GetValue<string>("AspCertsHttp:Host")!;
        channel = GrpcChannel.ForAddress($"http://{host}:{port}");
        client = new Greeter.GreeterClient(channel);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(4000);
            var request = new HelloRequest
            {
                Name = "Http",
            };
            var r = await client.SayHelloAsync(request);
            logger.LogInformation("from server: {}", r.Message);
            await Task.Yield();
        }
    }
}
