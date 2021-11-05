using System.Collections.Concurrent;
using System.Threading.Tasks;
using Domain.Abstractions;
using Domain.Keysight3458A;
using Emulator.Command;
using FunicularSwitch;

namespace Emulator.Controller
{
	public class Keysight3458AController : IDeviceController<Keysight3458ACommand, IByteArrayConvertible>
	{
		private Keysight3458A Device { get; }

		public Keysight3458AController(Keysight3458A device)
		{
			Device = device;
		}
		public Task<Result<CommandExecutionResult<Keysight3458ACommand>>> ProcessCommand(
			Keysight3458ACommand command,
			ConcurrentQueue<IByteArrayConvertible> queue,
			CommandExecutionResult<Keysight3458ACommand> executionResult)
		{
			executionResult.ExecutedCommands.Add(command);

			Task<Result<CommandExecutionResult<Keysight3458ACommand>>> Identification(Keysight3458ACommand.Identification_ identification) =>
				Device.GetIdentification()
					.Map(result =>
					{
						queue.Enqueue(result);
						return executionResult;
					});

			Task<Result<CommandExecutionResult<Keysight3458ACommand>>> Read(Keysight3458ACommand.Read_ read) =>
				ProcessCommand(Keysight3458ACommand.Initiate, queue, executionResult)
					.Bind(result => ProcessCommand(Keysight3458ACommand.Fetch, queue, result));

			Task<Result<CommandExecutionResult<Keysight3458ACommand>>> Abort(Keysight3458ACommand.Abort_ abort) =>
				Device.Abort()
					.Map(_ => executionResult);

			Task<Result<CommandExecutionResult<Keysight3458ACommand>>> Initiate(Keysight3458ACommand.Initiate_ initiate) =>
				Device.Initiate()
					.Map(_ => executionResult);

			Task<Result<CommandExecutionResult<Keysight3458ACommand>>> Fetch(Keysight3458ACommand.Fetch_ fetch) =>
				Device.Fetch(queue)
					.Map(_ => executionResult);

			Task<Result<CommandExecutionResult<Keysight3458ACommand>>> ConfigureCurrent(Keysight3458ACommand.ConfigureCurrent_ configureCurrent) =>
				Device.ConfigureCurrent(configureCurrent.ElectricityType, configureCurrent.Range, configureCurrent.Resolution)
					.Map(_ => executionResult);

			Task<Result<CommandExecutionResult<Keysight3458ACommand>>> MeasureCurrent(Keysight3458ACommand.MeasureCurrent_ measureCurrent) =>
				ProcessCommand(Keysight3458ACommand.ConfigureCurrent(measureCurrent.ElectricityType, measureCurrent.Range, measureCurrent.Resolution), queue, executionResult)
					.Bind(result => ProcessCommand(Keysight3458ACommand.Read, queue, result));

			Task<Result<CommandExecutionResult<Keysight3458ACommand>>> ConfigureVoltage(Keysight3458ACommand.ConfigureVoltage_ configureVoltage) =>
				Device.ConfigureVoltage(configureVoltage.ElectricityType, configureVoltage.Range, configureVoltage.Resolution)
					.Map(_ => executionResult);

			Task<Result<CommandExecutionResult<Keysight3458ACommand>>> MeasureVoltage(Keysight3458ACommand.MeasureVoltage_ measureVoltage) =>
				ProcessCommand(Keysight3458ACommand.ConfigureVoltage(measureVoltage.ElectricityType, measureVoltage.Range, measureVoltage.Resolution), queue, executionResult)
					.Bind(_ => ProcessCommand(Keysight3458ACommand.Read, queue, executionResult))
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