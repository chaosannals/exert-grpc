using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuthDemo.Grpc;
using Grpc.Core;

namespace AuthDemo.Server.Implements;

public class AuthDemoImplement : AuthDemoRpc.AuthDemoRpcBase
{
    public override async Task<AuthSayReply> Say(AuthSayRequest request, ServerCallContext context)
    {
        await Task.Yield();
        return new AuthSayReply
        {
            Code = 0,
            Tip = "Ok",
        };
    }
}
