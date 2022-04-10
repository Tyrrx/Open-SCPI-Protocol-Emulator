using System.Collections.Concurrent;
using System.Threading.Tasks;
using Domain.Interfaces;
using Emulator.Command;
using FunicularSwitch;

namespace Emulator.CommandHandler
{
	public interface ICommandHandler<TDeviceType, TCommandInput>
	{
		Task<Result<CommandExecutionResult<TCommandInput>>> ProcessCommand(
			TDeviceType device,
			TCommandInput command,
			ConcurrentQueue<IStringConvertible> outputQueue,
			CommandExecutionResult<TCommandInput> executionResult);
		//todo add cancelation token
	}
}