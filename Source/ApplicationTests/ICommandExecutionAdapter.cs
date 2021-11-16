using System.Collections.Concurrent;
using System.Threading.Tasks;
using Domain.Interfaces;
using Emulator.Command;
using FunicularSwitch;

namespace EmulatorTests
{
    public interface ICommandExecutionAdapter<TCommand>
    {
        Task<Result<CommandExecutionResult<TCommand>>> Execute(TCommand command);

        ConcurrentQueue<IStringConvertible> GetOutputQueue();
    }
}