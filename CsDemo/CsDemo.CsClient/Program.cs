using System;
using Grpc.Core;

namespace CsDemo.CsClient
{
    class Program
    {
        public static void Main(string[] args)
        {
            Channel channel = new Channel("127.0.0.1:30051", ChannelCredentials.Insecure);

            // Greeter
            var client = new Greeter.GreeterClient(channel);
            string user = "you";

            var reply = client.SayHello(new HelloRequest { Name = user });
            Console.WriteLine("Greeting: " + reply.Message);

            // Book
            var bclient = new Book.BookClient(channel);
            var breply = bclient.SayHello(new BookRequest { Name = "boook" });
            Console.WriteLine("Book: " + breply.Message);

            channel.ShutdownAsync().Wait();
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
