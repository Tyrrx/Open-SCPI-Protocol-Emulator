using Emulator.Command;
using FunicularSwitch;

namespace Protocol.Interpreter
{
    public interface IProtocolInterpreter<TCommand>
    {
        Result<TCommand> GetCommand(string input);
    }
}