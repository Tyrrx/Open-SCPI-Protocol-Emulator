using System.Collections.Concurrent;
using System.Threading.Tasks;
using Domain.Interfaces;
using Domain.Keysight34465A;
using Emulator.Command;
using FunicularSwitch;

namespace Emulator.CommandHandler
{
    public class Keysight34465ACommandHandler : ICommandHandler<Keysight34465A, Keysight34465ACommand>
    {
        public Task<Result<CommandExecutionResult<Keysight34465ACommand>>> ProcessCommand(
            Keysight34465A device,
            Keysight34465ACommand command,
            ConcurrentQueue<IStringConvertible> outputQueue,
            CommandExecutionResult<Keysight34465ACommand> executionResult)
        {
            executionResult.ExecutedCommands.Add(command);

            Task<Result<CommandExecutionResult<Keysight34465ACommand>>> Identification(
                Keysight34465ACommand.Identification_ identification) =>
                Task.FromResult(
                        FetchIdentification.OfDeviceIntoOutputQueue(device, outputQueue))
                    .Map(_ => executionResult);

            Task<Result<CommandExecutionResult<Keysight34465ACommand>>> Read(Keysight34465ACommand.Read_ read) =>
                ProcessCommand(
                        device,
                        Keysight34465ACommand.Initiate,
                        outputQueue,
                        executionResult)
                    .Bind(result =>
                        ProcessCommand(
                            device,
                            Keysight34465ACommand.Fetch,
                            outputQueue,
                            result));

            Task<Result<CommandExecutionResult<Keysight34465ACommand>>> AbortHandler(Keysight34465ACommand.Abort_ abort) =>
                Task.FromResult(
                        AbortMeasuring.OfDevice(device))
                    .Map(_ => executionResult);

            Task<Result<CommandExecutionResult<Keysight34465ACommand>>> InitiateHandler(
                Keysight34465ACommand.Initiate_ initiate) =>
                Task.FromResult(
                        InitiateMeasuring.OfDevice(device))
                    .Map(_ => executionResult);

            Task<Result<CommandExecutionResult<Keysight34465ACommand>>> FetchHandler(Keysight34465ACommand.Fetch_ fetch) =>
                Task.FromResult(
                        FetchMeasuringValues.FromDeviceIntoOutputQueue(device, outputQueue))
                    .Map(_ => executionResult);

            Task<Result<CommandExecutionResult<Keysight34465ACommand>>> ConfigureCurrentHandler(
                Keysight34465ACommand.ConfigureCurrent_ configureCurrent) =>
                Task.FromResult(
                        ConfigureCurrent.OfDeviceAccordingToGivenParameters(
                            device,
                            configureCurrent.ElectricCurrentType,
                            configureCurrent.Range,
                            configureCurrent.Resolution))
                    .Map(_ => executionResult);

            Task<Result<CommandExecutionResult<Keysight34465ACommand>>> MeasureCurrentHandler(
                Keysight34465ACommand.MeasureCurrent_ measureCurrent) =>
                ProcessCommand(device,
                        Keysight34465ACommand.ConfigureCurrent(
                            measureCurrent.ElectricCurrentType,
                            measureCurrent.Range,
                            measureCurrent.Resolution),
                        outputQueue,
                        executionResult)
                    .Bind(result =>
                        ProcessCommand(
                            device,
                            Keysight34465ACommand.Read,
                            outputQueue,
                            result));

            Task<Result<CommandExecutionResult<Keysight34465ACommand>>> ConfigureVoltageHandler(
                Keysight34465ACommand.ConfigureVoltage_ configureVoltage) =>
                Task.FromResult(
                        ConfigureVoltage.OfDeviceAccordingToGivenParameters(
                            device,
                            configureVoltage.ElectricCurrentType,
                            configureVoltage.Range,
                            configureVoltage.Resolution))
                    .Map(_ => executionResult);

            Task<Result<CommandExecutionResult<Keysight34465ACommand>>> MeasureVoltageHandler(
                Keysight34465ACommand.MeasureVoltage_ measureVoltage) =>
                ProcessCommand(device,
                        Keysight34465ACommand.ConfigureVoltage(
                            measureVoltage.ElectricCurrentType,
                            measureVoltage.Range,
                            measureVoltage.Resolution),
                        outputQueue,
                        executionResult)
                    .Bind(result =>
                        ProcessCommand(
                            device,
                            Keysight34465ACommand.Read,
                            outputQueue,
                            result));

            Task<Result<CommandExecutionResult<Keysight34465ACommand>>> DisplayTextHandler(
                Keysight34465ACommand.DisplayText_ displayText) =>
                ProcessCommand(
                        device,
                        Keysight34465ACommand.ClearDisplay,
                        outputQueue,
                        executionResult)
                    .Bind(result =>
                        DisplayText.OnMeasuringDevice(
                                displayText.Text,
                                device)
                            .Map(_ => result))
                    .Bind(result =>
                        ProcessCommand(
                            device,
                            Keysight34465ACommand.ClearDisplay,
                            outputQueue,
                            result));

            Task<Result<CommandExecutionResult<Keysight34465ACommand>>> ClearDisplayHandler(
                Keysight34465ACommand.ClearDisplay_ clearDisplay) =>
                Task.FromResult(
                        ClearDisplay.OfMeasuringDevice(device))
                    .Map(_ => executionResult);

            Task<Result<CommandExecutionResult<Keysight34465ACommand>>> SetImpedanceHandler(
                Keysight34465ACommand.SetImpedance_ setImpedance) =>
                Task.FromResult(
                        SetImpedance.OfMeasuringDevice(
                            device,
                            setImpedance.Impedance))
                    .Map(_ => executionResult);

            return command.Match(
                Identification,
                Read,
                AbortHandler,
                InitiateHandler,
                FetchHandler,
                ConfigureCurrentHandler,
                MeasureCurrentHandler,
                ConfigureVoltageHandler,
                MeasureVoltageHandler,
                DisplayTextHandler,
                ClearDisplayHandler,
                SetImpedanceHandler);
        }
    }
}