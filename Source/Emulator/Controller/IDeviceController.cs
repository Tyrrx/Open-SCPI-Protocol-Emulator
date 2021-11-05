using System.Collections.Concurrent;
using System.Threading.Tasks;
using Emulator.Command;
using FunicularSwitch;

namespace Emulator.Controller
{
	public interface IDeviceController<TCommand, TOutputQueue>
	{
		Task<Result<CommandExecutionResult<TCommand>>> ProcessCommand(
			TCommand command,
			ConcurrentQueue<TOutputQueue> queue,
			CommandExecutionResult<TCommand> executionResult);
		//todo add cancelation token
	}
}