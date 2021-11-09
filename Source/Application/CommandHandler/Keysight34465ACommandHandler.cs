using System.Collections.Concurrent;
using System.Threading.Tasks;
using Domain.Abstractions;
using Domain.Keysight34465A;
using Emulator.Command;
using FunicularSwitch;

namespace Emulator.CommandHandler
{
	public class Keysight34465ACommandHandler : ICommandHandler<Keysight34465ACommand, IByteArrayConvertible>
	{
		private Keysight34465A Device { get; }

		public Keysight34465ACommandHandler(Keysight34465A device)
		{
			Device = device;
		}
		
		public Task<Result<CommandExecutionResult<Keysight34465ACommand>>> ProcessCommand(
			Keysight34465ACommand command,
			ConcurrentQueue<IByteArrayConvertible> queue,
			CommandExecutionResult<Keysight34465ACommand> executionResult)
		{
			executionResult.ExecutedCommands.Add(command);

			Task<Result<CommandExecutionResult<Keysight34465ACommand>>> Identification(Keysight34465ACommand.Identification_ identification) =>
				Device.GetIdentification()
					.Map(result =>
					{
						queue.Enqueue(result);
						return executionResult;
					});

			Task<Result<CommandExecutionResult<Keysight34465ACommand>>> Read(Keysight34465ACommand.Read_ read) =>
				ProcessCommand(Keysight34465ACommand.Initiate, queue, executionResult)
					.Bind(result => ProcessCommand(Keysight34465ACommand.Fetch, queue, result));

			Task<Result<CommandExecutionResult<Keysight34465ACommand>>> Abort(Keysight34465ACommand.Abort_ abort) =>
				Device.Abort()
					.Map(_ => executionResult);

			Task<Result<CommandExecutionResult<Keysight34465ACommand>>> Initiate(Keysight34465ACommand.Initiate_ initiate) =>
				Device.Initiate()
					.Map(_ => executionResult);

			Task<Result<CommandExecutionResult<Keysight34465ACommand>>> Fetch(Keysight34465ACommand.Fetch_ fetch) =>
				Device.Fetch(queue)
					.Map(_ => executionResult);

			Task<Result<CommandExecutionResult<Keysight34465ACommand>>> ConfigureCurrent(Keysight34465ACommand.ConfigureCurrent_ configureCurrent) =>
				Device.ConfigureCurrent(configureCurrent.ElectricityType, configureCurrent.Range, configureCurrent.Resolution)
					.Map(_ => executionResult);

			Task<Result<CommandExecutionResult<Keysight34465ACommand>>> MeasureCurrent(Keysight34465ACommand.MeasureCurrent_ measureCurrent) =>
				ProcessCommand(Keysight34465ACommand.ConfigureCurrent(measureCurrent.ElectricityType, measureCurrent.Range, measureCurrent.Resolution), queue, executionResult)
					.Bind(result => ProcessCommand(Keysight34465ACommand.Read, queue, result));

			Task<Result<CommandExecutionResult<Keysight34465ACommand>>> ConfigureVoltage(Keysight34465ACommand.ConfigureVoltage_ configureVoltage) =>
				Device.ConfigureVoltage(configureVoltage.ElectricityType, configureVoltage.Range, configureVoltage.Resolution)
					.Map(_ => executionResult);

			Task<Result<CommandExecutionResult<Keysight34465ACommand>>> MeasureVoltage(Keysight34465ACommand.MeasureVoltage_ measureVoltage) =>
				ProcessCommand(Keysight34465ACommand.ConfigureVoltage(measureVoltage.ElectricityType, measureVoltage.Range, measureVoltage.Resolution), queue, executionResult)
					.Bind(result => ProcessCommand(Keysight34465ACommand.Read, queue, result));

			Task<Result<CommandExecutionResult<Keysight34465ACommand>>> DisplayText(Keysight34465ACommand.DisplayText_ displayText) =>
				ProcessCommand(Keysight34465ACommand.ClearDisplay, queue, executionResult)
					.Bind(async result =>
					{
						await Device.DisplayText(displayText.Text);
						return await ProcessCommand(Keysight34465ACommand.ClearDisplay, queue, result)
							.ConfigureAwait(false);
					});

			Task<Result<CommandExecutionResult<Keysight34465ACommand>>> ClearDisplay(Keysight34465ACommand.ClearDisplay_ clearDisplay) =>
				Device.ClearDisplay()
					.Map(_ => executionResult);

			Task<Result<CommandExecutionResult<Keysight34465ACommand>>> SetImpedance(Keysight34465ACommand.SetImpedance_ setImpedance) =>
				Device.SetImpedance(setImpedance.Impedance)
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
				MeasureVoltage,
				DisplayText,
				ClearDisplay,
				SetImpedance);
		}
	}
}