using System.Collections.Generic;

namespace Emulator.Command
{
	public class CommandExecutionResult<TCommand>
	{
		public List<TCommand> ExecutedCommands { get; } = new List<TCommand>();
	}
}