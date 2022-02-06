using System;
using System.Threading.Tasks;
using Domain.Keysight34465A;
using Domain.Keysight3458A;
using Emulator;
using Emulator.Command;
using Emulator.CommandHandler;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PluginConfiguration;
using PluginHosting;
using PluginInterpreter;
using PluginNetwork;
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
                    services.AddTransient<IServer, TcpServer>();
                    
                    services.AddSingleton(deviceConfigurations.Keysight34465AConfiguration);
                    services.AddTransient<IProtocolInterpreter<Keysight34465ACommand>, Keysight34465AProtocolInterpreter>();
                    services.AddTransient<Keysight34465A>();
                    services.AddTransient<ICommandHandler<Keysight34465ACommand>, Keysight34465ACommandHandler>();
                    services.AddTransient<GenericDevice<Keysight34465ACommand, Keysight34465AConfiguration>>();
                    services.AddHostedService<HostedDeviceService<Keysight34465ACommand, Keysight34465AConfiguration>>();
                    
                    services.AddSingleton(deviceConfigurations.Keysight3458AConfiguration);
                    services.AddTransient<IProtocolInterpreter<Keysight3458ACommand>, Keysight3458AProtocolInterpreter>();
                    services.AddTransient<Keysight3458A>();
                    services.AddTransient<ICommandHandler<Keysight3458ACommand>, Keysight3458ACommandHandler>();
                    services.AddTransient<GenericDevice<Keysight3458ACommand, Keysight3458AConfiguration>>();
                    services.AddHostedService<HostedDeviceService<Keysight3458ACommand, Keysight3458AConfiguration>>();
                });
        }
    }
}