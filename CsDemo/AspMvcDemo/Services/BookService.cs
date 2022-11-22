using AspMvcDemo.GrpcProtos;
using Grpc.Core;

namespace AspMvcDemo.Services;

public class BookService : AspMvcBookDemo.AspMvcBookDemoBase
{
    public override async Task<AspMvcBookListBooksReply> ListBooks(AspMvcBookListBooksRequest request, ServerCallContext context)
    {
        return new AspMvcBookListBooksReply
        {
            Code = 0,
            Tip = "Ok",
        };
    }
}
