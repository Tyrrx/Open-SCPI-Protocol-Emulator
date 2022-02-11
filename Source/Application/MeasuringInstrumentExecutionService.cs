using System;
using System.Collections.Concurrent;
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
    public class MeasuringInstrumentExecutionService<TDeviceType, TCommandType>

    {
        private readonly ILogger<MeasuringInstrumentExecutionService<TDeviceType, TCommandType>> logger;
        private readonly IProtocolParser<TCommandType> protocolParser;
        private readonly ICommandHandler<TDeviceType, TCommandType> commandHandler;

        public MeasuringInstrumentExecutionService(
            ILogger<MeasuringInstrumentExecutionService<TDeviceType, TCommandType>> logger,
            IProtocolParser<TCommandType> protocolParser,
            ICommandHandler<TDeviceType, TCommandType> commandHandler)
        {
            this.logger = logger;
            this.protocolParser = protocolParser;
            this.commandHandler = commandHandler;
        }

        public IDisposable BeginExecution(
            IObservable<string> inputStream,
            ConcurrentQueue<IStringConvertible> outputQueue,
            TDeviceType device)
        {
            return inputStream.Subscribe(input =>
                protocolParser
                    .GetCommand(input)
                    .Bind(command =>
                        commandHandler.ProcessCommand(
                            device,
                            command,
                            outputQueue,
                            new CommandExecutionResult<TCommandType>()))
                    .Match(
                        OnSuccessfulCommandExecution,
                        OnErrorCommandExecution)
            );
        }

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