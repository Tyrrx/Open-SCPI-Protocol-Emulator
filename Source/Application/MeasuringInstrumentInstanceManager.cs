using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Interfaces;

namespace Emulator
{
    // Todo: this is a repository and should be called repository
    // Todo: StartAll, StopAll and Start methods should be services and not part of this repository
    public class MeasuringInstrumentInstanceManager<TMeasurementDevice, TCommand, TConfiguration>
        where TConfiguration : IDeviceConfiguration
    {
        private readonly IServerFactory serverFactory;

        private readonly MeasuringInstrumentExecutionService<TMeasurementDevice, TCommand>
            measuringInstrumentExecutionService;

        private readonly List<TConfiguration> configurations;
        private readonly IMeasuringInstrumentFactory<TMeasurementDevice, TConfiguration> measuringInstrumentFactory;
        private readonly IMeasuringInstrumentRepository<TMeasurementDevice> measuringInstrumentRepository;

        // Todo how to handle these disposables?
        private List<IDisposable> subscriptions = new List<IDisposable>();
        
        private readonly List<IServer> servers = new List<IServer>();

        public MeasuringInstrumentInstanceManager(
            IServerFactory serverFactory,
            MeasuringInstrumentExecutionService<TMeasurementDevice, TCommand> measuringInstrumentExecutionService,
            List<TConfiguration> configurations,
            IMeasuringInstrumentFactory<TMeasurementDevice, TConfiguration> measuringInstrumentFactory,
            IMeasuringInstrumentRepository<TMeasurementDevice> measuringInstrumentRepository
        )
        {
            this.serverFactory = serverFactory;
            this.measuringInstrumentExecutionService = measuringInstrumentExecutionService;
            this.configurations = configurations;
            this.measuringInstrumentFactory = measuringInstrumentFactory;
            this.measuringInstrumentRepository = measuringInstrumentRepository;
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

            var device = measuringInstrumentRepository.GetByConfiguration(configuration)
                .Match(
                    some => some,
                    () => measuringInstrumentFactory.Create(configuration));
            var subscription = measuringInstrumentExecutionService.BeginExecution(
                server.GetInputStream(),
                server.GetOutputQueue(),
                device);
            server.Start(configuration);
            return subscription;
        }
    }
}