using System.Collections.Concurrent;
using System.Collections.Generic;
using Domain.UnionTypes;

namespace Domain.Interfaces
{
    public interface IValueGenerationSubsystem
    {
        public ConcurrentQueue<MeasurementValue> ReadingQueue { get; }

        public Queue<double> GeneratorQueue { get; set; }

        public double GenerateMeasurementValue(double interferenceMultiplier);
    }
}