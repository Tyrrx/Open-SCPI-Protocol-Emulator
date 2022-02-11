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
			ConcurrentQueue<IStringConvertible> queue,
			CommandExecutionResult<Keysight34465ACommand> executionResult)
		{
			executionResult.ExecutedCommands.Add(command);

			Task<Result<CommandExecutionResult<Keysight34465ACommand>>> Identification(Keysight34465ACommand.Identification_ identification) =>
				device.GetIdentification()
					.Map(result =>
					{
						queue.Enqueue(result);
						return executionResult;
					});

			Task<Result<CommandExecutionResult<Keysight34465ACommand>>> Read(Keysight34465ACommand.Read_ read) =>
				ProcessCommand(device, Keysight34465ACommand.Initiate, queue, executionResult)
					.Bind(result => ProcessCommand(device, Keysight34465ACommand.Fetch, queue, result));

			Task<Result<CommandExecutionResult<Keysight34465ACommand>>> Abort(Keysight34465ACommand.Abort_ abort) =>
				device.Abort()
					.Map(_ => executionResult);

			Task<Result<CommandExecutionResult<Keysight34465ACommand>>> Initiate(Keysight34465ACommand.Initiate_ initiate) =>
				device.Initiate()
					.Map(_ => executionResult);

			Task<Result<CommandExecutionResult<Keysight34465ACommand>>> Fetch(Keysight34465ACommand.Fetch_ fetch) =>
				device.Fetch(queue)
					.Map(_ => executionResult);

			Task<Result<CommandExecutionResult<Keysight34465ACommand>>> ConfigureCurrent(Keysight34465ACommand.ConfigureCurrent_ configureCurrent) =>
				device.ConfigureCurrent(configureCurrent.ElectricCurrentType, configureCurrent.Range, configureCurrent.Resolution)
					.Map(_ => executionResult);

			Task<Result<CommandExecutionResult<Keysight34465ACommand>>> MeasureCurrent(Keysight34465ACommand.MeasureCurrent_ measureCurrent) =>
				ProcessCommand(device, Keysight34465ACommand.ConfigureCurrent(measureCurrent.ElectricCurrentType, measureCurrent.Range, measureCurrent.Resolution), queue, executionResult)
					.Bind(result => ProcessCommand(device, Keysight34465ACommand.Read, queue, result));

			Task<Result<CommandExecutionResult<Keysight34465ACommand>>> ConfigureVoltage(Keysight34465ACommand.ConfigureVoltage_ configureVoltage) =>
				device.ConfigureVoltage(configureVoltage.ElectricCurrentType, configureVoltage.Range, configureVoltage.Resolution)
					.Map(_ => executionResult);

			Task<Result<CommandExecutionResult<Keysight34465ACommand>>> MeasureVoltage(Keysight34465ACommand.MeasureVoltage_ measureVoltage) =>
				ProcessCommand(device, Keysight34465ACommand.ConfigureVoltage(measureVoltage.ElectricCurrentType, measureVoltage.Range, measureVoltage.Resolution), queue, executionResult)
					.Bind(result => ProcessCommand(device, Keysight34465ACommand.Read, queue, result));

			Task<Result<CommandExecutionResult<Keysight34465ACommand>>> DisplayText(Keysight34465ACommand.DisplayText_ displayText) =>
				ProcessCommand(device, Keysight34465ACommand.ClearDisplay, queue, executionResult)
					.Bind(async result =>
					{
						await device.DisplayText(displayText.Text);
						return await ProcessCommand(device, Keysight34465ACommand.ClearDisplay, queue, result)
							.ConfigureAwait(false);
					});

			Task<Result<CommandExecutionResult<Keysight34465ACommand>>> ClearDisplay(Keysight34465ACommand.ClearDisplay_ clearDisplay) =>
				device.ClearDisplay()
					.Map(_ => executionResult);

			Task<Result<CommandExecutionResult<Keysight34465ACommand>>> SetImpedance(Keysight34465ACommand.SetImpedance_ setImpedance) =>
				device.SetImpedance(setImpedance.Impedance)
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