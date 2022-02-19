using Domain.Keysight3458A;

namespace Emulator.Factories
{
    public class Keysight3458AFactory : IMeasuringInstrumentFactory<Keysight3458A, Keysight3458AConfiguration>
    {
        public Keysight3458A Create(Keysight3458AConfiguration configuration)
        {
            return new Keysight3458A(configuration);
        }
    }
}