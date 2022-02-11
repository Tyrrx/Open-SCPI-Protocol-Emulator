namespace Emulator
{
    public interface IMeasuringInstrumentFactory<TMeasurementDevice, TConfiguration>
    {
        public TMeasurementDevice Create(TConfiguration configuration);
    }
}