using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Domain.Interfaces;
using Emulator.Command;
using Emulator.CommandHandler;
using FunicularSwitch;
using FunicularSwitch.Extensions;
using Microsoft.Extensions.Logging;

namespace Emulator
{
    public class GenericDevice<TCommandType, TOfIDeviceConfigurationType>
        where TOfIDeviceConfigurationType : IDeviceConfiguration
    {
        private readonly ILogger<GenericDevice<TCommandType, TOfIDeviceConfigurationType>> logger;
        private readonly IServer server;
        private readonly ICommandHandler<TCommandType> commandHandler;
        private readonly Option<TOfIDeviceConfigurationType> optionalDeviceConfiguration;
        private readonly Task inputHandlerTask;

        public GenericDevice(
            ILogger<GenericDevice<TCommandType, TOfIDeviceConfigurationType>> logger,
            IServer server,
            IProtocolInterpreter<TCommandType> protocolInterpreter,
            ICommandHandler<TCommandType> commandHandler,
            Option<TOfIDeviceConfigurationType> optionalDeviceConfiguration)
        {
            this.logger = logger;
            this.server = server;
            this.commandHandler = commandHandler;
            this.optionalDeviceConfiguration = optionalDeviceConfiguration;
            inputHandlerTask = RegisterInputHandler(protocolInterpreter);
        }

        private Task RegisterInputHandler(IProtocolInterpreter<TCommandType> protocolInterpreter)
        {
            return server
                .GetInputStream()
                .ForEachAsync(input =>
                {
                    protocolInterpreter.GetCommand(input)
                        .Bind(ProcessCommandWithHandler)
                        .Match(
                            OnSuccessfulCommandExecution,
                            OnErrorCommandExecution);
                });
        }

        public Task StartAsync()
        {
            optionalDeviceConfiguration
                .Match(configuration =>
                    server.Start(configuration));
            return Task.CompletedTask;
        }

        public Task StopAsync()
        {
            server.Stop();
            return inputHandlerTask;
        }

        private Task<Result<CommandExecutionResult<TCommandType>>> ProcessCommandWithHandler(TCommandType command) =>
            commandHandler.ProcessCommand(
                command,
                server.GetOutputQueue(),
                new CommandExecutionResult<TCommandType>());

        private Unit OnSuccessfulCommandExecution(CommandExecutionResult<TCommandType> success)
        {
            // ReSharper disable once TemplateIsNotCompileTimeConstantProblem
            logger.LogInformation($"{string.Join(" -> ", SelectCommandTypeNames(success))}");
            return No.Thing;
        }

        private Unit OnErrorCommandExecution(string error)
        {
            // ReSharper disable once TemplateIsNotCompileTimeConstantProblem
            logger.LogError(error);
            return No.Thing;
        }

        private static IEnumerable<string> SelectCommandTypeNames(CommandExecutionResult<TCommandType> result)
        {
            return result.ExecutedCommands.Select(executedCommand =>
                executedCommand.GetType().BeautifulName().TrimEnd('_'));
        }
    }
}