using System.Threading;
using System.Threading.Tasks;
using Domain.Interfaces;
using Emulator;
using Microsoft.Extensions.Hosting;

namespace PluginHosting
{
    public class HostedMeasuringInstrumentInstanceManager<TMeasurementDevice, TCommand, TConfiguration> : IHostedService
        where TConfiguration : IDeviceConfiguration
    {
        private readonly MeasuringInstrumentInstanceManager<TMeasurementDevice, TCommand, TConfiguration> measuringInstrumentInstanceManager;

        public HostedMeasuringInstrumentInstanceManager(MeasuringInstrumentInstanceManager<TMeasurementDevice, TCommand, TConfiguration> measuringInstrumentInstanceManager)
        {
            this.measuringInstrumentInstanceManager = measuringInstrumentInstanceManager;
        }
        
        public Task StartAsync(CancellationToken cancellationToken)
        {
            return measuringInstrumentInstanceManager.StartAll();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return measuringInstrumentInstanceManager.StopAll();
        }
    }
}