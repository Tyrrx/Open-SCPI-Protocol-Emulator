using System.Collections.Generic;
using System.Reactive.Subjects;
using Domain.UnionTypes;

namespace Domain.Interfaces
{
    public interface IElectricalMeasuringSubsystem
    {
        public BehaviorSubject<ElectricalUnitOfMeasure> ElectricalUnitOfMeasureBehaviorSubject { get; }

        public BehaviorSubject<ElectricCurrentType> ElectricCurrentTypeBehaviorSubject { get; }

        public BehaviorSubject<Impedance> ImpedanceBehaviorSubject { get; }

        public BehaviorSubject<Range> RangeBehaviorSubject { get; }

        public BehaviorSubject<Resolution> ResolutionBehaviorSubject { get; }
    }
}