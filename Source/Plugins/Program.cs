using System;
using System.Threading.Tasks;
using Domain.Keysight34465A;
using Emulator;
using Emulator.Command;
using Emulator.CommandHandler;
using Emulator.Factories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PluginConfiguration;
using PluginHosting;
using PluginInterpreter;
using PluginNetwork;
using PluginPersistence;
using static PluginConfiguration.DeviceConfigurationLoader;

namespace EmulatorHost
{
    // ReSharper disable once ClassNeverInstantiated.Global
    // ReSharper disable once ArrangeTypeModifiers
    class Program
    {
        private const string DeviceSettingsJsonFileName = "devicesettings.json";

        // ReSharper disable once ArrangeTypeMemberModifiers
        static Task Main(string[] args)
        {
            return LoadDeviceConfigurations(args, DeviceSettingsJsonFileName)
                .Map(deviceConfigurations =>
                    CreateHostBuilder(deviceConfigurations)
                        .Build()
                        .RunAsync())
                .Match(
                    ok => ok,
                    error =>
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(error);
                        Console.ForegroundColor = ConsoleColor.White;
                        return Task.CompletedTask;
                    });
        }

        private static IHostBuilder CreateHostBuilder(DeviceConfigurations deviceConfigurations)
        {
            return Host.CreateDefaultBuilder( /*args*/)
                .ConfigureServices(services =>
                {
                    services.AddTransient<IServerFactory, ServerFactory>();
                    
                    services.AddSingleton(deviceConfigurations.Keysight34465AConfiguration);
                    services.AddTransient<IProtocolParser<Keysight34465ACommand>, Keysight34465AProtocolParser>();
                    services.AddTransient<ICommandHandler<Keysight34465A, Keysight34465ACommand>, Keysight34465ACommandHandler>();
                    services.AddTransient<IMeasuringInstrumentFactory<Keysight34465A, Keysight34465AConfiguration>, Keysight34465AFactory>();
                    services.AddTransient<MeasuringInstrumentExecutionService<Keysight34465A, Keysight34465ACommand>>();
                    services.AddSingleton<IMeasuringInstrumentRepository<Keysight34465A>, MeasuringInstrumentRepository<Keysight34465A>>();
                    services.AddSingleton<MeasuringInstrumentInstanceManager<Keysight34465A,Keysight34465ACommand,Keysight34465AConfiguration>>();
                    services.AddHostedService<HostedMeasuringInstrumentInstanceManager<Keysight34465A,Keysight34465ACommand,Keysight34465AConfiguration>>();
                    
                    
                    // services.AddSingleton(deviceConfigurations.Keysight3458AConfiguration);
                    // services.AddTransient<IProtocolInterpreter<Keysight3458ACommand>, Keysight3458AProtocolInterpreter>();
                    // services.AddTransient<Keysight3458A>();
                    // services.AddTransient<ICommandHandler<Keysight3458A, Keysight3458ACommand>, Keysight3458ACommandHandler>();
                    // services.AddTransient<MeasurementDeviceExecutionService<Keysight3458ACommand, Keysight3458AConfiguration>>();
                    // services.AddHostedService<HostedMeasurementDeviceInstanceManager<,,>>();
                });
        }
    }
}