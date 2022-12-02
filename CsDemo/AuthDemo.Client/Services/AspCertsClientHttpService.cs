using AspCertsDemo;
using AuthDemo.Client.Properties;
using Google.Protobuf;
using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace AuthDemo.Client.Services;

public class AspCertsClientHttpService : BackgroundService
{
    private int port;
    private string host;
    private GrpcChannel channel;
    private Greeter.GreeterClient client;
    private AspCertsMake.AspCertsMakeClient makeClient;
    private ILogger<AspCertsClientHttpService> logger;

    private int vPort;
    private string vHost;

    public AspCertsClientHttpService(IConfiguration config, ILogger<AspCertsClientHttpService> logger)
    {
        this.logger = logger;
        port = config.GetValue<int>("AspCertsHttp:Port");
        host = config.GetValue<string>("AspCertsHttp:Host")!;
        vPort = config.GetValue<int>("AspCertsHttps:Port");
        vHost = config.GetValue<string>("AspCertsHttps:Host")!;
        channel = GrpcChannel.ForAddress($"http://{host}:{port}");
        client = new Greeter.GreeterClient(channel);
        makeClient = new AspCertsMake.AspCertsMakeClient(channel);
    }

    public async Task ValidCert(ByteString bytes)
    {
        // TODO
        var clientCertificate = new X509Certificate2(bytes.Span, "123456");
        //var clientCertificate = new X509Certificate2(Resources.client_pfx, "1234");
        var httpHandler = new HttpClientHandler();
        httpHandler.ClientCertificates.Add(clientCertificate);
        using var vChannel = GrpcChannel.ForAddress($"https://{vHost}:{vPort}", new GrpcChannelOptions { HttpHandler = httpHandler });
        var vClient = new AspCertsAuth.AspCertsAuthClient(vChannel);
        var request = new HelloRequest
        {
            Name = "Https Valid Certs",
        };
        var r = await vClient.SayHelloAsync(request);
        logger.LogInformation("from valid server: {}", r.Message);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await Task.Delay(4000);


                var request = new HelloRequest
                {
                    Name = "Http",
                };
                var r = await client.SayHelloAsync(request);
                logger.LogInformation("from server: {}", r.Message);

                // make
                var makeRequest = new MakeCertRequest
                {
                    Account = "123456",
                };
                var makeReply = makeClient.MakeCert(makeRequest);
                logger.LogInformation("from make server: {} {}", makeReply.Tip, makeReply.Cert.Length);

                await ValidCert(makeReply.Cert);

                await Task.Yield();
            }
            catch (Exception e)
            {
                logger.LogError("certs http error: {}", e);
            }
        }
    }
}
