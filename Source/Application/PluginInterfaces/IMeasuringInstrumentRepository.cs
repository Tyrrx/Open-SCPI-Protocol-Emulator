using Domain.Interfaces;
using FunicularSwitch;

namespace Emulator
{
    public interface IMeasuringInstrumentRepository<TMeasurementDevice>
    {
        public Option<TMeasurementDevice> GetByIdentification(string identification);
        public Option<TMeasurementDevice> GetByConfiguration(IDeviceConfiguration configuration);

        public void Persist(TMeasurementDevice measurementDevice);
    }
}