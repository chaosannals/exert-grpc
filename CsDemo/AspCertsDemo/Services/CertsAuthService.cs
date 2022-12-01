using Grpc.Core;
using Microsoft.AspNetCore.Authorization;

namespace AspCertsDemo.Services;

[Authorize]
public class CertsAuthService : AspCertsAuth.AspCertsAuthBase
{
    private readonly ILogger<CertsAuthService> _logger;
    public CertsAuthService(ILogger<CertsAuthService> logger)
    {
        _logger = logger;
    }

    public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
    {
        var user = context.GetHttpContext().User;
        return Task.FromResult(new HelloReply
        {
            Message = "Hello" + request.Name + " By Certs"
        });
    }

    public override Task<HelloReply> SetAuth(HelloRequest request, ServerCallContext context)
    {
        return Task.FromResult(new HelloReply
        {
            Message = "Hello" + request.Name + " By Certs"
        });
    }
}
