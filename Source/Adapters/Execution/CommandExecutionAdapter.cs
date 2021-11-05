using System.Collections.Concurrent;
using System.Threading.Tasks;
using Emulator.Command;
using Emulator.Controller;
using FunicularSwitch;

namespace Protocol.Execution
{
    public class
        CommandExecutionAdapter<TCommand, TOutputQueue> : ICommandExecutionAdapter<TCommand, TOutputQueue>
    {
        private readonly ConcurrentQueue<TOutputQueue> outputQueue = new ConcurrentQueue<TOutputQueue>();
        private readonly IDeviceController<TCommand, TOutputQueue> deviceController;

        public CommandExecutionAdapter(IDeviceController<TCommand, TOutputQueue> deviceController)
        {
            this.deviceController = deviceController;
        }
        
        public Task<Result<CommandExecutionResult<TCommand>>> Execute(TCommand command)
        {
            return deviceController.ProcessCommand(command, outputQueue, new CommandExecutionResult<TCommand>());
        }

        public ConcurrentQueue<TOutputQueue> GetOutputQueue() => outputQueue;
    }
}