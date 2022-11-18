using AuthDemo.Grpc;
using Grpc.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AuthDemo.Client.Services;

public class TesterService : BackgroundService
{
    private int port;
    private string host;
    private Channel channel;
    private AuthDemoRpc.AuthDemoRpcClient adclient;
    private ILogger<TesterService> logger;

    public TesterService(IConfiguration config, ILogger<TesterService> logger)
    {
        this.logger = logger;
        port = config.GetValue<int>("Tcp:Port");
        host = config.GetValue<string>("Tcp:Host")!;
        channel = new Channel($"{host}:{port}", ChannelCredentials.Insecure);
        adclient = new AuthDemoRpc.AuthDemoRpcClient(channel);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while(!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(1000);
            var req = new AuthSayRequest
            {
                Content = "adfadsfsdf",
            };
            var res = await adclient.SayAsync(req);
            if (res.Code == 0)
            {

            }
            logger.LogInformation("respond: {}", res.Tip);
        }
    }
}
