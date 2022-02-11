using FunicularSwitch;

namespace Emulator
{
    public interface IProtocolParser<TCommand>
    {
        Result<TCommand> GetCommand(string input);
    }
}