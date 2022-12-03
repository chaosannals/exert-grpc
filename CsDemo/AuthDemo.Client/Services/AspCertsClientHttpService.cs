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

    public async Task ValidCert(ByteString bytes, string pem)
    {
        // TODO
        //var clientCertificate = X509Certificate2.CreateFromPem(pem);

        var clientCertificate = new X509Certificate2(bytes.Span, "123456");
        //ExportCertPfxTo(clientCertificate, "bbb.pfx", "123456");

        //var clientCertificate = new X509Certificate2(Resources.client_pfx, "1234");
        //PrintChain(clientCertificate);
        //ExportCertPfxTo(clientCertificate, "ddd.pfx", "1234");


        //var c1 = new X509Certificate2(Resources.client_pfx, "1234");
        //var clientCertificate = X509Certificate2.CreateFromPem(c1.ExportCertificatePem());

        logger.LogInformation("client certs: {}", clientCertificate.ExportCertificatePem());

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

                await ValidCert(makeReply.Cert, makeReply.Pem);

                await Task.Yield();
            }
            catch (Exception e)
            {
                logger.LogError("certs http error: {}", e);
            }
        }
    }

    public static void ExportCertPfxTo(X509Certificate2 cert, string path, string? password)
    {
        var bytes =cert.Export(X509ContentType.Pfx, password);
        File.WriteAllBytes(path, bytes);
    }

    public void PrintChain(X509Certificate2 certificate)
    {
        var ch = new X509Chain();
        ch.ChainPolicy.RevocationMode = X509RevocationMode.Online;
        ch.Build(certificate);
        logger.LogInformation("Client ChainInformation");
        logger.LogInformation("Client Chainrevocation flag: {0}", ch.ChainPolicy.RevocationFlag);
        logger.LogInformation("Client Chainrevocation mode: {0}", ch.ChainPolicy.RevocationMode);
        logger.LogInformation("Client Chainverification flag: {0}", ch.ChainPolicy.VerificationFlags);
        logger.LogInformation("Client Chainverification time: {0}", ch.ChainPolicy.VerificationTime);
        logger.LogInformation("Client Chainstatus length: {0}", ch.ChainStatus.Length);
        logger.LogInformation("Client Chainapplication policy count: {0}", ch.ChainPolicy.ApplicationPolicy.Count);
        logger.LogInformation("Client Chaincertificate policy count: {0} {1}", ch.ChainPolicy.CertificatePolicy.Count, Environment.NewLine);

        //Output chain element information.
        logger.LogInformation("Client ChainElement Information");
        logger.LogInformation("Client Number of chain elements: {0}", ch.ChainElements.Count);
        logger.LogInformation("Client Chainelements synchronized? {0} {1}", ch.ChainElements.IsSynchronized, Environment.NewLine);

        foreach (X509ChainElement element in ch.ChainElements)
        {
            logger.LogInformation("Client Element issuer name: {0}", element.Certificate.Issuer);
            logger.LogInformation("Client Element certificate valid until: {0}", element.Certificate.NotAfter);
            logger.LogInformation("Client Element certificate is valid: {0}", element.Certificate.Verify());
            logger.LogInformation("Client Element error status length: {0}", element.ChainElementStatus.Length);
            logger.LogInformation("Client Element information: {0}", element.Information);
            logger.LogInformation("Client Number of element extensions: {0}{1}", element.Certificate.Extensions.Count, Environment.NewLine);

            if (ch.ChainStatus.Length > 1)
            {
                for (int index = 0; index < element.ChainElementStatus.Length; index++)
                {
                    logger.LogInformation("Client {}", element.ChainElementStatus[index].Status);
                    logger.LogInformation("Client {}", element.ChainElementStatus[index].StatusInformation);
                }
            }
        }
    }
}
