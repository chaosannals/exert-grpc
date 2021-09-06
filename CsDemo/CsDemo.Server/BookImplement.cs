using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grpc.Core;

namespace CsDemo.Server
{
    class BookImplement : Book.BookBase
    {
        public override Task<BookReply> SayHello(BookRequest request, ServerCallContext context)
        {
            return Task.FromResult(new BookReply { Message = "Hello " + request.Name });
        }
    }
}
