using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reactive.Subjects;
using Domain.UnionTypes;

namespace Domain.Interfaces
{
    public interface IKeysight34Series
    {
        public BehaviorSubject<TriggerState> TriggerStateBehaviourSubject { get; }
        
        public ConcurrentQueue<MeasurementValue> ReadingQueue { get; }
        
        public Queue<double> GeneratorQueue { get; set; }
        
        public BehaviorSubject<ElectricalUnitOfMeasure> ElectricalUnitOfMeasureBehaviorSubject { get; }
        
        public BehaviorSubject<ElectricCurrentType> ElectricCurrentTypeBehaviorSubject { get; }
        
        public BehaviorSubject<Impedance> ImpedanceBehaviorSubject { get; }
        
        public BehaviorSubject<Range> RangeBehaviorSubject { get; }
        
        public BehaviorSubject<Resolution> ResolutionBehaviorSubject { get; }
        
    }
}