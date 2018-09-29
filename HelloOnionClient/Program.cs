using System;
using System.Threading.Tasks;
using Grpc.Core;
using MagicOnion;
using MagicOnion.Client;
using MagicOnion.Server;
using System;

namespace HelloOnionClient
{
    class Program
    {
        static void Main(string[] args)
        {
            ClientImpl().GetAwaiter().GetResult();

            Console.WriteLine("Finished");
        }

        // Blank, used by next section 
        static async Task ClientImpl()
        {
            // standard gRPC channel
            var channel = new Channel("localhost", 8080, ChannelCredentials.Insecure);

            // create MagicOnion dynamic client proxy
            var client = MagicOnionClient.Create<IMyFirstService>(channel);

            // call method.
            var result = await client.SumAsync(100, 200);
            Console.WriteLine("Client Received:" + result);
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
