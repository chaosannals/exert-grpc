using Grpc.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace CsDemo.CsClient.Services;

public class ClientService : BackgroundService
{
    private int port;
    private string host;
    private Channel channel;
    private Greeter.GreeterClient client;
    private Book.BookClient bclient;
    private SkiaDemo.SkiaDemoClient sclient;
    private ILogger<ClientService> logger;

    public ClientService(IConfiguration config, ILogger<ClientService> logger)
    {
        port = config.GetValue<int>("Tcp:Port");
        host = config.GetValue<string>("Tcp:Host");
        channel = new Channel($"{host}:{port}", ChannelCredentials.Insecure);
        client = new Greeter.GreeterClient(channel);
        bclient = new Book.BookClient(channel);
        sclient = new SkiaDemo.SkiaDemoClient(channel);
        this.logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        bool skiaOnce = false;
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(1000);

            // Greeter
            string user = "you";
            var reply = client.SayHello(new HelloRequest { Name = user });
            logger.LogInformation("Greeting: {}", reply.Message);

            // Book
            var br = new BookRequest { Name = "boook" };
            var bi = 0;
            for (var i = 0; i < Random.Shared.Next(0, 10); i++)
            {
                bi += Random.Shared.Next(0, 10);
                br.Marks.Add(new BookMark
                {
                    Index = bi,
                    Tag = $"书签 {i}",
                });
            }
            var breply = bclient.SayHello(br);
            logger.LogInformation("Book: {} Marks: {}", breply.Message, breply.MarksCount);

            // Skia
            if (!skiaOnce)
            {
                await DoSkiaAsync();
                skiaOnce = true;
            }

            await Task.Delay(10000);
        }
        await channel.ShutdownAsync();
    }

    private async Task DoSkiaAsync()
    {
        var sr = new DrawBySkiaRequest
        {

        };
        sr.Contents.AddRange(new List<string>
            {
                "测试中文aa啊手动阀手动阀1324657943333333333333333333撒地方",
                "冒号中文：阿斯蒂芬撒打发打发打发             4f444444444444444444444444444444444444444444444444444444444444阿三发射点发撒地方444444444444444444",
                "dsfdsfsdfsd: sdfsd f sdfs dfsd fsd fsdfsdfsdf",
            });
        var sreply = await sclient.DrawBySkiaAsync(sr);
        logger.LogInformation("Skia Length: {} : {}", sreply.Result.Length, sreply.Message);
        if (sreply.Code == 0)
        {
            var p = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "result.jpg");
            using var f = File.Open(p, FileMode.OpenOrCreate);
            f.Write(sreply.Result.Span);
        }
    }
}
