using Domain.Keysight34465A;

namespace Emulator.Factories
{
    public class Keysight34465AFactory : IMeasuringInstrumentFactory<Keysight34465A, Keysight34465AConfiguration>
    {
        public Keysight34465A Create(Keysight34465AConfiguration configuration)
        {
            return new Keysight34465A(configuration);
        }
    }
}