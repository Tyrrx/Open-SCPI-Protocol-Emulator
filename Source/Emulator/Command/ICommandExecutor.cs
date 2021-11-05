using System.Collections.Concurrent;
using System.Threading.Tasks;
using FunicularSwitch;

namespace Emulator.Command
{
    public interface ICommandExecutor<TCommand, TOutputQueue>
    {
        Task<Result<CommandExecutionResult<TCommand>>> Execute(TCommand command);

        ConcurrentQueue<TOutputQueue> GetOutputQueue();
    }
}