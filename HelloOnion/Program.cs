using Grpc.Core;
using MagicOnion;
using MagicOnion.Client;
using MagicOnion.Server;
using System;
using System.Threading.Tasks;

namespace HelloOnion
{
    class Program
    {
        static void Main(string[] args)
        {
            GrpcEnvironment.SetLogger(new Grpc.Core.Logging.ConsoleLogger());

            var service = MagicOnionEngine.BuildServerServiceDefinition(isReturnExceptionStackTraceInErrorDetail: true);

            var server = new global::Grpc.Core.Server
            {
                Services = { service },
                Ports = { new ServerPort("localhost", 8080, ServerCredentials.Insecure) }
            };
            
            // launch gRPC Server.
            server.Start();

            Console.ReadLine();
        }
    }

    // define interface as Server/Client IDL.
    // implements T : IService<T>.
    public interface IMyFirstService : IService<IMyFirstService>
    {
        UnaryResult<int> SumAsync(int x, int y);
    }

    // implement RPC service.
    // inehrit ServiceBase<interface>, interface
    public class MyFirstService : ServiceBase<IMyFirstService>, IMyFirstService
    {
        public async UnaryResult<int> SumAsync(int x, int y)
        {
            Logger.Debug($"Received:{x}, {y}");

            return x + y;
        }
    }
}
