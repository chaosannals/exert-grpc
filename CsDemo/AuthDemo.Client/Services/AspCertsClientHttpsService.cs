using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using AspCertsDemo;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography.X509Certificates;
using AuthDemo.Client.Properties;

namespace AuthDemo.Client.Services;

public class AspCertsClientHttpsService : BackgroundService
{
    private int port;
    private string host;
    private GrpcChannel channel;
    private Greeter.GreeterClient client;
    private AspCertsAuth.AspCertsAuthClient authClient;
    private AspCertsMake.AspCertsMakeClient makeClient;
    private ILogger<AspCertsClientHttpsService> logger;

    public AspCertsClientHttpsService(IConfiguration config, ILogger<AspCertsClientHttpsService> logger)
    {
        this.logger = logger;
        port = config.GetValue<int>("AspCertsHttps:Port");
        host = config.GetValue<string>("AspCertsHttps:Host")!;

        var clientCertificate = new X509Certificate2(Resources.client_pfx, "1234");
        var httpHandler = new HttpClientHandler();
        httpHandler.ClientCertificates.Add(clientCertificate);
        channel = GrpcChannel.ForAddress(
            $"https://{host}:{port}"
            ,new GrpcChannelOptions { HttpHandler = httpHandler }
        );
        client = new Greeter.GreeterClient(channel);
        authClient = new AspCertsAuth.AspCertsAuthClient(channel);
        makeClient = new AspCertsMake.AspCertsMakeClient(channel);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await Task.Delay(4000);

                // 
                var request = new HelloRequest
                {
                    Name = "Https",
                };
                var r = await client.SayHelloAsync(request);
                logger.LogInformation("from server: {}", r.Message);

                var ra = await authClient.SayHelloAsync(request);
                logger.LogInformation("from auth server: {}", r.Message);


                // make
                var makeRequest = new MakeCertRequest
                {
                    Account = "123456",
                };
                var makeReply = makeClient.MakeCert(makeRequest);
                logger.LogInformation("from make server: {} {}", makeReply.Tip, makeReply.Cert.Length);

                await Task.Yield();
            }
            catch (Exception ex)
            {
                logger.LogWarning("request exception: {}", ex);
            }
        }
    }
}
