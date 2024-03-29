﻿using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using AuthDemo.Client.Services;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureHostConfiguration(cd =>
    {
        cd.SetBasePath(Directory.GetCurrentDirectory());
        cd.AddEnvironmentVariables(prefix: "GRPC_CS_DEMO_");
    })
    .ConfigureServices((hc, services) =>
    {
        services.AddHostedService<AspCertsClientHttpService>();
        services.AddHostedService<AspCertsClientHttpsService>();
        services.AddHostedService<TesterService>();
    })
    .ConfigureLogging((hc, cl) =>
    {
        cl.AddFile(hc.Configuration.GetSection("LoggingFile"));
        cl.AddConsole();
    })
    .UseConsoleLifetime()
    .Build();

await host.RunAsync();

