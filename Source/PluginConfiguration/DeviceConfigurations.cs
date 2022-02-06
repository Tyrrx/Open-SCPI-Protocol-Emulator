using System.Collections.Generic;
using Domain.Keysight34465A;
using Domain.Keysight3458A;
using Newtonsoft.Json;

namespace PluginConfiguration
{
	public sealed class DeviceConfigurations
	{
		public List<Keysight34465AConfiguration> Keysight34465AConfiguration { get; set; }
		
		public List<Keysight3458AConfiguration> Keysight3458AConfiguration { get; set; }
	}
}