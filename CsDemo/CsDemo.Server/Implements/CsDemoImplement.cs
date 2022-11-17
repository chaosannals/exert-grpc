using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace CsDemo.Server.Implements;

public class CsDemoImplement : Greeter.GreeterBase
{
    private ILogger<CsDemoImplement> logger;

    public CsDemoImplement(ILogger<CsDemoImplement> logger)
    {
        this.logger = logger;
    }

    public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
    {
        logger.LogInformation("SayHello CSDemo");
        return Task.FromResult(new HelloReply { Message = "Hello " + request.Name });
    }
}
