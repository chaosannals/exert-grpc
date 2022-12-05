using AspCertsDemo.Services;
using Microsoft.AspNetCore.Authentication.Certificate;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.AspNetCore.Server.Kestrel.Https;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using AspCertsDemo.Properties;

var builder = WebApplication.CreateBuilder(args);


// 强制 HTTPS 索要证书
builder.Services.Configure<KestrelServerOptions>(options =>
{
    options.ConfigureHttpsDefaults(op =>
    {
        //op.ServerCertificate = new X509Certificate2(Resources.rootca_pfx, "1234"); // 设置的是服务器的证书。
        op.ClientCertificateMode = ClientCertificateMode.RequireCertificate;
    });
});

// 证书授权 TODO
builder.Services.AddAuthentication(CertificateAuthenticationDefaults.AuthenticationScheme)
    .AddCertificate(options =>
    {
        //options.AllowedCertificateTypes = CertificateTypes.SelfSigned;
        options.AllowedCertificateTypes = CertificateTypes.All;
        options.RevocationMode = X509RevocationMode.NoCheck; // 不检查可以让代码自动生成的证书通过，打开手动命令签的可以，代码自动生成的不行（会进入 OnAuthenticationFailed）。
        // TODO 尝试修改生成证书代码，使得打开检查也能和手动命令签的一样通过。
        // tip: 应该是代码生成的不是 chain certs ，没有带上 CA 的 Pubkey 导致不能通过 CA 的验证。

        options.Events = new CertificateAuthenticationEvents
        {
            OnAuthenticationFailed = context =>
            {
                context.Fail("aa");
                return Task.CompletedTask;
            },

            OnCertificateValidated = context =>
            {
                //var validationService = context.HttpContext.RequestServices
                //    .GetRequiredService<ICertificateValidationService>();
                var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<int>>();
                var clientCerts = context.ClientCertificate;

                //var rootca = new X509Certificate2(Resources.rootca_pfx, "1234");
                //logger.LogInformation("rootca: {}", rootca.ExportCertificatePem());

                logger.LogInformation("certs xxxxxxxxxxxx {}", clientCerts.Thumbprint);

                CertsMakeService.ExportCertPfxTo(clientCerts, "ccc.pfx");

                var claims = new[]
                {
                    new Claim(
                        ClaimTypes.NameIdentifier,
                        context.ClientCertificate.Subject,
                        ClaimValueTypes.String, context.Options.ClaimsIssuer),
                    new Claim(
                        ClaimTypes.Name,
                        context.ClientCertificate.Subject,
                        ClaimValueTypes.String, context.Options.ClaimsIssuer)
                };

                context.Principal = new ClaimsPrincipal(
                    new ClaimsIdentity(claims, context.Scheme.Name));
                context.Success();

                return Task.CompletedTask;
            }
        };
    });
builder.Services.AddAuthorization();

// 代理使用，通过头部传递证书。
//builder.Services.AddCertificateForwarding(options =>
//{
//    options.CertificateHeader = "X-SSL-CERT-KEY";

//    options.HeaderConverter = headerValue =>
//    {
//        X509Certificate2? clientCertificate = null;

//        if (!string.IsNullOrWhiteSpace(headerValue))
//        {
//            clientCertificate = new X509Certificate2();
//        }

//        return clientCertificate!;
//    };
//});

// Add services to the container.
builder.Services.AddGrpc();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

// Configure the HTTP request pipeline.
app.MapGrpcService<GreeterService>();
app.MapGrpcService<CertsAuthService>();
app.MapGrpcService<CertsMakeService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
