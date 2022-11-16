using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grpc.Core;

namespace CsDemo.Server.Implements;

class BookImplement : Book.BookBase
{
    public override async Task<BookReply> SayHello(BookRequest request, ServerCallContext context)
    {
        return new BookReply {
            Message = "Hello " + request.Name,
            MarksCount = request.Marks.Count,
        };
    }
}
