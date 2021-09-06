using System;
using System.Threading.Tasks;
using Grpc.Core;

namespace CsDemo.Server
{
    class Program
    {
        const int Port = 30051;

        public static void Main(string[] args)
        {
            Grpc.Core.Server server = new Grpc.Core.Server
            {
                Services = { Greeter.BindService(new CsDemoImplement()), Book.BindService(new BookImplement()) },
                Ports = { new ServerPort("localhost", Port, ServerCredentials.Insecure) }
            };
            server.Start();

            Console.WriteLine("Greeter server listening on port " + Port);
            Console.WriteLine("Press any key to stop the server...");
            Console.ReadKey();

            server.ShutdownAsync().Wait();
        }
    }
}
