using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace CsDemo.Server.Implements;

public class BookImplement : Book.BookBase
{
    private ILogger<BookImplement> logger;
    public BookImplement(ILogger<BookImplement> logger)
    {
        this.logger = logger;
    }

    public override async Task<BookReply> SayHello(BookRequest request, ServerCallContext context)
    {
        logger.LogInformation("SayHello");
        await Task.Yield();
        return new BookReply {
            Message = "Hello " + request.Name,
            MarksCount = request.Marks.Count,
        };
    }
}
