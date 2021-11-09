using System.Collections.Concurrent;
using System.Threading.Tasks;
using Emulator.Command;
using Emulator.Controller;
using FunicularSwitch;

namespace Protocol.Execution
{
    public class
        CommandExecutionAdapter<TCommand, TOutput> : ICommandExecutionAdapter<TCommand, TOutput>
    {
        private readonly ConcurrentQueue<TOutput> outputQueue = new ConcurrentQueue<TOutput>();
        private readonly ICommandHandler<TCommand, TOutput> commandHandler;

        public CommandExecutionAdapter(ICommandHandler<TCommand, TOutput> commandHandler)
        {
            this.commandHandler = commandHandler;
        }
        
        public Task<Result<CommandExecutionResult<TCommand>>> Execute(TCommand command)
        {
            return commandHandler.ProcessCommand(command, outputQueue, new CommandExecutionResult<TCommand>());
        }
        public ConcurrentQueue<TOutput> GetOutputQueue() => outputQueue;
    }
}