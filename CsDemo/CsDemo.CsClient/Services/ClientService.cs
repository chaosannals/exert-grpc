using Grpc.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
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
    private ILogger<ClientService> logger;

    public ClientService(IConfiguration config, ILogger<ClientService> logger)
    {
        port = config.GetValue<int>("Tcp:Port");
        host = config.GetValue<string>("Tcp:Host");
        channel = new Channel($"{host}:{port}", ChannelCredentials.Insecure);
        client = new Greeter.GreeterClient(channel);
        bclient = new Book.BookClient(channel);
        this.logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
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
        }
        await channel.ShutdownAsync();
    }
}
