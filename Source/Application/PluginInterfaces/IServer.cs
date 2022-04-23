using System;
using System.Collections.Concurrent;
using Domain.Interfaces;

namespace Emulator
{
    public interface IServer
    {
        public IObservable<string> GetInputStream();
        public ConcurrentQueue<IStringConvertible> GetOutputQueue();
        public void Start(IServerConfiguration serverConfiguration);
        public void Stop();
    }
}