using FunicularSwitch;

namespace Emulator
{
    public interface IProtocolInterpreter<TCommand>
    {
        Result<TCommand> GetCommand(string input);
    }
}