using System;
using System.IO;
using FunicularSwitch;

namespace PluginConfiguration
{
    public static class DeviceConfigurationLoader
    {
        public static Result<DeviceConfigurations> LoadDeviceConfigurations(string[] args, string fileName)
        {
            var configurationSerializer = new DeviceConfigurationSerializer();

            if (args.Length > 0 && args[0] != null)
            {
                return configurationSerializer.DeserializeDeviceConfigurations(args[0]);
            }

            var configFilePath = Path.Combine(Environment.CurrentDirectory, fileName);
            return File.Exists(configFilePath)
                ? configurationSerializer.DeserializeDeviceConfigurations(File.ReadAllText(configFilePath))
                : Result.Error<DeviceConfigurations>(
                    $"Missing device configuration file {configFilePath}. Pass configuration as start parameters or make sure the config file is present.");
        }
    }
}