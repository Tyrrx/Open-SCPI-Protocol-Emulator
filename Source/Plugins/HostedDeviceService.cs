using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain.Abstractions;
using Emulator.Command;
using Emulator.Controller;
using EmulatorHost.Network;
using FunicularSwitch;
using FunicularSwitch.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Protocol.Execution;
using Protocol.Interpreter;

namespace EmulatorHost
{
    public class HostedDeviceService<TDeviceController, TCommand, TConfiguration, TProtocolInterpreter> : IHostedService
        where TDeviceController : IDeviceController<TCommand, IByteArrayConvertible>
        where TConfiguration : IDeviceConfiguration
        where TProtocolInterpreter : IProtocolInterpreter<TCommand>
    {
        private readonly ILogger<HostedDeviceService<TDeviceController, TCommand, TConfiguration, TProtocolInterpreter>>
            logger;

        private readonly TcpServer server;
        private readonly CommandExecutionAdapter<TCommand, IByteArrayConvertible> executionAdapter;

        private readonly IPEndPoint endPoint;
        private readonly Task resultStreamDisposable;

        public HostedDeviceService(
            ILogger<HostedDeviceService<TDeviceController, TCommand, TConfiguration, TProtocolInterpreter>> logger,
            Func<TConfiguration> configurationProvider,
            TcpServer server,
            TProtocolInterpreter protocolInterpreter,
            CommandExecutionAdapter<TCommand, IByteArrayConvertible> executionAdapter)
        {
            this.logger = logger;
            this.server = server;
            this.executionAdapter = executionAdapter;

            endPoint = new IPEndPoint(IPAddress.Parse(configurationProvider().Ip), configurationProvider().Port);

            resultStreamDisposable = Task.Run(() =>
                server.ReceiveStream.ForEachAsync(async input =>
                {
                    await protocolInterpreter
                        .GetCommand(input)
                        .Bind(command =>
                            executionAdapter.Execute(command))
                        .Match(
                            success =>
                            {
                                logger.LogInformation(
                                    $"{string.Join(" -> ", SelectCommandTypeNames(success))}");
                                return No.Thing;
                            },
                            errorMessage =>
                            {
                                logger.LogError(errorMessage);
                                return No.Thing;
                            });
                }));
        }

        private IEnumerable<string> SelectCommandTypeNames(CommandExecutionResult<TCommand> result)
        {
            return result.ExecutedCommands.Select(executedCommand =>
                executedCommand.GetType().BeautifulName().TrimEnd('_'));
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation(
                $"Starting device: {typeof(TDeviceController).BeautifulName()} on: {endPoint}");
            server.Start(endPoint, executionAdapter.GetOutputQueue());
            return Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("Shutting down...");
            server.Stop();
            await resultStreamDisposable.ConfigureAwait(false);
        }
    }
}