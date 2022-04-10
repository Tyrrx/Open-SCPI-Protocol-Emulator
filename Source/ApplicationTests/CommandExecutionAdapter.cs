using System.Collections.Concurrent;
using System.Threading.Tasks;
using Domain.Interfaces;
using Emulator.Command;
using Emulator.CommandHandler;
using FunicularSwitch;

namespace EmulatorTests
{
    public class CommandExecutionAdapter<TDevice, TCommand> : ICommandExecutionAdapter<TCommand>
    {
        private readonly ConcurrentQueue<IStringConvertible> outputQueue = new ConcurrentQueue<IStringConvertible>();
        private readonly ICommandHandler<TDevice, TCommand> commandHandler;
        private readonly TDevice device;

        public CommandExecutionAdapter(ICommandHandler<TDevice, TCommand> commandHandler, TDevice device)
        {
            this.commandHandler = commandHandler;
            this.device = device;
        }
        
        public Task<Result<CommandExecutionResult<TCommand>>> Execute(TCommand command)
        {
            return commandHandler.ProcessCommand(device, command, outputQueue, new CommandExecutionResult<TCommand>());
        }
        public ConcurrentQueue<IStringConvertible> GetOutputQueue() => outputQueue;
    }
}