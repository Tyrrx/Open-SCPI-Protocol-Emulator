using System.Collections.Concurrent;
using System.Threading.Tasks;
using Domain;
using Domain.Interfaces;
using Domain.Keysight3458A;
using Emulator.Command;
using FunicularSwitch;

namespace Emulator.CommandHandler
{
    public class Keysight3458ACommandHandler : ICommandHandler<Keysight3458A, Keysight3458ACommand>
    {
        public Task<Result<CommandExecutionResult<Keysight3458ACommand>>> ProcessCommand(
            Keysight3458A device,
            Keysight3458ACommand command,
            ConcurrentQueue<IStringConvertible> outputQueue,
            CommandExecutionResult<Keysight3458ACommand> executionResult)
        {
            executionResult.ExecutedCommands.Add(command);

            Task<Result<CommandExecutionResult<Keysight3458ACommand>>> IdentificationHandler(
                Keysight3458ACommand.Identification_ identification) =>
                Task.FromResult(
                        FetchIdentification.OfDeviceIntoOutputQueue(
                            device,
                            outputQueue))
                    .Map(_ => executionResult);

            Task<Result<CommandExecutionResult<Keysight3458ACommand>>> ReadHandler(Keysight3458ACommand.Read_ read) =>
                ProcessCommand(
                        device,
                        Keysight3458ACommand.Initiate,
                        outputQueue,
                        executionResult)
                    .Bind(result =>
                        ProcessCommand(
                            device,
                            Keysight3458ACommand.Fetch,
                            outputQueue,
                            result));

            Task<Result<CommandExecutionResult<Keysight3458ACommand>>>
                AbortHandler(Keysight3458ACommand.Abort_ abort) =>
                Task.FromResult(
                        AbortMeasuring.ByNotifying(device))
                    .Map(_ => executionResult);

            Task<Result<CommandExecutionResult<Keysight3458ACommand>>>
                InitiateHandler(Keysight3458ACommand.Initiate_ initiate) =>
                Task.FromResult(
                        InitiateMeasuring.OfDevice(device))
                    .Map(_ => executionResult);

            Task<Result<CommandExecutionResult<Keysight3458ACommand>>>
                FetchHandler(Keysight3458ACommand.Fetch_ fetch) =>
                Task.FromResult(
                        FetchMeasuringValues.FromDeviceIntoOutputQueue(device, outputQueue))
                    .Map(_ => executionResult);

            Task<Result<CommandExecutionResult<Keysight3458ACommand>>> ConfigureCurrentHandler(
                Keysight3458ACommand.ConfigureCurrent_ configureCurrent) =>
                Task.FromResult(
                        ConfigureCurrent.OfDeviceAccordingToGivenParameters(
                            device,
                            configureCurrent.ElectricCurrentType,
                            configureCurrent.Range,
                            configureCurrent.Resolution))
                    .Map(_ => executionResult);

            Task<Result<CommandExecutionResult<Keysight3458ACommand>>> MeasureCurrentHandler(
                Keysight3458ACommand.MeasureCurrent_ measureCurrent) =>
                ProcessCommand(device,
                        Keysight3458ACommand.ConfigureCurrent(
                            measureCurrent.ElectricCurrentType,
                            measureCurrent.Range,
                            measureCurrent.Resolution),
                        outputQueue,
                        executionResult)
                    .Bind(result =>
                        ProcessCommand(
                            device,
                            Keysight3458ACommand.Read,
                            outputQueue,
                            result));

            Task<Result<CommandExecutionResult<Keysight3458ACommand>>> ConfigureVoltageHandler(
                Keysight3458ACommand.ConfigureVoltage_ configureVoltage) =>
                Task.FromResult(
                        ConfigureVoltage.OfDeviceAccordingToGivenParameters(
                            device,
                            configureVoltage.ElectricCurrentType,
                            configureVoltage.Range,
                            configureVoltage.Resolution))
                    .Map(_ => executionResult);

            Task<Result<CommandExecutionResult<Keysight3458ACommand>>> MeasureVoltageHandler(
                Keysight3458ACommand.MeasureVoltage_ measureVoltage) =>
                ProcessCommand(device,
                        Keysight3458ACommand.ConfigureVoltage(
                            measureVoltage.ElectricCurrentType,
                            measureVoltage.Range,
                            measureVoltage.Resolution),
                        outputQueue,
                        executionResult)
                    .Bind(_ =>
                        ProcessCommand(
                            device,
                            Keysight3458ACommand.Read,
                            outputQueue,
                            executionResult))
                    .Map(_ => executionResult);

            return command.Match(
                IdentificationHandler,
                ReadHandler,
                AbortHandler,
                InitiateHandler,
                FetchHandler,
                ConfigureCurrentHandler,
                MeasureCurrentHandler,
                ConfigureVoltageHandler,
                MeasureVoltageHandler);
        }
    }
}