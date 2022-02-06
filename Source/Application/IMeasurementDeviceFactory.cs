namespace Emulator
{
    public interface IMeasurementDeviceFactory<TMeasurementDevice, TConfiguration>
    {
        public TMeasurementDevice Create(TConfiguration configuration);
    }
}