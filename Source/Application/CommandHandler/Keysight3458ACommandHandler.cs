using System.Collections.Concurrent;
using System.Threading.Tasks;
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

			Task<Result<CommandExecutionResult<Keysight3458ACommand>>> Identification(Keysight3458ACommand.Identification_ identification) =>
				device.GetIdentification()
					.Map(result =>
					{
						outputQueue.Enqueue(result);
						return executionResult;
					});

			Task<Result<CommandExecutionResult<Keysight3458ACommand>>> Read(Keysight3458ACommand.Read_ read) =>
				ProcessCommand(device, Keysight3458ACommand.Initiate, outputQueue, executionResult)
					.Bind(result => ProcessCommand(device, Keysight3458ACommand.Fetch, outputQueue, result));

			Task<Result<CommandExecutionResult<Keysight3458ACommand>>> Abort(Keysight3458ACommand.Abort_ abort) =>
				device.Abort()
					.Map(_ => executionResult);

			Task<Result<CommandExecutionResult<Keysight3458ACommand>>> Initiate(Keysight3458ACommand.Initiate_ initiate) =>
				device.Initiate()
					.Map(_ => executionResult);

			Task<Result<CommandExecutionResult<Keysight3458ACommand>>> Fetch(Keysight3458ACommand.Fetch_ fetch) =>
				device.Fetch(outputQueue)
					.Map(_ => executionResult);

			Task<Result<CommandExecutionResult<Keysight3458ACommand>>> ConfigureCurrent(Keysight3458ACommand.ConfigureCurrent_ configureCurrent) =>
				device.ConfigureCurrent(configureCurrent.ElectricCurrentType, configureCurrent.Range, configureCurrent.Resolution)
					.Map(_ => executionResult);

			Task<Result<CommandExecutionResult<Keysight3458ACommand>>> MeasureCurrent(Keysight3458ACommand.MeasureCurrent_ measureCurrent) =>
				ProcessCommand(device,Keysight3458ACommand.ConfigureCurrent(measureCurrent.ElectricCurrentType, measureCurrent.Range, measureCurrent.Resolution), outputQueue, executionResult)
					.Bind(result => ProcessCommand(device, Keysight3458ACommand.Read, outputQueue, result));

			Task<Result<CommandExecutionResult<Keysight3458ACommand>>> ConfigureVoltage(Keysight3458ACommand.ConfigureVoltage_ configureVoltage) =>
				device.ConfigureVoltage(configureVoltage.ElectricCurrentType, configureVoltage.Range, configureVoltage.Resolution)
					.Map(_ => executionResult);

			Task<Result<CommandExecutionResult<Keysight3458ACommand>>> MeasureVoltage(Keysight3458ACommand.MeasureVoltage_ measureVoltage) =>
				ProcessCommand(device, Keysight3458ACommand.ConfigureVoltage(measureVoltage.ElectricCurrentType, measureVoltage.Range, measureVoltage.Resolution), outputQueue, executionResult)
					.Bind(_ => ProcessCommand(device, Keysight3458ACommand.Read, outputQueue, executionResult))
					.Map(_ => executionResult);

			return command.Match(
				Identification,
				Read,
				Abort,
				Initiate,
				Fetch,
				ConfigureCurrent,
				MeasureCurrent,
				ConfigureVoltage,
				MeasureVoltage);
		}
	}
}