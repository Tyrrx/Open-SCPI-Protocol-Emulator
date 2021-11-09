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
        private readonly IDeviceController<TCommand, TOutput> deviceController;

        public CommandExecutionAdapter(IDeviceController<TCommand, TOutput> deviceController)
        {
            this.deviceController = deviceController;
        }
        
        public Task<Result<CommandExecutionResult<TCommand>>> Execute(TCommand command)
        {
            return deviceController.ProcessCommand(command, outputQueue, new CommandExecutionResult<TCommand>());
        }
        public ConcurrentQueue<TOutput> GetOutputQueue() => outputQueue;
    }
}