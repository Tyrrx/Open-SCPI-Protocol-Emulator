using System.Collections.Concurrent;
using System.Threading.Tasks;
using Emulator.Command;
using FunicularSwitch;

namespace Emulator.Controller
{
	public interface ICommandHandler<TCommandInput, TExecutionResultOutput>
	{
		Task<Result<CommandExecutionResult<TCommandInput>>> ProcessCommand(
			TCommandInput command,
			ConcurrentQueue<TExecutionResultOutput> queue,
			CommandExecutionResult<TCommandInput> executionResult);
		//todo add cancelation token
	}
}