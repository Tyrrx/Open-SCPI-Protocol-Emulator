using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Emulator.Controller;
using FunicularSwitch;

namespace Emulator.Command
{
    public class
        CommandExecutor<TCommand, TOutputQueue> : ICommandExecutor<TCommand, TOutputQueue>
    {
        private readonly ConcurrentQueue<TOutputQueue> outputQueue = new ConcurrentQueue<TOutputQueue>();
        private readonly IDeviceController<TCommand, TOutputQueue> deviceController;

        public CommandExecutor(IDeviceController<TCommand, TOutputQueue> deviceController)
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