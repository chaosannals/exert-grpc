using System.Threading.Tasks;
using Grpc.Core;

namespace CsDemo.Server.Implements;

public class CsDemoImplement : Greeter.GreeterBase
{
    public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
    {
        return Task.FromResult(new HelloReply { Message = "Hello " + request.Name });
    }
}
