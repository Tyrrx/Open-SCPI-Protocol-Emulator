using Emulator;
using Microsoft.Extensions.Logging;

namespace PluginNetwork
{
    public class ServerFactory : IServerFactory
    {
        private readonly ILogger<TcpServer> logger;

        public ServerFactory(ILogger<TcpServer> logger)
        {
            this.logger = logger;
        }

        public IServer Create()
        {
            return new TcpServer(logger);
        }
    }
}