using System.Collections.Generic;
using Domain.Interfaces;
using Emulator;
using FunicularSwitch;
using FunicularSwitch.Extensions;

namespace PluginPersistence
{
    public class MeasurementDeviceRepository<TMeasurementDevice> : IMeasurementDeviceRepository<TMeasurementDevice>
    where TMeasurementDevice: IMeasurementDevice
    {
        private readonly Dictionary<string, TMeasurementDevice> measurementDevices =
            new Dictionary<string, TMeasurementDevice>();

        public Option<TMeasurementDevice> GetByIdentification(string identification)
        {
            return measurementDevices.TryGetValue(identification);
        }

        public Option<TMeasurementDevice> GetByConfiguration(IDeviceConfiguration configuration)
        {
            return measurementDevices.TryGetValue(configuration.Identification);
        }

        public void Persist(TMeasurementDevice measurementDevice)
        {
            measurementDevices.Add(measurementDevice.GetIdentification(), measurementDevice);
        }
    }
}