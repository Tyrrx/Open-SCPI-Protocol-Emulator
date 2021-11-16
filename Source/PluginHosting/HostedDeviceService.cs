using System.Threading;
using System.Threading.Tasks;
using Domain.Interfaces;
using Emulator;
using Microsoft.Extensions.Hosting;

namespace PluginHosting
{
    public class HostedDeviceService<TCommandType, TOfIDeviceConfigurationType> : IHostedService
        where TOfIDeviceConfigurationType : IDeviceConfiguration
    {
        private readonly GenericDevice<TCommandType, TOfIDeviceConfigurationType> genericDevice;

        public HostedDeviceService(GenericDevice<TCommandType, TOfIDeviceConfigurationType> genericDevice)
        {
            this.genericDevice = genericDevice;
        }
        
        public Task StartAsync(CancellationToken cancellationToken)
        {
            return genericDevice.StartAsync();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return genericDevice.StopAsync();
        }
    }
}