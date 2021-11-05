using System.Collections.Concurrent;
using System.Threading.Tasks;
using Emulator.Command;
using FunicularSwitch;

namespace Protocol.Execution
{
    public interface ICommandExecutionAdapter<TCommand, TOutputQueue>
    {
        Task<Result<CommandExecutionResult<TCommand>>> Execute(TCommand command);

        ConcurrentQueue<TOutputQueue> GetOutputQueue();
    }
}