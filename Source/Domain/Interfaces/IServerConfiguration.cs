namespace Domain.Interfaces
{
    public interface IServerConfiguration
    {
        public string Ip { get; init; }
        public int Port { get; init; }
    }
}