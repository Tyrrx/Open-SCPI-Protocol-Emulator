using System.Collections.Generic;
using Domain.Interfaces;
using Emulator;
using FunicularSwitch;
using FunicularSwitch.Extensions;

namespace PluginPersistence
{
    public class MeasuringInstrumentRepository<TMeasurementDevice> : IMeasuringInstrumentRepository<TMeasurementDevice>
    where TMeasurementDevice: IMeasuringInstrument
    {
        private readonly Dictionary<string, TMeasurementDevice> measuringInstruments =
            new Dictionary<string, TMeasurementDevice>();

        public Option<TMeasurementDevice> GetByIdentification(string identification)
        {
            return measuringInstruments.TryGetValue(identification);
        }

        public Option<TMeasurementDevice> GetByConfiguration(IDeviceConfiguration configuration)
        {
            return measuringInstruments.TryGetValue(configuration.Identification);
        }

        public void Persist(TMeasurementDevice measurementDevice)
        {
            measuringInstruments.Add(measurementDevice.GetIdentification(), measurementDevice);
        }
    }
}