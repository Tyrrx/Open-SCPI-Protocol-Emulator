using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Interfaces;

namespace Emulator
{
    public class MeasurementDeviceInstanceManager<TMeasurementDevice, TCommand, TConfiguration>
        where TConfiguration : IDeviceConfiguration
    {
        private readonly IServerFactory serverFactory;

        private readonly MeasurementDeviceExecutionService<TMeasurementDevice, TCommand>
            measurementDeviceExecutionService;

        private readonly List<TConfiguration> configurations;
        private readonly IMeasurementDeviceFactory<TMeasurementDevice, TConfiguration> measurementDeviceFactory;
        private readonly IMeasurementDeviceRepository<TMeasurementDevice> measurementDeviceRepository;

        private List<IDisposable> subscriptions = new List<IDisposable>();

        private readonly List<IServer> servers = new List<IServer>();

        public MeasurementDeviceInstanceManager(
            IServerFactory serverFactory,
            MeasurementDeviceExecutionService<TMeasurementDevice, TCommand> measurementDeviceExecutionService,
            List<TConfiguration> configurations,
            IMeasurementDeviceFactory<TMeasurementDevice, TConfiguration> measurementDeviceFactory,
            IMeasurementDeviceRepository<TMeasurementDevice> measurementDeviceRepository
        )
        {
            this.serverFactory = serverFactory;
            this.measurementDeviceExecutionService = measurementDeviceExecutionService;
            this.configurations = configurations;
            this.measurementDeviceFactory = measurementDeviceFactory;
            this.measurementDeviceRepository = measurementDeviceRepository;
        }

        public Task StartAll()
        {
            subscriptions = configurations.Select(Start).ToList();
            return Task.CompletedTask;
        }

        public Task StopAll()
        {
            subscriptions.ForEach(sub => sub.Dispose());
            servers.ForEach(server => server.Stop());
            return Task.CompletedTask;
        }

        private IDisposable Start(TConfiguration configuration)
        {
            var server = serverFactory.Create();
            servers.Add(server);

            var device = measurementDeviceRepository.GetByConfiguration(configuration)
                .Match(
                    some => some,
                    () => measurementDeviceFactory.Create(configuration));
            var subscription = measurementDeviceExecutionService.BeginExecution(
                server.GetInputStream(),
                server.GetOutputQueue(),
                device);
            server.Start(configuration);
            return subscription;
        }
    }
}