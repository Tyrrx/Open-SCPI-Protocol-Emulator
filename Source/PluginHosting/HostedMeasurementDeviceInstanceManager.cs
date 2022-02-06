using System.Threading;
using System.Threading.Tasks;
using Domain.Interfaces;
using Emulator;
using Microsoft.Extensions.Hosting;

namespace PluginHosting
{
    public class HostedMeasurementDeviceInstanceManager<TMeasurementDevice, TCommand, TConfiguration> : IHostedService
        where TConfiguration : IDeviceConfiguration
    {
        private readonly MeasurementDeviceInstanceManager<TMeasurementDevice, TCommand, TConfiguration> measurementDeviceInstanceManager;

        public HostedMeasurementDeviceInstanceManager(MeasurementDeviceInstanceManager<TMeasurementDevice, TCommand, TConfiguration> measurementDeviceInstanceManager)
        {
            this.measurementDeviceInstanceManager = measurementDeviceInstanceManager;
        }
        
        public Task StartAsync(CancellationToken cancellationToken)
        {
            return measurementDeviceInstanceManager.StartAll();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return measurementDeviceInstanceManager.StopAll();
        }
    }
}