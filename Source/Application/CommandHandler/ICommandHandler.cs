using System.Collections.Concurrent;
using System.Threading.Tasks;
using Domain.Interfaces;
using Emulator.Command;
using FunicularSwitch;

namespace Emulator.CommandHandler
{
	public interface ICommandHandler<TCommandInput>
	{
		Task<Result<CommandExecutionResult<TCommandInput>>> ProcessCommand(
			TCommandInput command,
			ConcurrentQueue<IStringConvertible> queue,
			CommandExecutionResult<TCommandInput> executionResult);
		//todo add cancelation token
	}
}