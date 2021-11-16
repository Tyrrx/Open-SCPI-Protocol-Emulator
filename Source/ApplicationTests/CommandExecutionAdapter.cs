using System.Collections.Concurrent;
using System.Threading.Tasks;
using Domain.Interfaces;
using Emulator.Command;
using Emulator.CommandHandler;
using FunicularSwitch;

namespace EmulatorTests
{
    public class
        CommandExecutionAdapter<TCommand> : ICommandExecutionAdapter<TCommand>
    {
        private readonly ConcurrentQueue<IStringConvertible> outputQueue = new ConcurrentQueue<IStringConvertible>();
        private readonly ICommandHandler<TCommand> commandHandler;

        public CommandExecutionAdapter(ICommandHandler<TCommand> commandHandler)
        {
            this.commandHandler = commandHandler;
        }
        
        public Task<Result<CommandExecutionResult<TCommand>>> Execute(TCommand command)
        {
            return commandHandler.ProcessCommand(command, outputQueue, new CommandExecutionResult<TCommand>());
        }
        public ConcurrentQueue<IStringConvertible> GetOutputQueue() => outputQueue;
    }
}