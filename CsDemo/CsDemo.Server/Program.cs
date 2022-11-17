using System.IO;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using CsDemo.Server.Services;
using CsDemo.Server.Implements;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureHostConfiguration(cd =>
    {
        cd.SetBasePath(Directory.GetCurrentDirectory());
        cd.AddEnvironmentVariables(prefix: "GRPC_CS_DEMO_");
    })
    .ConfigureServices((hc, services) =>
    {
        services.AddSingleton<CsDemoImplement>();
        services.AddSingleton<BookImplement>();
        services.AddHostedService<ServerService>();
    })
    .ConfigureLogging((hc, cl) =>
    {
        cl.AddFile(hc.Configuration.GetSection("LoggingFile"));
        cl.AddConsole();
    })
    .UseConsoleLifetime()
    .Build();

await host.RunAsync();

