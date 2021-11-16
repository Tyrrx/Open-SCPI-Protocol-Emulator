namespace Domain.Interfaces
{
	public interface IDeviceConfiguration : IServerConfiguration
	{
		public string Identification { get; init; }
	}
}